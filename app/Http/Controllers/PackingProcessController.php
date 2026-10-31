<?php

namespace App\Http\Controllers;

use App\Models\PackingProcess;
use Illuminate\Contracts\View\View;
use Illuminate\Database\Eloquent\Builder;
use Illuminate\Http\JsonResponse;
use Illuminate\Http\RedirectResponse;
use Illuminate\Http\Request;
use Illuminate\Http\Response;
use Illuminate\Support\Facades\DB;
use Illuminate\Validation\Rule;

class PackingProcessController extends Controller
{
    public function dashboard(Request $request): View
    {
        $locations = collect(range(1, 7))
            ->map(fn (int $number): string => "WP-{$number}");

        $validated = $request->validate([
            'location' => ['nullable', 'string', Rule::in($locations->all())],
            'search' => ['nullable', 'string', 'max:100'],
        ]);

        $selectedLocation = $validated['location'] ?? 'WP-1';
        $search = $validated['search'] ?? null;

        $filteredProcesses = fn (): Builder => PackingProcess::query()
            ->where('location', $selectedLocation)
            ->when($search, function (Builder $query) use ($search): void {
                $query->where(function (Builder $query) use ($search): void {
                    $query->where('model', 'like', "%{$search}%")
                        ->orWhere('no_hu', 'like', "%{$search}%")
                        ->orWhere('customer', 'like', "%{$search}%")
                        ->orWhere('box_id', 'like', "%{$search}%");
                });
            });

        $activeProcesses = $filteredProcesses()
            ->whereNull('time_out')
            ->latest('id')
            ->paginate(5, ['*'], 'active_page')
            ->withQueryString();

        $historyProcesses = $filteredProcesses()
            ->latest('id')
            ->paginate(10, ['*'], 'history_page')
            ->withQueryString();

        $waitingSummary = PackingProcess::query()
            ->where('location', $selectedLocation)
            ->whereNull('time_out')
            ->selectRaw('COALESCE(SUM(qty), 0) as qty, COUNT(*) as boxes')
            ->first();

        $outSummary = PackingProcess::query()
            ->where('location', $selectedLocation)
            ->whereDate('time_out', today())
            ->selectRaw('COALESCE(SUM(qty), 0) as qty, COUNT(*) as boxes')
            ->first();

        $activeBoxCount = PackingProcess::query()
            ->where('location', $selectedLocation)
            ->whereNull('time_out')
            ->count();

        $outAvailableCount = PackingProcess::query()
            ->where('location', $selectedLocation)
            ->whereNull('time_out')
            ->whereNotNull('no_hu')
            ->where('no_hu', '<>', '')
            ->count();

        $boxIdPattern = '/^'.preg_quote($selectedLocation, '/').'-([0-9]+)$/';
        $activeBoxNumbers = PackingProcess::query()
            ->where('location', $selectedLocation)
            ->whereNull('time_out')
            ->pluck('box_id')
            ->map(function (string $boxId) use ($boxIdPattern): ?int {
                return preg_match($boxIdPattern, $boxId, $matches) === 1
                    ? (int) $matches[1]
                    : null;
            })
            ->filter()
            ->unique()
            ->values();

        $nextBoxNumber = 1;

        while ($activeBoxNumbers->contains($nextBoxNumber)) {
            $nextBoxNumber++;
        }

        return view('packing.index', compact(
            'activeProcesses',
            'activeBoxCount',
            'activeBoxNumbers',
            'outAvailableCount',
            'historyProcesses',
            'locations',
            'nextBoxNumber',
            'outSummary',
            'search',
            'selectedLocation',
            'waitingSummary',
        ));
    }

    public function storeFromDashboard(Request $request): RedirectResponse
    {
        $validated = $request->validate([
            'location' => ['required', 'string', Rule::in(collect(range(1, 7))->map(fn (int $number): string => "WP-{$number}")->all())],
            'customer' => ['required', 'string', 'max:100'],
            'remarks' => ['nullable', 'string', 'max:5000'],
            'boxes' => ['required', 'array', 'min:1', 'max:50'],
            'boxes.*' => ['required', 'array'],
            'boxes.*.customer' => ['nullable', 'string', 'max:100'],
            'boxes.*.model' => ['required', 'string', 'max:100'],
            'boxes.*.no_hu' => ['nullable', 'string', 'max:100', 'distinct:strict', Rule::unique('packing_processes', 'no_hu')],
            'boxes.*.box_id' => ['required', 'string', 'max:100'],
            'boxes.*.qty' => ['required', 'integer', 'min:1'],
        ], [
            'boxes.*.no_hu.distinct' => 'No HU tidak boleh digunakan lebih dari satu kali dalam proses Packing IN.',
            'boxes.*.no_hu.unique' => 'No HU sudah terdaftar pada data Packing.',
        ]);

        DB::transaction(function () use ($validated): void {
            $timeIn = now();

            foreach ($validated['boxes'] as $box) {
                PackingProcess::query()->create([
                    ...$box,
                    'customer' => $box['customer'] ?? $validated['customer'],
                    'location' => $validated['location'],
                    'remarks' => $validated['remarks'] ?? null,
                    'process_status' => 'IN',
                    'time_in' => $timeIn,
                ]);
            }
        });

        return to_route('packing.index', ['location' => $validated['location']])
            ->with('success', count($validated['boxes']).' box Packing IN berhasil disimpan.');
    }

    public function markOut(PackingProcess $packingProcess): RedirectResponse
    {
        $wasMarkedOut = DB::transaction(function () use ($packingProcess): bool {
            $process = PackingProcess::query()
                ->whereKey($packingProcess->getKey())
                ->lockForUpdate()
                ->firstOrFail();

            if ($process->time_out !== null) {
                return false;
            }

            $process->update([
                'process_status' => 'OUT',
                'time_out' => now(),
            ]);

            return true;
        });

        if (! $wasMarkedOut) {
            return to_route('packing.index', ['location' => $packingProcess->location])
                ->withErrors(['packing_process' => 'Packing ini sudah berstatus OUT.']);
        }

        return to_route('packing.index', ['location' => $packingProcess->location])
            ->with('success', 'Packing OUT berhasil disimpan.');
    }

    public function updateFromDashboard(Request $request, PackingProcess $packingProcess): RedirectResponse
    {
        $validated = $request->validate([
            'model' => ['required', 'string', 'max:100'],
            'no_hu' => ['nullable', 'string', 'max:100', Rule::unique('packing_processes', 'no_hu')->ignore($packingProcess->getKey())],
            'qty' => ['required', 'integer', 'min:1'],
        ], [
            'no_hu.unique' => 'No HU sudah terdaftar pada data Packing.',
        ]);

        $wasUpdated = DB::transaction(function () use ($packingProcess, $validated): bool {
            $process = PackingProcess::query()
                ->whereKey($packingProcess->getKey())
                ->lockForUpdate()
                ->firstOrFail();

            if ($process->time_out !== null) {
                return false;
            }

            $process->update($validated);

            return true;
        });

        if (! $wasUpdated) {
            return to_route('packing.index', ['location' => $packingProcess->location])
                ->withErrors(['packing_process' => 'Packing yang sudah OUT tidak dapat diedit.']);
        }

        return to_route('packing.index', ['location' => $packingProcess->location])
            ->with('success', 'Data packing berhasil diperbarui.');
    }

    public function scanForIn(Request $request): JsonResponse
    {
        $validated = $request->validate([
            'barcode' => ['required', 'string', 'max:255'],
        ]);

        $segments = explode('@', $validated['barcode']);

        if (
            ! str_starts_with($validated['barcode'], '@')
            || count($segments) < 5
            || preg_match('/^\d{4}/', $segments[1]) !== 1
            || strlen($segments[2]) < 15
            || $segments[3] === ''
        ) {
            return response()->json([
                'message' => 'Format barcode tidak valid.',
            ], 422);
        }

        $customerCode = substr($segments[1], 0, 4);
        $customerName = DB::table('customers')
            ->where('customer_code', $customerCode)
            ->value('customer_name');

        if ($customerName === null) {
            return response()->json([
                'message' => "Customer dengan kode {$customerCode} tidak ditemukan.",
            ], 404);
        }

        $noHu = ltrim(substr($segments[2], -15), '0');

        if ($noHu === '') {
            return response()->json([
                'message' => 'No HU pada barcode tidak valid.',
            ], 422);
        }

        if (PackingProcess::query()->where('no_hu', $noHu)->exists()) {
            return response()->json([
                'message' => 'No HU sudah terdaftar pada data Packing.',
            ], 409);
        }

        return response()->json([
            'customer_code' => $customerCode,
            'customer' => $customerName,
            'model' => $segments[3],
            'no_hu' => $noHu,
        ]);
    }

    public function scanForOut(Request $request): JsonResponse
    {
        $validated = $request->validate([
            'location' => ['required', 'string', Rule::in(collect(range(1, 7))->map(fn (int $number): string => "WP-{$number}")->all())],
            'no_hu' => ['required', 'string', 'max:100'],
        ]);

        $matches = PackingProcess::query()
            ->where('location', $validated['location'])
            ->where('no_hu', $validated['no_hu'])
            ->whereNull('time_out')
            ->limit(2)
            ->get(['id', 'no_hu', 'model', 'qty', 'box_id']);

        if ($matches->isEmpty()) {
            return response()->json([
                'message' => 'No HU tidak ditemukan pada packing aktif di lokasi ini.',
            ], 404);
        }

        if ($matches->count() > 1) {
            return response()->json([
                'message' => 'No HU ini terdaftar lebih dari satu kali pada packing aktif. Periksa data terlebih dahulu.',
            ], 409);
        }

        return response()->json(['process' => $matches->first()]);
    }

    public function completeOut(Request $request): JsonResponse
    {
        $validated = $request->validate([
            'location' => ['required', 'string', Rule::in(collect(range(1, 7))->map(fn (int $number): string => "WP-{$number}")->all())],
            'process_ids' => ['required', 'array', 'min:1', 'max:50'],
            'process_ids.*' => ['required', 'integer', 'distinct', 'exists:packing_processes,id'],
        ]);

        $completedCount = DB::transaction(function () use ($validated): ?int {
            $processes = PackingProcess::query()
                ->whereIn('id', $validated['process_ids'])
                ->where('location', $validated['location'])
                ->whereNotNull('no_hu')
                ->where('no_hu', '<>', '')
                ->whereNull('time_out')
                ->lockForUpdate()
                ->get();

            if ($processes->count() !== count($validated['process_ids'])) {
                return null;
            }

            $timeOut = now();

            foreach ($processes as $process) {
                $process->update([
                    'process_status' => 'OUT',
                    'time_out' => $timeOut,
                ]);
            }

            return $processes->count();
        });

        if ($completedCount === null) {
            return response()->json([
                'message' => 'Sebagian packing sudah OUT, tidak memiliki No HU, atau tidak termasuk lokasi ini. Muat ulang halaman dan scan kembali.',
            ], 409);
        }

        return response()->json([
            'message' => "{$completedCount} box berhasil diselesaikan.",
            'completed' => $completedCount,
        ]);
    }

    public function index(): JsonResponse
    {
        $packingProcesses = PackingProcess::query()
            ->latest('id')
            ->paginate(15);

        return response()->json($packingProcesses);
    }

    public function store(Request $request): JsonResponse
    {
        $packingProcess = PackingProcess::query()->create(
            $request->validate($this->validationRules()),
        );

        return response()->json($packingProcess, 201);
    }

    public function show(PackingProcess $packingProcess): JsonResponse
    {
        return response()->json($packingProcess);
    }

    public function update(Request $request, PackingProcess $packingProcess): JsonResponse
    {
        $packingProcess->update($request->validate($this->validationRules(true)));

        return response()->json($packingProcess->refresh());
    }

    public function destroy(PackingProcess $packingProcess): Response
    {
        $packingProcess->delete();

        return response()->noContent();
    }

    /**
     * @return array<string, array<int, string>>
     */
    private function validationRules(bool $isUpdate = false): array
    {
        $required = $isUpdate ? 'sometimes' : 'required';

        return [
            'model' => [$required, 'string', 'max:100'],
            'no_hu' => ['nullable', 'string', 'max:100'],
            'customer' => [$required, 'string', 'max:100'],
            'location' => [$required, 'string', 'max:100'],
            'box_id' => [$required, 'string', 'max:100'],
            'qty' => [$required, 'integer', 'min:1'],
            'remarks' => $isUpdate
                ? ['sometimes', 'nullable', 'string', 'max:5000']
                : ['nullable', 'string', 'max:5000'],
        ];
    }
}
