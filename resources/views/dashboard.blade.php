<x-sidebar title="Dashboard">
    <div style="margin-bottom: 1.5rem;">
        <h2 style="font-size: 1.2rem; font-weight: 700; margin: 0;">Machine Status</h2>
        <p style="margin: .45rem 0 0; color: var(--muted); font-size: .78rem;">
            Status terakhir untuk setiap line produksi.
        </p>
    </div>

    <table>
        <thead>
            <tr>
                <th>No.</th>
                <th>Line Produksi</th>
                <th>Status Mesin</th>
                <th>Start On</th>
            </tr>
        </thead>
        <tbody>
            @foreach ($machineStatuses as $index => $machineStatus)
                <tr>
                    <td>{{ $machineStatuses->firstItem() + $index }}</td>
                    <td>{{ $machineStatus['line'] }}</td>
                    <td>
                        @if ($machineStatus['status'] === 'Running')
                            <span
                                style="display: inline-flex; align-items: center; gap: .5rem; color: #18794e; font-weight: 700;">
                                <span aria-hidden="true"
                                    style="width: .55rem; height: .55rem; border-radius: 50%; background: #4ade80;"></span>
                                Running
                            </span>
                        @else
                            <span
                                style="display: inline-flex; align-items: center; gap: .5rem; color: #b42318; font-weight: 700;">
                                <span aria-hidden="true"
                                    style="width: .55rem; height: .55rem; border-radius: 50%; background: #f87171;"></span>
                                Tidak Running
                            </span>
                        @endif
                    </td>
                    <td>{{ $machineStatus['startOn'] }}</td>
                </tr>
            @endforeach
        </tbody>
    </table>

    @if ($machineStatuses->hasPages())
        <nav aria-label="Pagination Machine Status"
            style="display: flex; align-items: center; justify-content: center; gap: .75rem; margin-top: 1.5rem;">
            @if ($machineStatuses->onFirstPage())
                <span class="btn" style="opacity: .5; pointer-events: none;">&larr; Sebelumnya</span>
            @else
                <a href="{{ $machineStatuses->previousPageUrl() }}" class="btn">&larr; Sebelumnya</a>
            @endif

            <span style="font-size: .82rem;">Halaman {{ $machineStatuses->currentPage() }} dari
                {{ $machineStatuses->lastPage() }}</span>

            @if ($machineStatuses->hasMorePages())
                <a href="{{ $machineStatuses->nextPageUrl() }}" class="btn">Berikutnya &rarr;</a>
            @else
                <span class="btn" style="opacity: .5; pointer-events: none;">Berikutnya &rarr;</span>
            @endif
        </nav>
    @endif
</x-sidebar>
