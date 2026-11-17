<?php

namespace App\Http\Controllers;

use App\Models\PackingLocation;
use App\Models\PackingProcess;
use Illuminate\Contracts\View\View;
use Illuminate\Database\Eloquent\Builder;
use Illuminate\Http\JsonResponse;
use Illuminate\Http\RedirectResponse;
use Illuminate\Http\Request;
use Illuminate\Http\Response;
use Illuminate\Support\Collection;
use Illuminate\Support\Facades\DB;
use Illuminate\Validation\Rule;

class PackingProcessController extends Controller
{
    public function dashboard(Request $request): View
    {
        $locations = PackingLocation::activeNames();

        $validated = $request->validate([
            'location' => ['nullable', 'string', Rule::in($locations->all())],
            'search' => ['nullable', 'string', 'max:100'],
            'search_submitted' => ['sometimes', 'boolean'],
        ]);

        $selectedLocation = $validated['location'] ?? $locations->first() ?? 'WP-1';
        $search = $validated['search'] ?? null;
        $loggedInUserId = $request->user()->getKey();

        $applySearch = function (Builder $query) use ($search): Builder {
            return $query->when($search, function (Builder $query) use ($search): void {
                $query->where(function (Builder $query) use ($search): void {
                    $query->where('model', 'like', "%{$search}%")
                        ->orWhere('no_hu', 'like', "%{$search}%")
                        ->orWhere('customer', 'like', "%{$search}%")
                        ->orWhere('box_id', 'like', "%{$search}%");
                });
            });
        };

        $filteredProcesses = fn (): Builder => $applySearch(
            PackingProcess::query()->where('location', $selectedLocation),
        );
        $matchingLocations = $search
            ? $applySearch(PackingProcess::query())
                ->whereIn('location', $locations->all())
                ->distinct()
                ->pluck('location')
                ->all()
            : [];
        $searchNotFound = (bool) ($validated['search_submitted'] ?? false)
            && filled($search)
            && $matchingLocations === [];

        $activeProcesses = $filteredProcesses()
            ->whereNull('time_out')
            ->latest('id')
            ->paginate(5, ['*'], 'active_page')
            ->withQueryString();

        $historyProcesses = $applySearch(
            PackingProcess::query()->where(function (Builder $query) use ($selectedLocation): void {
                $query->where('location', $selectedLocation)
                    ->orWhere('location', 'ONLINE');
            }),
        )
            ->with(['inUser:id,name', 'outUser:id,name'])
            ->where(function (Builder $query) use ($loggedInUserId): void {
                $query->where('in_user_id', $loggedInUserId)
                    ->orWhere('out_user_id', $loggedInUserId);
            })
            ->latest('id')
            ->paginate(10, ['*'], 'history_page')
            ->withQueryString();

        $waitingSummary = PackingProcess::query()
            ->whereNull('time_out')
            ->selectRaw('COALESCE(SUM(qty), 0) as qty, COUNT(*) as boxes')
            ->first();

        $outSummary = PackingProcess::query()
            ->whereNotNull('time_out')
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

        $activeLocationCounts = PackingProcess::query()
            ->whereNull('time_out')
            ->selectRaw('location, COUNT(*) as active_count')
            ->groupBy('location')
            ->pluck('active_count', 'location');
        $inactiveLocations = PackingLocation::inactiveNames();
        $nextLocationName = PackingLocation::nextName();

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
            'matchingLocations',
            'activeLocationCounts',
            'inactiveLocations',
            'nextLocationName',
            'nextBoxNumber',
            'outSummary',
            'search',
            'searchNotFound',
            'selectedLocation',
            'waitingSummary',
        ));
    }

    public function waiting(): View
    {
        $waitingProcesses = PackingProcess::query()
            ->with('inUser:id,name')
            ->whereNotNull('time_in')
            ->whereNull('time_out')
            ->latest('id')
            ->paginate(10)
            ->withQueryString();

        return view('packing.waiting', compact('waitingProcesses'));
    }

    public function updateRemarks(Request $request, PackingProcess $packingProcess): RedirectResponse
    {
        $validated = $request->validate([
            'remarks' => ['nullable', 'string', 'max:5000'],
        ]);

        $wasUpdated = DB::transaction(function () use ($packingProcess, $validated): bool {
            $process = PackingProcess::query()
                ->whereKey($packingProcess->getKey())
                ->lockForUpdate()
                ->firstOrFail();

            if ($process->time_in === null || $process->time_out !== null) {
                return false;
            }

            $process->update([
                'remarks' => $validated['remarks'] ?? null,
            ]);

            return true;
        });

        if (! $wasUpdated) {
            return to_route('packing.waiting')
                ->withErrors(['remarks' => 'Remarks hanya bisa diubah saat packing masih menunggu OUT.']);
        }

        return to_route('packing.waiting')
            ->with('success', 'Remarks berhasil diperbarui.')
            ->with('packing_success_auto_close', true);
    }

    public function storeFromDashboard(Request $request): RedirectResponse
    {
        $validated = $request->validate([
            'location' => ['required', 'string', Rule::in(PackingLocation::activeNames()->all())],
            'customer' => ['required', 'string', 'max:100'],
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

        $userId = $request->user()->getKey();

        DB::transaction(function () use ($validated, $userId): void {
            PackingLocation::query()
                ->where('name', $validated['location'])
                ->where('is_active', true)
                ->lockForUpdate()
                ->firstOrFail();

            $timeIn = now();

            foreach ($validated['boxes'] as $box) {
                PackingProcess::query()->create([
                    ...$box,
                    'customer' => $box['customer'] ?? $validated['customer'],
                    'location' => $validated['location'],
                    'process_status' => 'IN',
                    'time_in' => $timeIn,
                    'in_user_id' => $userId,
                ]);
            }
        });

        return to_route('packing.index', ['location' => $validated['location']])
            ->with('success', count($validated['boxes']).' box Packing IN berhasil disimpan.')
            ->with('packing_success_auto_close', true);
    }

    public function markOut(PackingProcess $packingProcess): RedirectResponse
    {
        $userId = auth()->id();

        $result = DB::transaction(function () use ($packingProcess, $userId): array {
            $process = PackingProcess::query()->findOrFail($packingProcess->getKey());

            if ($process->time_out !== null) {
                return ['status' => 'already_out'];
            }

            $modelKey = $this->normalizeModel($process->model);
            $activeQueue = $this->activeModelQueue($process->model, lockForUpdate: true);
            $lockedProcess = $activeQueue->firstWhere('id', $process->getKey());

            if (! $lockedProcess) {
                return ['status' => 'already_out'];
            }

            $violation = $this->findFifoViolation(
                collect([$lockedProcess]),
                collect([$modelKey => $activeQueue]),
            );

            if ($violation) {
                return ['status' => 'fifo_violation', 'process' => $violation];
            }

            $lockedProcess->update([
                'process_status' => 'OUT',
                'time_out' => now(),
                'out_user_id' => $userId,
            ]);

            return ['status' => 'completed'];
        });

        if ($result['status'] === 'fifo_violation') {
            return to_route('packing.index', ['location' => $packingProcess->location])
                ->withErrors(['packing_process' => e($this->fifoViolationMessage($result['process']))]);
        }

        if ($result['status'] === 'already_out') {
            return to_route('packing.index', ['location' => $packingProcess->location])
                ->withErrors(['packing_process' => 'Packing ini sudah berstatus OUT.']);
        }

        return to_route('packing.index', ['location' => $packingProcess->location])
            ->with('success', 'Packing OUT berhasil disimpan.')
            ->with('packing_success_auto_close', true);
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

        $barcode = trim($validated['barcode']);
        $segments = explode('@', $barcode);
        $noHu = $this->extractNoHuFromBarcode($barcode);
        $qtySegment = $segments[count($segments) - 3] ?? null;
        $qty = is_string($qtySegment) && ctype_digit($qtySegment) && (int) $qtySegment > 0
            ? (int) $qtySegment
            : null;

        if ($noHu === null) {
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
            'qty' => $qty,
        ]);
    }

    public function scanOnline(Request $request): JsonResponse
    {
        $validated = $request->validate([
            'barcode' => ['required', 'string', 'max:255'],
        ]);

        $item = $this->onlineBarcodeData(trim($validated['barcode']));

        if (PackingProcess::query()->where('no_hu', $item['no_hu'])->exists()) {
            return response()->json([
                'message' => "No HU {$item['no_hu']} sudah pernah terdaftar pada data Packing.",
            ], 409);
        }

        return response()->json($item);
    }

    public function completeOnline(Request $request): JsonResponse
    {
        $validated = $request->validate([
            'barcodes' => ['required', 'array', 'min:1', 'max:50'],
            'barcodes.*' => ['required', 'string', 'max:255'],
        ]);

        $items = collect($validated['barcodes'])
            ->map(fn (string $barcode): array => $this->onlineBarcodeData(trim($barcode)));
        $noHuList = $items->pluck('no_hu');

        if ($noHuList->unique()->count() !== $noHuList->count()) {
            return response()->json([
                'message' => 'Daftar scan berisi No HU yang sama lebih dari satu kali.',
            ], 409);
        }

        $userId = $request->user()->getKey();
        $result = DB::transaction(function () use ($items, $noHuList, $userId): array {
            $existingNoHu = PackingProcess::query()
                ->whereIn('no_hu', $noHuList->all())
                ->lockForUpdate()
                ->value('no_hu');

            if ($existingNoHu !== null) {
                return ['status' => 'duplicate', 'no_hu' => $existingNoHu];
            }

            $modelsByKey = $items
                ->mapWithKeys(fn (array $item): array => [
                    $this->normalizeModel($item['model']) => $item['model'],
                ])
                ->sortKeys();

            foreach ($modelsByKey as $model) {
                $activeQueue = $this->activeModelQueue($model, lockForUpdate: true);

                if ($blockingProcess = $activeQueue->first()) {
                    return ['status' => 'fifo_violation', 'process' => $blockingProcess];
                }
            }

            $timestamp = now();

            foreach ($items as $item) {
                PackingProcess::query()->create([
                    'customer' => $item['customer'],
                    'model' => $item['model'],
                    'no_hu' => $item['no_hu'],
                    'location' => 'ONLINE',
                    'box_id' => "ONLINE-{$item['no_hu']}",
                    'qty' => $item['qty'],
                    'process_status' => 'OUT',
                    'time_in' => $timestamp,
                    'in_user_id' => $userId,
                    'time_out' => $timestamp,
                    'out_user_id' => $userId,
                ]);
            }

            return ['status' => 'created', 'count' => $items->count()];
        });

        if ($result['status'] === 'fifo_violation') {
            return $this->fifoViolationResponse($result['process']);
        }

        if ($result['status'] === 'duplicate') {
            return response()->json([
                'message' => "No HU {$result['no_hu']} sudah terdaftar. Muat ulang daftar scan sebelum Complete.",
            ], 409);
        }

        return response()->json([
            'message' => "{$result['count']} box ONLINE berhasil disimpan dengan data IN dan OUT.",
            'completed' => $result['count'],
        ], 201);
    }

    public function scanForOut(Request $request): JsonResponse
    {
        $validated = $request->validate([
            'location' => ['required', 'string', Rule::in(PackingLocation::activeNames()->all())],
            'no_hu' => ['required', 'string', 'max:255'],
            'process_ids' => ['sometimes', 'array', 'max:50'],
            'process_ids.*' => ['required', 'integer', 'distinct', 'exists:packing_processes,id'],
        ]);

        $scannedValue = trim($validated['no_hu']);
        $noHu = str_starts_with($scannedValue, '@')
            ? $this->extractNoHuFromBarcode($scannedValue)
            : $scannedValue;

        if ($noHu === null) {
            return response()->json([
                'message' => 'Format barcode tidak valid.',
            ], 422);
        }

        if ($noHu === '') {
            return response()->json([
                'message' => 'No HU pada barcode tidak valid.',
            ], 422);
        }

        $matches = PackingProcess::query()
            ->where('location', $validated['location'])
            ->where('no_hu', $noHu)
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

        $process = $matches->first();
        $pendingIds = array_map('intval', $validated['process_ids'] ?? []);

        if (in_array((int) $process->id, $pendingIds, true)) {
            return response()->json([
                'message' => 'No HU ini sudah ada di daftar scan.',
            ], 409);
        }

        $pendingProcesses = collect();

        if ($pendingIds !== []) {
            $pendingProcesses = PackingProcess::query()
                ->whereIn('id', $pendingIds)
                ->where('location', $validated['location'])
                ->whereNull('time_out')
                ->whereNotNull('no_hu')
                ->where('no_hu', '<>', '')
                ->get(['id', 'no_hu', 'model', 'qty', 'box_id', 'location', 'time_in']);

            if ($pendingProcesses->count() !== count($pendingIds)) {
                return response()->json([
                    'message' => 'Sebagian No HU yang sudah dipindai berubah atau tidak aktif. Muat ulang halaman dan scan kembali.',
                ], 409);
            }
        }

        $selectedProcesses = $pendingProcesses->push($process);
        $fifoViolation = $this->findFifoViolation($selectedProcesses);

        if ($fifoViolation) {
            return $this->fifoViolationResponse($fifoViolation);
        }

        return response()->json(['process' => $process]);
    }

    public function completeOut(Request $request): JsonResponse
    {
        $validated = $request->validate([
            'location' => ['required', 'string', Rule::in(PackingLocation::activeNames()->all())],
            'process_ids' => ['required', 'array', 'min:1', 'max:50'],
            'process_ids.*' => ['required', 'integer', 'distinct', 'exists:packing_processes,id'],
        ]);

        $userId = $request->user()->getKey();

        $result = DB::transaction(function () use ($validated, $userId): array {
            $processIds = array_map('intval', $validated['process_ids']);
            $requestedProcesses = PackingProcess::query()
                ->whereIn('id', $processIds)
                ->get(['id', 'model']);

            if ($requestedProcesses->count() !== count($processIds)) {
                return ['status' => 'invalid'];
            }

            $modelsByKey = $requestedProcesses
                ->mapWithKeys(fn (PackingProcess $process): array => [
                    $this->normalizeModel($process->model) => $process->model,
                ])
                ->sortKeys();
            $activeQueues = collect();

            foreach ($modelsByKey as $modelKey => $model) {
                $activeQueues->put($modelKey, $this->activeModelQueue($model, lockForUpdate: true));
            }

            $processes = PackingProcess::query()
                ->whereIn('id', $validated['process_ids'])
                ->lockForUpdate()
                ->get(['id', 'location', 'no_hu', 'model', 'process_status', 'time_in', 'time_out']);

            if ($processes->count() !== count($validated['process_ids'])) {
                return ['status' => 'invalid'];
            }

            $eligibleProcesses = $processes->filter(fn (PackingProcess $process): bool => (
                $process->location === $validated['location']
                && $process->time_out === null
                && trim((string) $process->no_hu) !== ''
            ));

            if ($eligibleProcesses->count() !== count($processIds)) {
                return ['status' => 'invalid'];
            }

            foreach ($eligibleProcesses as $process) {
                $modelKey = $this->normalizeModel($process->model);
                $activeQueue = $activeQueues->get($modelKey);

                if (! $activeQueue || ! $activeQueue->contains(
                    fn (PackingProcess $queuedProcess): bool => (int) $queuedProcess->id === (int) $process->id,
                )) {
                    return ['status' => 'invalid'];
                }
            }

            $fifoViolation = $this->findFifoViolation($eligibleProcesses, $activeQueues);

            if ($fifoViolation) {
                return ['status' => 'fifo_violation', 'process' => $fifoViolation];
            }

            $timeOut = now();

            foreach ($eligibleProcesses as $process) {
                $process->update([
                    'process_status' => 'OUT',
                    'time_out' => $timeOut,
                    'out_user_id' => $userId,
                ]);
            }

            return ['status' => 'completed', 'count' => $eligibleProcesses->count()];
        });

        if ($result['status'] === 'fifo_violation') {
            return $this->fifoViolationResponse($result['process']);
        }

        if ($result['status'] === 'invalid') {
            return response()->json([
                'message' => 'Sebagian packing sudah OUT, tidak memiliki No HU, atau tidak termasuk lokasi ini. Muat ulang halaman dan scan kembali.',
            ], 409);
        }

        return response()->json([
            'message' => "{$result['count']} box berhasil diselesaikan.",
            'completed' => $result['count'],
        ]);
    }

    private function activeModelQueue(string $model, bool $lockForUpdate = false): Collection
    {
        $query = PackingProcess::query()
            ->whereRaw('LOWER(model) = ?', [mb_strtolower(trim($model))])
            ->whereNull('time_out')
            ->orderBy('time_in')
            ->orderBy('id');

        if ($lockForUpdate) {
            $query->lockForUpdate();
        }

        return $query->get(['id', 'location', 'model', 'no_hu', 'box_id', 'time_in', 'time_out']);
    }

    private function extractNoHuFromBarcode(string $barcode): ?string
    {
        $segments = explode('@', trim($barcode));

        if (
            ! str_starts_with($barcode, '@')
            || count($segments) < 5
            || preg_match('/^\d{4}/', $segments[1]) !== 1
            || strlen($segments[2]) < 15
            || $segments[3] === ''
        ) {
            return null;
        }

        return ltrim(substr($segments[2], -15), '0');
    }

    private function onlineBarcodeData(string $barcode): array
    {
        $segments = explode('@', $barcode);
        $noHu = $this->extractNoHuFromBarcode($barcode);
        $qtySegment = $segments[count($segments) - 3] ?? null;
        $qty = is_string($qtySegment) && ctype_digit($qtySegment) && (int) $qtySegment > 0
            ? (int) $qtySegment
            : null;

        if ($noHu === null || $noHu === '' || count($segments) < 7) {
            abort(422, 'Format barcode atau No HU tidak valid.');
        }

        if ($qty === null) {
            abort(422, 'QTY pada bagian akhir barcode tidak valid.');
        }

        $customerCode = substr($segments[1], 0, 4);
        $customerName = DB::table('customers')
            ->where('customer_code', $customerCode)
            ->value('customer_name');

        if ($customerName === null) {
            abort(404, "Customer dengan kode {$customerCode} tidak ditemukan.");
        }

        $model = trim($segments[3]);

        if ($model === '' || mb_strlen($model) > 100) {
            abort(422, 'Model pada barcode tidak valid.');
        }

        return [
            'customer' => $customerName,
            'model' => $model,
            'no_hu' => $noHu,
            'qty' => $qty,
            'barcode' => $barcode,
        ];
    }

    private function findFifoViolation(Collection $selectedProcesses, ?Collection $activeQueues = null): ?PackingProcess
    {
        $selectedByModel = $selectedProcesses
            ->groupBy(fn (PackingProcess $process): string => $this->normalizeModel($process->model))
            ->sortKeys();

        foreach ($selectedByModel as $modelKey => $selectedForModel) {
            $activeQueue = $activeQueues?->get($modelKey)
                ?? $this->activeModelQueue($selectedForModel->first()->model);
            $selectedIds = $selectedForModel->pluck('id')
                ->map(fn ($id): int => (int) $id)
                ->all();
            $expectedIds = $activeQueue->take($selectedForModel->count())
                ->pluck('id')
                ->map(fn ($id): int => (int) $id)
                ->all();
            $selectedIdSet = array_fill_keys($selectedIds, true);
            $expectedIdSet = array_fill_keys($expectedIds, true);

            if (
                array_diff_key($selectedIdSet, $expectedIdSet) !== []
                || array_diff_key($expectedIdSet, $selectedIdSet) !== []
            ) {
                return $activeQueue->first(
                    fn (PackingProcess $process): bool => ! isset($selectedIdSet[(int) $process->id]),
                ) ?? $activeQueue->first();
            }
        }

        return null;
    }

    private function fifoViolationResponse(PackingProcess $requiredProcess): JsonResponse
    {
        return response()->json([
            'code' => 'fifo_violation',
            'message' => $this->fifoViolationMessage($requiredProcess),
            'fifo' => [
                'model' => $requiredProcess->model,
                'no_hu' => $requiredProcess->no_hu,
                'location' => $requiredProcess->location,
                'box_id' => $requiredProcess->box_id,
                'time_in' => $requiredProcess->time_in?->toIso8601String(),
            ],
        ], 409);
    }

    private function fifoViolationMessage(PackingProcess $requiredProcess): string
    {
        $noHu = trim((string) $requiredProcess->no_hu);
        $requiredItem = $noHu !== ''
            ? "No HU {$noHu}"
            : 'record IN tertua (No HU belum diisi)';

        return "Urutan FIFO untuk model {$requiredProcess->model} dilanggar. Proses {$requiredItem} di lokasi {$requiredProcess->location} terlebih dahulu.";
    }

    private function normalizeModel(string $model): string
    {
        return mb_strtolower(trim($model));
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
            'location' => [$required, 'string', Rule::in(PackingLocation::activeNames()->all())],
            'box_id' => [$required, 'string', 'max:100'],
            'qty' => [$required, 'integer', 'min:1'],
            'remarks' => $isUpdate
                ? ['sometimes', 'nullable', 'string', 'max:5000']
                : ['nullable', 'string', 'max:5000'],
        ];
    }
}
