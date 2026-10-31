@if ($data->hasPages())
    @php
        $current = $data->currentPage();
        $last = $data->lastPage();
        $start = max(1, $current - 2);
        $end = min($last, $current + 2);
    @endphp

    <nav class="modern-pagination mt-4" id="pcsPagination-{{ $data->getPageName() }}"
        data-packing-async-navigation
        aria-label="{{ $data->getPageName() === 'active_page' ? 'Pagination packing aktif' : 'Pagination history packing' }}">
        <div class="modern-page-summary">
            Showing <strong>{{ $data->firstItem() }}</strong> –
            <strong>{{ $data->lastItem() }}</strong>
            of <strong>{{ $data->total() }}</strong> Data
        </div>

        <div class="modern-pager">

            {{-- Prev --}}
            @php($prev = $data->previousPageUrl())
            <a href="{{ $prev ?? '#' }}" class="modern-page-btn {{ $prev ? '' : 'disabled' }}"
                @if (! $prev) aria-disabled="true" tabindex="-1" @endif>
                &laquo;
            </a>

            {{-- First --}}
            @if ($start > 1)
                <a href="{{ $data->url(1) }}" class="modern-page-btn">1</a>
                @if ($start > 2)
                    <span class="modern-ellipsis">…</span>
                @endif
            @endif

            {{-- Middle --}}
            @for ($i = $start; $i <= $end; $i++)
                <a href="{{ $data->url($i) }}" class="modern-page-btn {{ $i == $current ? 'active' : '' }}"
                    @if ($i == $current) aria-current="page" @endif>
                    {{ $i }}
                </a>
            @endfor

            {{-- Last --}}
            @if ($end < $last)
                @if ($end < $last - 1)
                    <span class="modern-ellipsis">…</span>
                @endif
                <a href="{{ $data->url($last) }}" class="modern-page-btn">
                    {{ $last }}
                </a>
            @endif

            {{-- Next --}}
            @php($next = $data->nextPageUrl())
            <a href="{{ $next ?? '#' }}" class="modern-page-btn {{ $next ? '' : 'disabled' }}"
                @if (! $next) aria-disabled="true" tabindex="-1" @endif>
                &raquo;
            </a>

        </div>
    </nav>
@endif
