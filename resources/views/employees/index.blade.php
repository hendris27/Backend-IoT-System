<x-sidebar title="All User">
    <section class="surface-card employee-card" aria-labelledby="employees-title">
        <div class="employee-toolbar">
            <div>
                <h2 id="employees-title">Daftar Karyawan</h2>
                <p>Nama, NIK, dan departemen karyawan aktif.</p>
            </div>
            <form class="employee-search" method="GET" action="{{ route('employees.index') }}">
                <label class="visually-hidden" for="employee-search">Cari karyawan</label>
                <input id="employee-search" type="search" name="search" value="{{ $search }}"
                    placeholder="Cari nama, NIK, atau departemen">
                <button class="btn btn-primary" type="submit">Cari</button>
                @if ($search !== '')
                    <a class="btn" href="{{ route('employees.index') }}">Reset</a>
                @endif
            </form>
        </div>

        <div class="employee-table-wrap">
            <table class="employee-table">
                <thead>
                    <tr>
                        <th scope="col">Nama</th>
                        <th scope="col">NIK</th>
                        <th scope="col">Departemen</th>
                    </tr>
                </thead>
                <tbody>
                    @forelse ($employees as $employee)
                        <tr>
                            <td>{{ $employee->name ?: '—' }}</td>
                            <td>{{ $employee->nik ?: '—' }}</td>
                            <td>{{ $employee->department ?: '—' }}</td>
                        </tr>
                    @empty
                        <tr>
                                <td class="employee-empty" colspan="3">
                                {{ $search !== '' ? 'Data karyawan tidak ditemukan.' : 'Data karyawan belum tersedia.' }}
                            </td>
                        </tr>
                    @endforelse
                </tbody>
            </table>
        </div>

        @if ($employees->hasPages())
            @php
                $currentPage = $employees->currentPage();
                $lastPage = $employees->lastPage();
                $startPage = max(1, $currentPage - 2);
                $endPage = min($lastPage, $currentPage + 2);
            @endphp

            <nav class="employee-pagination" aria-label="Pagination All User">
                <p class="employee-pagination-summary">
                    Menampilkan <strong>{{ $employees->firstItem() }}</strong>–<strong>{{ $employees->lastItem() }}</strong>
                    dari <strong>{{ $employees->total() }}</strong> karyawan
                </p>

                <div class="employee-pagination-controls">
                    @if ($employees->onFirstPage())
                        <span class="employee-page-link is-disabled" aria-disabled="true">&laquo; Sebelumnya</span>
                    @else
                        <a class="employee-page-link" href="{{ $employees->previousPageUrl() }}" rel="prev">
                            &laquo; Sebelumnya
                        </a>
                    @endif

                    @if ($startPage > 1)
                        <a class="employee-page-link" href="{{ $employees->url(1) }}">1</a>
                        @if ($startPage > 2)
                            <span class="employee-page-ellipsis" aria-hidden="true">&hellip;</span>
                        @endif
                    @endif

                    @for ($page = $startPage; $page <= $endPage; $page++)
                        @if ($page === $currentPage)
                            <span class="employee-page-link is-active" aria-current="page">{{ $page }}</span>
                        @else
                            <a class="employee-page-link" href="{{ $employees->url($page) }}">{{ $page }}</a>
                        @endif
                    @endfor

                    @if ($endPage < $lastPage)
                        @if ($endPage < $lastPage - 1)
                            <span class="employee-page-ellipsis" aria-hidden="true">&hellip;</span>
                        @endif
                        <a class="employee-page-link" href="{{ $employees->url($lastPage) }}">{{ $lastPage }}</a>
                    @endif

                    @if ($employees->hasMorePages())
                        <a class="employee-page-link" href="{{ $employees->nextPageUrl() }}" rel="next">
                            Berikutnya &raquo;
                        </a>
                    @else
                        <span class="employee-page-link is-disabled" aria-disabled="true">Berikutnya &raquo;</span>
                    @endif
                </div>
            </nav>
        @endif
    </section>
</x-sidebar>
