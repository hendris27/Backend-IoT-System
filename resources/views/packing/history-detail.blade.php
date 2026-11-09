@vite('resources/css/packing-history.css')

<x-sidebar title="Rincian Histori Packing">
    <section class="packing-history-page">
        @php
            $historyQuery = [];
            if ($date_from) {
                $historyQuery['date_from'] = $date_from;
            }
            if ($date_to) {
                $historyQuery['date_to'] = $date_to;
            }
            if ($search !== '') {
                $historyQuery['search'] = $search;
            }
        @endphp
        <header class="packing-history-page-heading packing-history-detail-heading">
            <div>
                <a class="packing-history-back-link" href="{{ route('packing.history', $historyQuery) }}">Kembali ke Histori</a>
                <p class="packing-history-eyebrow">Rincian histori</p>
                <h2>{{ $user->name ?: 'Tanpa nama' }}</h2>
                <p>NIK: {{ $user->nik ?: '—' }}</p>
            </div>
            <span class="packing-history-period-badge">{{ $dateLabel }}</span>
        </header>

        <section class="packing-history-card">
            <div class="packing-history-table-wrap">
                <table class="packing-history-table packing-history-detail-table">
                    <thead>
                        <tr>
                            <th scope="col">Customer</th>
                            <th scope="col">Model</th>
                            <th scope="col">No HU</th>
                            <th scope="col">Lokasi</th>
                            <th scope="col">Box</th>
                            <th scope="col">QTY</th>
                            <th scope="col">Time IN</th>
                            <th scope="col">PIC IN</th>
                            <th scope="col">Time OUT</th>
                            <th scope="col">PIC OUT</th>
                        </tr>
                    </thead>
                    <tbody>
                        @forelse ($processes as $process)
                            <tr>
                                <td>{{ $process->customer }}</td>
                                <td>{{ $process->model }}</td>
                                <td>{{ $process->no_hu ?: '—' }}</td>
                                <td>{{ $process->location }}</td>
                                <td>{{ $process->box_id }}</td>
                                <td>{{ number_format($process->qty) }}</td>
                                <td>{{ $process->time_in?->copy()->timezone('Asia/Bangkok')->format('d M Y, H:i') ?: '—' }}</td>
                                <td>{{ $process->inUser?->name ?: '—' }}</td>
                                <td>{{ $process->time_out?->copy()->timezone('Asia/Bangkok')->format('d M Y, H:i') ?: '—' }}</td>
                                <td>{{ $process->time_out === null ? '—' : ($process->outUser?->name ?: '—') }}</td>
                            </tr>
                        @empty
                            <tr>
                                <td class="packing-history-empty" colspan="10">Tidak ada rincian pada periode ini.</td>
                            </tr>
                        @endforelse
                    </tbody>
                </table>
            </div>

            @if ($processes->hasPages())
                <div class="packing-history-pagination">{{ $processes->onEachSide(1)->links() }}</div>
            @endif
        </section>
    </section>
</x-sidebar>
