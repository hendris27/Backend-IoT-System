<?php

namespace App\Http\Controllers;

use App\Models\PackingLocation;
use App\Models\PackingProcess;
use Illuminate\Http\RedirectResponse;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;

class PackingLocationController extends Controller
{
    public function store(): RedirectResponse
    {
        $locationName = DB::transaction(function (): string {
            $knownNames = PackingLocation::query()
                ->lockForUpdate()
                ->pluck('name');
            $highestNumber = $knownNames
                ->map(fn (string $name): int => $this->numberFromName($name))
                ->filter(fn (int $number): bool => $number !== PHP_INT_MAX)
                ->max() ?? 0;
            $locationName = 'WP-'.($highestNumber + 1);

            PackingLocation::query()->create([
                'name' => $locationName,
                'is_active' => true,
            ]);

            return $locationName;
        });

        return to_route('packing.index', ['location' => $locationName])
            ->with('success', "Lokasi {$locationName} berhasil ditambahkan.");
    }

    public function destroy(Request $request, string $location): RedirectResponse
    {
        $result = DB::transaction(function () use ($location): array {
            $packingLocation = PackingLocation::query()
                ->where('name', $location)
                ->lockForUpdate()
                ->firstOrFail();

            if (! $packingLocation->is_active) {
                return ['status' => 'inactive'];
            }

            $activeProcessCount = PackingProcess::query()
                ->where('location', $packingLocation->name)
                ->whereNull('time_out')
                ->count();

            if ($activeProcessCount > 0) {
                return [
                    'status' => 'has_active_processes',
                    'count' => $activeProcessCount,
                    'name' => $packingLocation->name,
                ];
            }

            if (PackingLocation::query()->where('is_active', true)->count() <= 1) {
                return ['status' => 'last_location', 'name' => $packingLocation->name];
            }

            $packingLocation->update(['is_active' => false]);

            return ['status' => 'removed', 'name' => $packingLocation->name];
        });

        $requestedLocation = $request->input('selected_location');
        $redirectLocation = $this->availableRedirectLocation(
            is_string($requestedLocation) ? $requestedLocation : null,
        );

        if ($result['status'] !== 'removed') {
            $message = match ($result['status']) {
                'has_active_processes' => "WP {$result['name']} masih memiliki {$result['count']} packing aktif. Selesaikan packing itu sebelum menghapus lokasi.",
                'last_location' => 'Minimal satu lokasi WP harus tetap aktif.',
                default => "Lokasi {$location} sudah tidak aktif.",
            };

            return to_route('packing.index', ['location' => $redirectLocation])
                ->with('location_settings_error', $message);
        }

        return to_route('packing.index', ['location' => $redirectLocation])
            ->with('success', "{$result['name']} dinonaktifkan. Riwayat packing tetap tersimpan.");
    }

    public function activate(string $location): RedirectResponse
    {
        $packingLocation = DB::transaction(function () use ($location): PackingLocation {
            $packingLocation = PackingLocation::query()
                ->where('name', $location)
                ->lockForUpdate()
                ->firstOrFail();

            if (! $packingLocation->is_active) {
                $packingLocation->update(['is_active' => true]);
            }

            return $packingLocation;
        });

        return to_route('packing.index', ['location' => $packingLocation->name])
            ->with('success', "Lokasi {$packingLocation->name} aktif kembali.");
    }

    private function availableRedirectLocation(?string $requestedLocation): string
    {
        $activeLocations = PackingLocation::activeNames();

        if ($requestedLocation !== null && $activeLocations->contains($requestedLocation)) {
            return $requestedLocation;
        }

        return (string) ($activeLocations->first() ?? 'WP-1');
    }

    private function numberFromName(string $name): int
    {
        return preg_match('/^WP-(\d+)$/', $name, $matches) === 1
            ? (int) $matches[1]
            : PHP_INT_MAX;
    }
}
