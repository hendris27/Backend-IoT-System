@vite('resources/css/packing-history.css')

<x-sidebar title="Histori Packing">
    <section class="packing-history-page">
        @php
            $historyQuery = [];
            if ($filters['date_from']) {
                $historyQuery['date_from'] = $filters['date_from'];
            }
            if ($filters['date_to']) {
                $historyQuery['date_to'] = $filters['date_to'];
            }
            if ($filters['search'] !== '') {
                $historyQuery['search'] = $filters['search'];
            }
        @endphp
        <header class="packing-history-page-heading">

        </header>

        <section class="packing-history-filter-bar" aria-label="Pencarian dan filter tanggal">
            <form class="packing-history-search packing-history-filter-form" method="GET"
                action="{{ route('packing.history') }}">
                <div class="packing-history-filter-fields">
                    <label class="packing-history-search-field">

                        <input id="packing-history-search" type="search" name="search" value="{{ $filters['search'] }}"
                            placeholder="Search..." maxlength="100">
                    </label>
                    <label>

                        <input type="date" name="date_from" value="{{ $filters['date_from'] }}">
                    </label>
                    <label>

                        <input type="date" name="date_to" value="{{ $filters['date_to'] }}">
                    </label>
                </div>
                <div class="packing-history-filter-actions">
                    <button type="submit">Cari</button>
                    @if ($historyQuery !== [])
                        <a href="{{ route('packing.history') }}">Reset</a>
                    @endif
                </div>
                <button class="packing-history-export-button" type="submit"
                    formaction="{{ route('packing.history.export') }}" formmethod="GET">
                    Download Excel
                </button>
            </form>
        </section>

        <section class="packing-history-card">
            <div class="packing-history-table-heading">

            </div>
            <div class="packing-history-table-wrap">
                <table class="packing-history-table">
                    <thead>
                        <tr>

                            <th scope="col">No</th>
                            <th scope="col">Nama</th>
                            <th scope="col">NIK</th>
                            <th scope="col">Qty IN (Box)</th>
                            <th scope="col">Qty OUT (Box)</th>
                        </tr>
                    </thead>
                    <tbody>
                        @forelse ($users as $user)
                            <tr>
                                <td>{{ $users->firstItem() + $loop->index }}</td>
                                <td>
                                    <a class="packing-history-name-link"
                                        href="{{ route('packing.history.users.show', ['user' => $user->id] + $historyQuery) }}">
                                        {{ $user->name ?: 'Tanpa nama' }}
                                    </a>
                                </td>
                                <td>{{ $user->nik ?: '—' }}</td>
                                <td>{{ number_format($user->qty_in) }}</td>
                                <td>{{ number_format($user->qty_out) }}</td>
                            </tr>
                        @empty
                            <tr>
                                <td class="packing-history-empty" colspan="5">
                                    {{ $filters['search'] !== '' ? 'Data Pencarian Tidak Ditemukan.' : 'Belum ada histori pada filter ini.' }}
                                </td>
                            </tr>
                        @endforelse
                    </tbody>
                </table>
            </div>

            @if ($users->hasPages())
                <div class="packing-history-pagination">{{ $users->onEachSide(1)->links() }}</div>
            @endif
        </section>
    </section>
</x-sidebar>
