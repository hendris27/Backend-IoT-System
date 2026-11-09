<?php

namespace App\Http\Controllers;

use App\Exports\PackingHistoryExcelExport;
use App\Models\PackingProcess;
use App\Models\User;
use Illuminate\Contracts\View\View;
use Illuminate\Database\Eloquent\Builder as EloquentBuilder;
use Illuminate\Database\Query\Builder;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Carbon;

class PackingHistoryController extends Controller
{
    public function index(Request $request): View
    {
        $filters = $this->resolveFilters($request);
        $users = $this->historyUsersQuery($filters)
            ->paginate(10)
            ->withQueryString();

        return view('packing.history', compact(
            'filters',
            'users',
        ));
    }

    public function export(Request $request, PackingHistoryExcelExport $export)
    {
        $filters = $this->resolveFilters($request);
        $users = $this->historyUsersQuery($filters)->cursor();
        $processes = $this->historyDetailsQuery($filters)->cursor();

        return $export->download($users, $processes);
    }

    public function show(Request $request, User $user): View
    {
        $filters = $this->resolveFilters($request);
        $dateFrom = $filters['date_from'];
        $dateTo = $filters['date_to'];

        $processes = PackingProcess::query()
            ->with(['inUser:id,name', 'outUser:id,name'])
            ->where(function ($query) use ($user, $dateFrom, $dateTo): void {
                $query->where(function ($inQuery) use ($user, $dateFrom, $dateTo): void {
                    $inQuery->where('in_user_id', $user->id)
                        ->whereNotNull('time_in');
                    $this->applyDateBounds($inQuery, 'time_in', $dateFrom, $dateTo);
                })->orWhere(function ($outQuery) use ($user, $dateFrom, $dateTo): void {
                    $outQuery->where('out_user_id', $user->id)
                        ->whereNotNull('time_out');
                    $this->applyDateBounds($outQuery, 'time_out', $dateFrom, $dateTo);
                });
            })
            ->latest('id')
            ->paginate(25)
            ->withQueryString();

        return view('packing.history-detail', [
            ...$filters,
            'processes' => $processes,
            'user' => $user,
        ]);
    }

    private function resolveFilters(Request $request): array
    {
        $validated = $request->validate([
            'date_from' => ['nullable', 'date'],
            'date_to' => ['nullable', 'date', 'after_or_equal:date_from'],
            'search' => ['nullable', 'string', 'max:100'],
        ]);

        $dateFrom = $validated['date_from'] ?? null;
        $dateTo = $validated['date_to'] ?? null;
        $search = trim($validated['search'] ?? '');
        $dateFromLabel = $dateFrom ? Carbon::parse($dateFrom)->format('d M Y') : null;
        $dateToLabel = $dateTo ? Carbon::parse($dateTo)->format('d M Y') : null;
        $dateLabel = match (true) {
            $dateFromLabel !== null && $dateToLabel !== null => "{$dateFromLabel} – {$dateToLabel}",
            $dateFromLabel !== null => "Mulai {$dateFromLabel}",
            $dateToLabel !== null => "Sampai {$dateToLabel}",
            default => 'Tanpa batas tanggal',
        };

        return compact('dateFrom', 'dateTo', 'dateLabel', 'search') + [
            'date_from' => $dateFrom,
            'date_to' => $dateTo,
        ];
    }

    private function historyUsersQuery(array $filters): Builder
    {
        $dateFrom = $filters['date_from'];
        $dateTo = $filters['date_to'];
        $search = $filters['search'];

        $inCounts = DB::table('packing_processes')
            ->selectRaw('in_user_id AS user_id, COUNT(*) AS qty_in')
            ->whereNotNull('in_user_id')
            ->whereNotNull('time_in');
        $this->applyDateBounds($inCounts, 'time_in', $dateFrom, $dateTo);
        $inCounts->groupBy('in_user_id');

        $outCounts = DB::table('packing_processes')
            ->selectRaw('out_user_id AS user_id, COUNT(*) AS qty_out')
            ->whereNotNull('out_user_id')
            ->whereNotNull('time_out');
        $this->applyDateBounds($outCounts, 'time_out', $dateFrom, $dateTo);
        $outCounts->groupBy('out_user_id');

        return DB::table('users')
            ->leftJoinSub($inCounts, 'packing_in_counts', 'packing_in_counts.user_id', '=', 'users.id')
            ->leftJoinSub($outCounts, 'packing_out_counts', 'packing_out_counts.user_id', '=', 'users.id')
            ->where(function (Builder $query): void {
                $query->whereRaw('COALESCE(packing_in_counts.qty_in, 0) > 0')
                    ->orWhereRaw('COALESCE(packing_out_counts.qty_out, 0) > 0');
            })
            ->when($search !== '', function (Builder $query) use ($search): void {
                $query->where(function (Builder $query) use ($search): void {
                    $query->where('users.name', 'like', "%{$search}%")
                        ->orWhere('users.nik', 'like', "%{$search}%");
                });
            })
            ->orderBy('users.name')
            ->select([
                'users.id',
                'users.name',
                'users.nik',
                DB::raw('COALESCE(packing_in_counts.qty_in, 0) AS qty_in'),
                DB::raw('COALESCE(packing_out_counts.qty_out, 0) AS qty_out'),
            ]);
    }

    private function historyDetailsQuery(array $filters): Builder
    {
        $dateFrom = $filters['date_from'];
        $dateTo = $filters['date_to'];
        $search = $filters['search'];

        return DB::table('packing_processes as processes')
            ->leftJoin('users as in_users', 'in_users.id', '=', 'processes.in_user_id')
            ->leftJoin('users as out_users', 'out_users.id', '=', 'processes.out_user_id')
            ->where(function (Builder $query) use ($dateFrom, $dateTo, $search): void {
                $query->where(function (Builder $inQuery) use ($dateFrom, $dateTo, $search): void {
                    $inQuery->whereNotNull('processes.in_user_id')
                        ->whereNotNull('processes.time_in');
                    $this->applyDateBounds($inQuery, 'processes.time_in', $dateFrom, $dateTo);

                    if ($search !== '') {
                        $inQuery->where(function (Builder $searchQuery) use ($search): void {
                            $searchQuery->where('in_users.name', 'like', "%{$search}%")
                                ->orWhere('in_users.nik', 'like', "%{$search}%");
                        });
                    }
                })->orWhere(function (Builder $outQuery) use ($dateFrom, $dateTo, $search): void {
                    $outQuery->whereNotNull('processes.out_user_id')
                        ->whereNotNull('processes.time_out');
                    $this->applyDateBounds($outQuery, 'processes.time_out', $dateFrom, $dateTo);

                    if ($search !== '') {
                        $outQuery->where(function (Builder $searchQuery) use ($search): void {
                            $searchQuery->where('out_users.name', 'like', "%{$search}%")
                                ->orWhere('out_users.nik', 'like', "%{$search}%");
                        });
                    }
                });
            })
            ->orderByDesc('processes.id')
            ->select([
                'processes.customer',
                'processes.model',
                'processes.no_hu',
                'processes.location',
                'processes.box_id',
                'processes.qty',
                'processes.time_in',
                'in_users.name as in_pic',
                'processes.time_out',
                'out_users.name as out_pic',
            ]);
    }

    private function applyDateBounds(
        Builder|EloquentBuilder $query,
        string $column,
        ?string $dateFrom,
        ?string $dateTo,
    ): void
    {
        if ($dateFrom !== null) {
            $query->whereDate($column, '>=', $dateFrom);
        }

        if ($dateTo !== null) {
            $query->whereDate($column, '<=', $dateTo);
        }
    }
}
