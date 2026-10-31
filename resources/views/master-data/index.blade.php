<x-sidebar title="Master Data CLL">
    <style>
        table {
            table-layout: fixed;
        }

        .master-data-toolbar {
            display: flex;
            align-items: end;
            justify-content: space-between;
            gap: 1rem;
            margin-bottom: 1.5rem;
        }

        .master-data-toolbar-actions {
            display: flex;
            align-items: end;
            justify-content: end;
            gap: .75rem;
            flex-wrap: wrap;
        }

        .model-filter-form {
            display: flex;
            align-items: end;
            gap: .75rem;
            flex-wrap: wrap;
        }

        .model-filter-select {
            min-width: 220px;
            padding: .6rem .75rem;
            border: 1px solid var(--card-border);
            border-radius: 7px;
            background: #fff;
            color: #102a43;
            font: inherit;
        }

        .model-filter-select option {
            background: #fff;
            color: #102a43;
        }

        .master-data-table .number-column {
            width: 4.5rem;
            padding-right: .5rem;
            padding-left: .5rem;
            text-align: center;
        }

        .master-data-table .actions-column {
            width: 11rem;
        }

        @media (max-width: 700px) {

            .master-data-toolbar,
            .master-data-toolbar-actions {
                align-items: stretch;
                flex-direction: column;
            }

            .master-data-toolbar-actions .btn {
                text-align: center;
            }
        }
    </style>

    <div class="master-data-toolbar">
        <div class="master-data-toolbar-actions">
            <form method="GET" action="{{ route('master-data.index') }}" class="model-filter-form">
                <div>
                    <label for="model_name"
                        style="display: block; margin-bottom: .4rem; font-size: .8rem; font-weight: 700;">Filter
                        Model</label>
                    <select name="model_name" id="model_name" class="model-filter-select">
                        <option value="">All Model</option>
                        @foreach ($modelNames as $modelName)
                            <option value="{{ $modelName }}" @selected($selectedModel === $modelName)>{{ $modelName }}
                            </option>
                        @endforeach
                    </select>
                </div>
                <button type="submit" class="btn btn-primary">Filter</button>
                @if ($selectedModel)
                    <a href="{{ route('master-data.index') }}" class="btn">Reset</a>
                @endif
            </form>
            <a href="{{ route('master-data.create') }}" class="btn btn-primary">+ Add New</a>
        </div>
    </div>

    <table class="master-data-table">
        <colgroup>
            <col style="width: 4.5rem;">
            <col>
            <col>
            <col>
            <col style="width: 11rem;">
        </colgroup>
        <thead>
            <tr>
                <th class="number-column">No.</th>
                <th>Model Name</th>
                <th>Item Name</th>
                <th>Created At</th>
                <th class="actions-column">Actions</th>
            </tr>
        </thead>
        <tbody>
            @forelse ($masterDataList as $item)
                <tr>
                    <td class="number-column">{{ $masterDataList->firstItem() + $loop->index }}</td>
                    <td>{{ $item->model_name }}</td>
                    <td>{{ $item->item_name }}</td>
                    <td>{{ $item->created_at->format('d M Y') }}</td>
                    <td class="actions-column">
                        <a href="{{ route('master-data.edit', ['master_datum' => $item->id]) }}" class="btn"
                            style="padding: 0.4rem 0.8rem; font-size: 0.7rem;">Edit</a>
                        <form action="{{ route('master-data.destroy', ['master_datum' => $item->id]) }}" method="POST"
                            style="display: inline;" class="delete-master-data-form"
                            data-item-name="{{ $item->item_name }}">
                            @csrf
                            @method('DELETE')
                            <button type="submit" class="btn btn-danger"
                                style="padding: 0.4rem 0.8rem; font-size: 0.7rem;">Delete</button>
                        </form>
                    </td>
                </tr>
            @empty
                <tr>
                    <td colspan="5" style="text-align: center; padding: 2rem;">No data found</td>
                </tr>
            @endforelse
        </tbody>
    </table>

    @if ($masterDataList->hasPages())
        <nav aria-label="Pagination Master Data"
            style="display: flex; align-items: center; justify-content: center; gap: .75rem; margin-top: 1.5rem;">
            @if ($masterDataList->onFirstPage())
                <span class="btn" style="opacity: .5; pointer-events: none;">&larr; Sebelumnya</span>
            @else
                <a href="{{ $masterDataList->previousPageUrl() }}" class="btn">&larr; Sebelumnya</a>
            @endif

            <span style="font-size: .82rem;">Halaman {{ $masterDataList->currentPage() }} dari
                {{ $masterDataList->lastPage() }}</span>

            @if ($masterDataList->hasMorePages())
                <a href="{{ $masterDataList->nextPageUrl() }}" class="btn">Berikutnya &rarr;</a>
            @else
                <span class="btn" style="opacity: .5; pointer-events: none;">Berikutnya &rarr;</span>
            @endif
        </nav>
    @endif

    <script>
        document.querySelectorAll('.delete-master-data-form').forEach((deleteForm) => {
            deleteForm.addEventListener('submit', (event) => {
                event.preventDefault();

                Swal.fire({
                    icon: 'warning',
                    title: 'Hapus Master Data?',
                    text: `Item ${deleteForm.dataset.itemName} akan dihapus permanen dari database.`,
                    showCancelButton: true,
                    confirmButtonColor: '#d33',
                    cancelButtonColor: '#1677d2',
                    confirmButtonText: 'Ya, hapus',
                    cancelButtonText: 'Batal',
                }).then((result) => {
                    if (result.isConfirmed) {
                        deleteForm.submit();
                    }
                });
            });
        });
    </script>
</x-sidebar>
