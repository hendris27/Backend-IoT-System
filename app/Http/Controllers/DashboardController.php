<?php

namespace App\Http\Controllers;

use App\Models\MachineStatus;
use Illuminate\Pagination\LengthAwarePaginator;
use Illuminate\View\View;

class DashboardController extends Controller
{
    public function index(): View
    {
        $statusesByLine = MachineStatus::query()->get()->keyBy('line');
        $machineStatusRows = collect(range(1, 30))->map(function (int $lineNumber) use ($statusesByLine): array {
            $line = "Line {$lineNumber}";
            $machineStatus = $statusesByLine->get($line);

            return [
                'line' => $line,
                'status' => $machineStatus?->status ?? 'Locked',
                'startOn' => $machineStatus?->status === 'Running'
                    ? $machineStatus->running_since?->format('H:i') ?? '-'
                    : '-',
            ];
        });

        $currentPage = LengthAwarePaginator::resolveCurrentPage();
        $machineStatuses = new LengthAwarePaginator(
            $machineStatusRows->forPage($currentPage, 10)->values(),
            $machineStatusRows->count(),
            10,
            $currentPage,
            [
                'path' => request()->url(),
                'query' => request()->query(),
            ],
        );

        return view('dashboard', compact('machineStatuses'));
    }
}
