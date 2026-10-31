@vite('resources/css/packing.css')

<x-sidebar title="Packing Dashboard">
    <div class="packing-dashboard" data-packing-dashboard aria-busy="false"
        data-location="{{ $selectedLocation }}" data-active-box-numbers='@json($activeBoxNumbers->all())'
        data-reopen-in="{{ $errors->any() && ! $errors->has('packing_process') ? 'true' : 'false' }}"
        data-reopen-edit-process-id="{{ old('edit_process_id') }}"
        data-old-edit-model="{{ old('model') }}" data-old-edit-no-hu="{{ old('no_hu') }}"
        data-old-edit-qty="{{ old('qty') }}">
        @if (session('success'))
            <p class="packing-feedback packing-feedback--success" role="status">{{ session('success') }}</p>
        @endif
        @if ($errors->any())
            <div class="packing-feedback packing-feedback--error" role="alert">
                <p>Data belum dapat diproses:</p>
                <ul>
                    @foreach ($errors->all() as $error)
                        <li>{{ $error }}</li>
                    @endforeach
                </ul>
            </div>
        @endif

        <section class="packing-summary" aria-label="Ringkasan Packing">
            <article class="packing-summary-card packing-summary-card--waiting">
                <span class="packing-summary-icon" aria-hidden="true">
                    <svg viewBox="0 0 24 24" fill="none">
                        <path d="M4 7.5 12 3l8 4.5v9L12 21l-8-4.5v-9Z" />
                        <path d="m4.5 7.8 7.5 4.3 7.5-4.3M12 12v8.5M8 5.3l8 4.6" />
                    </svg>
                </span>
                <div class="packing-summary-copy">
                    <h2>Waiting (QTY)</h2>
                    <p>{{ $waitingSummary->qty }}</p>
                </div>
            </article>
            <article class="packing-summary-card packing-summary-card--waiting">
                <span class="packing-summary-icon" aria-hidden="true">
                    <svg viewBox="0 0 24 24" fill="none">
                        <path d="M3.5 7.5 12 3l8.5 4.5v9L12 21l-8.5-4.5v-9Z" />
                        <path d="m3.8 7.8 8.2 4.4 8.2-4.4M12 12.2v8.3M8 5.1l8.2 4.5" />
                    </svg>
                </span>
                <div class="packing-summary-copy">
                    <h2>Waiting (BOX)</h2>
                    <p>{{ $waitingSummary->boxes }}</p>
                </div>
            </article>
            <article class="packing-summary-card packing-summary-card--out">
                <span class="packing-summary-icon" aria-hidden="true">
                    <svg viewBox="0 0 24 24" fill="none">
                        <path d="M12 16V4m0 0 4.5 4.5M12 4 7.5 8.5M5 19.5h14" />
                    </svg>
                </span>
                <div class="packing-summary-copy">
                    <h2>OUT (QTY)</h2>
                    <p>{{ $outSummary->qty }}</p>
                </div>
            </article>
            <article class="packing-summary-card packing-summary-card--out">
                <span class="packing-summary-icon" aria-hidden="true">
                    <svg viewBox="0 0 24 24" fill="none">
                        <path d="M12 16V4m0 0 4.5 4.5M12 4 7.5 8.5M5 19.5h14" />
                    </svg>
                </span>
                <div class="packing-summary-copy">
                    <h2>OUT (BOX)</h2>
                    <p>{{ $outSummary->boxes }}</p>
                </div>
            </article>
        </section>

        <section class="packing-workflow" aria-label="Proses Packing">
            <aside class="packing-location-panel">
                <form class="packing-search" method="GET" action="{{ route('packing.index') }}">
                    <input type="hidden" name="location" value="{{ $selectedLocation }}">
                    <input id="packing-search" type="search" name="search" value="{{ $search }}"
                        placeholder="Search..." autocomplete="off">
                    <button type="submit" aria-label="Cari">
                        <svg viewBox="0 0 20 20" fill="none" aria-hidden="true">
                            <circle cx="8.8" cy="8.8" r="5.8" />
                            <path d="m13.2 13.2 4 4" />
                        </svg>
                    </button>
                </form>

                <nav class="packing-location-list" aria-label="Pilih lokasi" data-packing-async-navigation>
                    @foreach ($locations as $location)
                        <a class="packing-location-button {{ $selectedLocation === $location ? 'is-selected' : '' }}"
                            href="{{ route('packing.index', array_filter(['location' => $location, 'search' => $search])) }}"
                            @if ($selectedLocation === $location) aria-current="true" @endif>
                            {{ $location }}
                        </a>
                    @endforeach
                </nav>
            </aside>

            <section class="packing-process-panel" aria-label="Packing {{ $selectedLocation }}">
                <div class="packing-process-toolbar">
                    <div>
                        <h2>{{ $selectedLocation }}</h2>
                    </div>
                    <div class="packing-process-actions">
                        <button class="packing-action-button packing-action-button--in" type="button"
                            data-open-in-modal>
                            <span aria-hidden="true">↓</span>
                            IN
                        </button>
                        <button class="packing-action-button packing-action-button--out" type="button"
                            data-open-out-modal @disabled($outAvailableCount === 0)
                            @if ($outAvailableCount === 0) title="Tidak ada data aktif dengan No HU untuk diproses OUT" @endif>
                            <span aria-hidden="true">↑</span>
                            OUT
                        </button>
                    </div>
                </div>

                <div class="packing-table-wrap">
                    <table class="packing-table">
                        <thead>
                            <tr>
                                <th scope="col">No.</th>
                                <th scope="col">Customer</th>
                                <th scope="col">Model</th>
                                <th scope="col">No HU</th>
                                <th scope="col">QTY</th>
                                <th scope="col">Action</th>
                            </tr>
                        </thead>
                        <tbody>
                            @forelse ($activeProcesses as $index => $process)
                                <tr>
                                    <td>{{ $activeProcesses->firstItem() + $index }}</td>
                                    <td>{{ $process->customer }}</td>
                                    <td>{{ $process->model }}</td>
                                    <td>{{ $process->no_hu ?: '—' }}</td>
                                    <td>{{ $process->qty }}</td>
                                    <td>
                                        <button class="packing-row-action" type="button"
                                            data-open-edit-modal
                                            data-process-id="{{ $process->id }}"
                                            data-process-model="{{ $process->model }}"
                                            data-process-no-hu="{{ $process->no_hu }}"
                                            data-process-qty="{{ $process->qty }}"
                                            aria-label="Edit {{ $process->box_id }}">
                                            EDIT
                                        </button>
                                    </td>
                                </tr>
                            @empty
                                <tr>
                                    <td class="packing-empty-state" colspan="6">
                                        Belum ada packing aktif di {{ $selectedLocation }}.
                                    </td>
                                </tr>
                            @endforelse
                        </tbody>
                    </table>
                </div>
                @if ($activeProcesses->hasPages())
                    <div class="packing-pagination">
                        @include('packing.pagination.pagination', ['data' => $activeProcesses])
                    </div>
                @endif
            </section>
        </section>

        <section class="packing-history-panel" aria-labelledby="packing-history-title">
            <div class="packing-history-heading">
                <div>
                    <p class="packing-eyebrow">Aktivitas lokasi {{ $selectedLocation }}</p>
                    <h2 id="packing-history-title">History Process</h2>
                </div>
                <span class="packing-history-count">{{ $historyProcesses->total() }} record</span>
            </div>
            <div class="packing-table-wrap">
                <table class="packing-table packing-history-table">
                    <thead>
                        <tr>
                            <th scope="col">No.</th>
                            <th scope="col">Customer</th>
                            <th scope="col">Model</th>
                            <th scope="col">No HU</th>
                            <th scope="col">QTY</th>
                            <th scope="col">Time IN</th>
                            <th scope="col">Time OUT</th>
                            <th scope="col">Remarks PIC</th>
                        </tr>
                    </thead>
                    <tbody>
                        @forelse ($historyProcesses as $index => $process)
                            <tr>
                                <td>{{ $historyProcesses->firstItem() + $index }}</td>
                                <td>{{ $process->customer }}</td>
                                <td>{{ $process->model }}</td>
                                <td>{{ $process->no_hu ?: '—' }}</td>
                                <td>{{ $process->qty }}</td>
                                <td>{{ $process->time_in?->format('d M Y, H:i') ?: '—' }}</td>
                                <td>{{ $process->time_out?->format('d M Y, H:i') ?: '—' }}</td>
                                <td>{{ $process->remarks ?: '—' }}</td>
                            </tr>
                        @empty
                            <tr>
                                <td class="packing-empty-state" colspan="8">
                                    Belum ada history process di {{ $selectedLocation }}.
                                </td>
                            </tr>
                        @endforelse
                    </tbody>
                </table>
            </div>
            @if ($historyProcesses->hasPages())
                <div class="packing-pagination">
                    @include('packing.pagination.pagination', ['data' => $historyProcesses])
                </div>
            @endif
        </section>

        <dialog class="packing-modal" data-in-modal aria-labelledby="packing-in-title">
            <form class="packing-modal-form packing-in-form" method="POST" action="{{ route('packing.processes.store') }}">
                @csrf
                <input type="hidden" name="location" value="{{ $selectedLocation }}">
                @php($initialBoxRows = old('boxes'))
                @if (! is_array($initialBoxRows) || $initialBoxRows === [])
                    @php($initialBoxRows = [['box_id' => sprintf('%s-%03d', $selectedLocation, $nextBoxNumber), 'customer' => '', 'model' => '', 'no_hu' => '', 'qty' => '']])
                @endif
                <div class="packing-modal-heading">
                    <div>
                        <p class="packing-eyebrow">{{ $selectedLocation }}</p>
                        <h2 id="packing-in-title">Packing IN</h2>
                    </div>
                    <button class="packing-modal-close" type="button" data-close-modal aria-label="Tutup">×</button>
                </div>
                <div class="packing-in-overview" aria-label="Ringkasan input box">
                    <div class="packing-in-overview-item">
                        <span>Lokasi</span>
                        <strong>{{ $selectedLocation }}</strong>
                    </div>
                    <div class="packing-in-overview-item">
                        <span>Total BOX</span>
                        <strong data-box-total>1</strong>
                    </div>
                    <div class="packing-in-overview-item">
                        <span>BOX saat ini</span>
                        <strong data-box-current>1</strong>
                    </div>
                </div>

                <div class="packing-out-scan">
                    <label class="sr-only" for="packing-in-barcode">Scan barcode Packing IN</label>
                    <input id="packing-in-barcode" type="text" inputmode="text" autocomplete="off"
                        placeholder="Scan barcode untuk menambah Customer, Model, dan No HU" data-in-barcode>
                    <button class="packing-out-scan-button" type="button" data-in-scan>
                        <span data-in-scan-label>SCAN</span>
                    </button>
                </div>
                <p class="packing-out-error" data-in-barcode-error role="alert" hidden></p>

                <div class="packing-modal-fields packing-in-shared-fields">
                    <label>
                        Customer
                        <input name="customer" type="text" maxlength="100" value="{{ old('customer') }}" required
                            data-in-customer>
                    </label>
                    <label>
                        Remarks PIC
                        <textarea name="remarks" rows="1" maxlength="5000">{{ old('remarks') }}</textarea>
                    </label>
                </div>

                <div class="packing-box-list-scroll">
                    <div class="packing-box-list" data-box-list>
                        @foreach ($initialBoxRows as $index => $box)
                            <div class="packing-in-box-row" data-box-row>
                                <label>
                                    Box ID
                                    <input name="boxes[{{ $index }}][box_id]" data-box-field="box_id" type="text" maxlength="100"
                                        value="{{ data_get($box, 'box_id', '') }}" readonly required>
                                </label>
                                <label>
                                    Customer
                                    <input name="boxes[{{ $index }}][customer]" data-box-field="customer" type="text"
                                        maxlength="100" value="{{ data_get($box, 'customer', '') }}" readonly>
                                </label>
                                <label>
                                    Model
                                    <input name="boxes[{{ $index }}][model]" data-box-field="model" type="text" maxlength="100"
                                        value="{{ data_get($box, 'model', '') }}" required>
                                </label>
                                <label>
                                    No HU
                                    <input name="boxes[{{ $index }}][no_hu]" data-box-field="no_hu" type="text" maxlength="100"
                                        value="{{ data_get($box, 'no_hu', '') }}">
                                </label>
                                <label class="packing-qty-field">
                                    <span class="packing-qty-heading">
                                        <span>QTY</span>
                                        @if ($index === 0)
                                            <span class="packing-qty-all">
                                                <input id="packing-copy-qty-{{ $index }}" type="checkbox" name="copy_qty_all"
                                                    value="1" data-copy-qty @checked(old('copy_qty_all'))>
                                                <label for="packing-copy-qty-{{ $index }}" data-copy-qty-label>ALL</label>
                                            </span>
                                        @endif
                                    </span>
                                    <input name="boxes[{{ $index }}][qty]" data-box-field="qty" type="number"
                                        min="1" step="1" value="{{ data_get($box, 'qty', '') }}" required>
                                </label>
                                <button class="packing-remove-box-button" type="button" data-remove-box
                                    aria-label="Hapus box {{ $index + 1 }}" @disabled($index === 0)>×</button>
                            </div>
                        @endforeach
                    </div>
                </div>

                <template data-box-row-template>
                    <div class="packing-in-box-row" data-box-row>
                        <label>
                            Box ID
                            <input data-box-field="box_id" type="text" maxlength="100" readonly required>
                        </label>
                        <label>
                            Customer
                            <input data-box-field="customer" type="text" maxlength="100" readonly>
                        </label>
                        <label>
                            Model
                            <input data-box-field="model" type="text" maxlength="100" required>
                        </label>
                        <label>
                            No HU
                            <input data-box-field="no_hu" type="text" maxlength="100">
                        </label>
                        <label class="packing-qty-field">
                            <span class="packing-qty-heading">QTY</span>
                            <input data-box-field="qty" type="number" min="1" step="1" required>
                        </label>
                        <button class="packing-remove-box-button" type="button" data-remove-box aria-label="Hapus box">×</button>
                    </div>
                </template>

                <div class="packing-modal-footer">
                    <button class="packing-modal-secondary" type="button" data-close-modal>Batal</button>
                    <button class="packing-action-button packing-action-button--in" type="submit">Simpan IN</button>
                </div>
            </form>
        </dialog>

        <dialog class="packing-modal packing-edit-modal" data-edit-modal aria-labelledby="packing-edit-title">
            <form class="packing-modal-form" method="POST" data-edit-form
                data-update-url="{{ route('packing.processes.update', ['packingProcess' => '__ID__']) }}">
                @csrf
                @method('PATCH')
                <input type="hidden" name="edit_process_id" data-edit-process-id>
                <div class="packing-modal-heading">
                    <div>
                        <p class="packing-eyebrow">{{ $selectedLocation }}</p>
                        <h2 id="packing-edit-title">Edit Packing</h2>
                    </div>
                    <button class="packing-modal-close" type="button" data-close-modal aria-label="Tutup">×</button>
                </div>

                <div class="packing-modal-fields">
                    <label>
                        Model
                        <input name="model" type="text" maxlength="100" required data-edit-model>
                    </label>
                    <label>
                        No HU
                        <input name="no_hu" type="text" maxlength="100" data-edit-no-hu>
                    </label>
                    <label>
                        QTY
                        <input name="qty" type="number" min="1" step="1" required data-edit-qty>
                    </label>
                </div>

                <div class="packing-modal-footer">
                    <button class="packing-modal-secondary" type="button" data-close-modal>Batal</button>
                    <button class="packing-action-button packing-action-button--in" type="submit">Simpan Perubahan</button>
                </div>
            </form>
        </dialog>

        <dialog class="packing-modal" data-out-modal aria-labelledby="packing-out-title">
            <form class="packing-modal-form packing-out-form" data-out-form
                data-scan-url="{{ route('packing.processes.scan') }}"
                data-complete-url="{{ route('packing.processes.complete-out') }}">
                @csrf
                <input type="hidden" data-out-location value="{{ $selectedLocation }}">
                <div class="packing-modal-heading">
                    <div>
                        <p class="packing-eyebrow">Lokasi {{ $selectedLocation }}</p>
                        <h2 id="packing-out-title">Packing OUT</h2>
                    </div>
                    <button class="packing-modal-close" type="button" data-close-modal aria-label="Tutup">×</button>
                </div>

                <div class="packing-out-overview">
                    <div class="packing-out-overview-item">
                        <span>Location</span>
                        <strong>{{ $selectedLocation }}</strong>
                    </div>
                    <div class="packing-out-overview-item">
                        <span>Total BOX</span>
                        <strong>{{ $activeBoxCount }}</strong>
                    </div>
                    <div class="packing-out-overview-item">
                        <span>Current BOX</span>
                        <strong data-out-current>0</strong>
                    </div>
                </div>

                <div class="packing-out-scan">
                    <label class="sr-only" for="packing-out-no-hu">Scan barcode atau ketik No HU</label>
                    <input id="packing-out-no-hu" type="text" inputmode="text" autocomplete="off"
                        placeholder="Scan barcode atau ketik No HU" data-out-no-hu>
                    <button class="packing-out-scan-button" type="button" data-out-scan>
                        <span aria-hidden="true">▣</span>
                        <span data-out-scan-label aria-live="polite">SCAN</span>
                    </button>
                </div>
                <p class="packing-out-help">Gunakan scanner atau ketik No HU lalu tekan Enter.</p>
                <p class="packing-out-error" data-out-error role="alert" hidden></p>

                <div class="packing-out-list-heading">LIST PART / CODE YANG DI SCAN</div>
                <div class="packing-table-wrap packing-out-table-wrap">
                    <table class="packing-table packing-out-table">
                        <thead>
                            <tr>
                                <th scope="col">No.</th>
                                <th scope="col">No HU</th>
                                <th scope="col">Model</th>
                                <th scope="col">QTY</th>
                                <th scope="col">Box</th>
                                <th scope="col">Aksi</th>
                            </tr>
                        </thead>
                        <tbody data-out-list>
                            <tr>
                                <td class="packing-empty-state" colspan="6">Belum ada No HU dipindai.</td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <div class="packing-out-total">
                    <span>TOTAL SCAN</span>
                    <strong data-out-total>0 ITEM</strong>
                </div>

                <div class="packing-modal-footer">
                    <button class="packing-modal-secondary" type="button" data-close-modal>Batal</button>
                    <button class="packing-action-button packing-action-button--out" type="button"
                        data-complete-out disabled>COMPLETE</button>
                </div>
            </form>
        </dialog>
    </div>

    <script>
        function initializePackingDashboard(dashboard) {
            const inModal = dashboard.querySelector('[data-in-modal]');
            const inBarcode = dashboard.querySelector('[data-in-barcode]');
            const inScanButton = dashboard.querySelector('[data-in-scan]');
            const inScanLabel = dashboard.querySelector('[data-in-scan-label]');
            const inBarcodeError = dashboard.querySelector('[data-in-barcode-error]');
            const inCustomer = dashboard.querySelector('[data-in-customer]');
            const editModal = dashboard.querySelector('[data-edit-modal]');
            const editForm = dashboard.querySelector('[data-edit-form]');
            const editProcessId = dashboard.querySelector('[data-edit-process-id]');
            const editModel = dashboard.querySelector('[data-edit-model]');
            const editNoHu = dashboard.querySelector('[data-edit-no-hu]');
            const editQty = dashboard.querySelector('[data-edit-qty]');
            const outModal = dashboard.querySelector('[data-out-modal]');
            const outForm = dashboard.querySelector('[data-out-form]');
            const outLocation = dashboard.querySelector('[data-out-location]');
            const outNoHu = dashboard.querySelector('[data-out-no-hu]');
            const outScanButton = dashboard.querySelector('[data-out-scan]');
            const outScanLabel = dashboard.querySelector('[data-out-scan-label]');
            const outList = dashboard.querySelector('[data-out-list]');
            const outCurrent = dashboard.querySelector('[data-out-current]');
            const outTotal = dashboard.querySelector('[data-out-total]');
            const outError = dashboard.querySelector('[data-out-error]');
            const completeOutButton = dashboard.querySelector('[data-complete-out]');
            const scannedProcesses = new Map();
            const boxList = dashboard.querySelector('[data-box-list]');
            const boxRowTemplate = dashboard.querySelector('[data-box-row-template]');
            const boxTotal = dashboard.querySelector('[data-box-total]');
            const boxCurrent = dashboard.querySelector('[data-box-current]');
            const maxBoxes = 50;
            const boxIdPrefix = `${dashboard.dataset.location}-`;
            const activeBoxNumbers = new Set(JSON.parse(dashboard.dataset.activeBoxNumbers || '[]'));
            const reservedBoxNumbers = new Set(activeBoxNumbers);
            boxList.querySelectorAll('input[name$="[box_id]"]').forEach(input => {
                const boxNumber = getBoxNumber(input.value);

                if (boxNumber !== null) {
                    reservedBoxNumbers.add(boxNumber);
                }
            });

            dashboard.addEventListener('pointerover', event => {
                const link = event.target.closest(
                    '[data-packing-async-navigation] a, .modern-pagination a',
                );

                if (
                    link
                    && !link.classList.contains('disabled')
                    && !link.contains(event.relatedTarget)
                ) {
                    prefetchPackingDashboard(link.href);
                }
            });

            dashboard.addEventListener('focusin', event => {
                const link = event.target.closest(
                    '[data-packing-async-navigation] a, .modern-pagination a',
                );

                if (link && !link.classList.contains('disabled')) {
                    prefetchPackingDashboard(link.href);
                }
            });

            dashboard.addEventListener('click', event => {
                const link = event.target.closest(
                    '[data-packing-async-navigation] a, .modern-pagination a',
                );

                if (!link || link.classList.contains('disabled')) {
                    return;
                }

                const destination = new URL(link.href, window.location.href);

                if (destination.origin !== window.location.origin) {
                    return;
                }

                event.preventDefault();
                loadPackingDashboard(destination.href);
            });

            dashboard.querySelector('[data-open-in-modal]')?.addEventListener('click', () => {
                inModal.showModal();
                inBarcode.focus();
            });
            inScanButton.addEventListener('click', scanInBarcode);
            inBarcode.addEventListener('keydown', event => {
                if (event.key === 'Enter') {
                    event.preventDefault();
                    scanInBarcode();
                }
            });
            boxList.addEventListener('click', event => {
                const removeButton = event.target.closest('[data-remove-box]');

                if (!removeButton || boxList.querySelectorAll('[data-box-row]').length <= 1) {
                    return;
                }

                const row = removeButton.closest('[data-box-row]');
                const allCheckbox = row.querySelector('[data-copy-qty]');
                const removedBoxNumber = getBoxNumber(
                    row.querySelector('input[name$="[box_id]"]').value,
                );

                if (allCheckbox) {
                    const nextRow = row.nextElementSibling;
                    nextRow.querySelector('.packing-qty-heading').append(allCheckbox.closest('.packing-qty-all'));
                }

                row.remove();

                if (removedBoxNumber !== null && !activeBoxNumbers.has(removedBoxNumber)) {
                    reservedBoxNumbers.delete(removedBoxNumber);
                }

                updateBoxRows();
            });

            boxList.addEventListener('change', event => {
                if (event.target.matches('[data-copy-qty]') && event.target.checked) {
                    syncAllBoxQuantities();
                }
            });

            boxList.addEventListener('input', event => {
                const copyQtyCheckbox = boxList.querySelector('[data-copy-qty]');

                if (
                    copyQtyCheckbox?.checked
                    && event.target === copyQtyCheckbox.closest('[data-box-row]')
                        .querySelector('input[name$="[qty]"]')
                ) {
                    syncAllBoxQuantities();
                }
            });

            boxList.addEventListener('focusin', event => {
                const focusedRow = event.target.closest('[data-box-row]');

                if (focusedRow) {
                    boxCurrent.textContent = [...boxList.querySelectorAll('[data-box-row]')].indexOf(focusedRow) + 1;
                }
            });

            function addBoxRow(focusBoxId = true) {
                const rows = [...boxList.querySelectorAll('[data-box-row]')];

                if (rows.length >= maxBoxes) {
                    return;
                }

                const copyQtyCheckbox = boxList.querySelector('[data-copy-qty]');
                const sourceQty = copyQtyCheckbox.closest('[data-box-row]')
                    .querySelector('input[name$="[qty]"]');
                const row = boxRowTemplate.content.firstElementChild.cloneNode(true);
                const index = rows.length;

                row.querySelectorAll('[data-box-field]').forEach(field => {
                    field.name = `boxes[${index}][${field.dataset.boxField}]`;
                });

                row.querySelector('input[name$="[box_id]"]').value = allocateBoxId();

                if (copyQtyCheckbox.checked) {
                    row.querySelector('input[name$="[qty]"]').value = sourceQty.value;
                }

                boxList.append(row);
                updateBoxRows();

                if (focusBoxId) {
                    row.querySelector('input[name$="[box_id]"]').focus();
                }
            }

            function allocateBoxId() {
                let boxNumber = 1;

                while (reservedBoxNumbers.has(boxNumber)) {
                    boxNumber++;
                }

                reservedBoxNumbers.add(boxNumber);

                return `${boxIdPrefix}${String(boxNumber).padStart(3, '0')}`;
            }

            function getBoxNumber(boxId) {
                if (!boxId.startsWith(boxIdPrefix)) {
                    return null;
                }

                const boxNumber = Number(boxId.slice(boxIdPrefix.length));

                return Number.isInteger(boxNumber) && boxNumber > 0
                    ? boxNumber
                    : null;
            }

            function syncAllBoxQuantities() {
                const copyQtyCheckbox = boxList.querySelector('[data-copy-qty]');
                const sourceRow = copyQtyCheckbox.closest('[data-box-row]');
                const quantity = sourceRow.querySelector('input[name$="[qty]"]').value;

                boxList.querySelectorAll('[data-box-row]').forEach(row => {
                    if (row !== sourceRow) {
                        row.querySelector('input[name$="[qty]"]').value = quantity;
                    }
                });
            }

            async function scanInBarcode() {
                if (inScanButton.disabled) {
                    return;
                }

                const barcode = inBarcode.value.trim();

                if (!barcode) {
                    showInBarcodeError('Scan barcode terlebih dahulu.');
                    inBarcode.focus();
                    return;
                }

                clearInBarcodeError();
                inScanButton.disabled = true;
                inScanLabel.textContent = 'MEMPROSES...';

                try {
                    const query = new URLSearchParams({ barcode });
                    const response = await fetch(
                        `{{ route('packing.processes.scan-in') }}?${query}`,
                        { headers: { Accept: 'application/json' } },
                    );
                    const payload = await response.json();

                    if (!response.ok) {
                        throw new Error(payload.message || 'Barcode gagal diproses.');
                    }

                    const hasDuplicateNoHu = [...boxList.querySelectorAll('input[name$="[no_hu]"]')]
                        .some(input => input.value.trim() === payload.no_hu);

                    if (hasDuplicateNoHu) {
                        throw new Error('No HU sudah ditambahkan di modal IN ini.');
                    }

                    let targetRow = [...boxList.querySelectorAll('[data-box-row]')].find(row => (
                        !row.querySelector('input[name$="[model]"]').value.trim()
                        || !row.querySelector('input[name$="[no_hu]"]').value.trim()
                    ));

                    if (!targetRow) {
                        if (boxList.querySelectorAll('[data-box-row]').length >= maxBoxes) {
                            throw new Error(`Maksimal ${maxBoxes} box dalam satu proses Packing IN.`);
                        }

                        addBoxRow(false);
                        targetRow = boxList.querySelector('[data-box-row]:last-child');
                    }

                    inCustomer.value = payload.customer;
                    inCustomer.readOnly = true;
                    targetRow.querySelector('input[name$="[customer]"]').value = payload.customer;
                    targetRow.querySelector('input[name$="[model]"]').value = payload.model;
                    targetRow.querySelector('input[name$="[no_hu]"]').value = payload.no_hu;
                    inBarcode.value = '';
                    inBarcode.focus();
                } catch (error) {
                    showInBarcodeError(error.message);
                    inBarcode.select();
                } finally {
                    inScanButton.disabled = false;
                    inScanLabel.textContent = 'SCAN';
                }
            }

            function showInBarcodeError(message) {
                inBarcodeError.textContent = message;
                inBarcodeError.hidden = false;
            }

            function clearInBarcodeError() {
                inBarcodeError.textContent = '';
                inBarcodeError.hidden = true;
            }

            function updateBoxRows() {
                const rows = [...boxList.querySelectorAll('[data-box-row]')];

                rows.forEach((row, index) => {
                    row.querySelectorAll('[data-box-field]').forEach(field => {
                        field.name = `boxes[${index}][${field.dataset.boxField}]`;
                    });

                    const copyQtyCheckbox = row.querySelector('[data-copy-qty]');
                    const copyQtyLabel = row.querySelector('[data-copy-qty-label]');

                    if (copyQtyCheckbox) {
                        copyQtyCheckbox.id = `packing-copy-qty-${index}`;
                        copyQtyLabel.htmlFor = copyQtyCheckbox.id;
                    }

                    const removeButton = row.querySelector('[data-remove-box]');
                    removeButton.disabled = rows.length === 1;
                    removeButton.setAttribute('aria-label', `Hapus box ${index + 1}`);
                });

                boxTotal.textContent = rows.length;
                boxCurrent.textContent = Math.min(Number(boxCurrent.textContent), rows.length);
                addBoxButton.disabled = rows.length >= maxBoxes;
            }

            updateBoxRows();

            dashboard.querySelectorAll('[data-open-edit-modal]').forEach(button => {
                button.addEventListener('click', () => openEditModal(button));
            });

            dashboard.querySelectorAll('[data-open-out-modal]').forEach(button => {
                button.addEventListener('click', () => {
                    scannedProcesses.clear();
                    clearOutError();

                    if (button.dataset.processId && button.dataset.processNoHu) {
                        addScannedProcess({
                            id: Number(button.dataset.processId),
                            no_hu: button.dataset.processNoHu,
                            model: button.dataset.processModel,
                            qty: Number(button.dataset.processQty),
                            box_id: button.dataset.processBoxId,
                        });
                    }

                    renderScannedProcesses();
                    outModal.showModal();
                    outNoHu.focus();
                });
            });

            function openEditModal(button) {
                editProcessId.value = button.dataset.processId;
                editModel.value = button.dataset.processModel;
                editNoHu.value = button.dataset.processNoHu;
                editQty.value = button.dataset.processQty;
                editForm.action = editForm.dataset.updateUrl.replace('__ID__', button.dataset.processId);
                editModal.showModal();
                editModel.focus();
            }

            outScanButton.addEventListener('click', scanNoHu);
            outNoHu.addEventListener('keydown', event => {
                if (event.key === 'Enter') {
                    event.preventDefault();
                    scanNoHu();
                }
            });

            outList.addEventListener('click', event => {
                const removeButton = event.target.closest('[data-remove-scanned]');

                if (!removeButton) {
                    return;
                }

                scannedProcesses.delete(Number(removeButton.dataset.removeScanned));
                renderScannedProcesses();
            });

            completeOutButton.addEventListener('click', confirmCompleteOut);

            async function scanNoHu() {
                if (outScanButton.disabled) {
                    return;
                }

                const noHu = outNoHu.value.trim();

                if (!noHu) {
                    showOutError('Masukkan atau scan No HU terlebih dahulu.');
                    outNoHu.focus();
                    return;
                }

                clearOutError();
                outScanButton.disabled = true;
                outScanLabel.textContent = 'MEMPROSES...';

                try {
                    const query = new URLSearchParams({
                        location: outLocation.value,
                        no_hu: noHu,
                    });
                    const response = await fetch(`${outForm.dataset.scanUrl}?${query}`, {
                        headers: { Accept: 'application/json' },
                    });
                    const payload = await response.json();

                    if (!response.ok) {
                        throw new Error(payload.message || 'No HU gagal dipindai.');
                    }

                    if (scannedProcesses.has(Number(payload.process.id))) {
                        throw new Error('No HU ini sudah ada di daftar scan.');
                    }

                    addScannedProcess(payload.process);
                    renderScannedProcesses();
                    outNoHu.value = '';
                    outNoHu.focus();
                } catch (error) {
                    showOutError(error.message);
                    outNoHu.select();
                } finally {
                    outScanButton.disabled = false;
                    outScanLabel.textContent = 'SCAN';
                }
            }

            async function confirmCompleteOut() {
                if (outNoHu.value.trim()) {
                    showOutError('No HU belum diproses. Tekan Enter atau tombol SCAN terlebih dahulu.');
                    outNoHu.focus();
                    return;
                }

                if (scannedProcesses.size === 0) {
                    showOutError('Scan minimal satu No HU sebelum Complete.');
                    outNoHu.focus();
                    return;
                }

                clearOutError();
                const result = await Swal.fire({
                    target: outModal,
                    icon: 'warning',
                    title: 'Konfirmasi Packing OUT?',
                    text: `Anda akan menyelesaikan ${scannedProcesses.size} box di ${outLocation.value}. Pastikan daftar scan sudah benar.`,
                    showCancelButton: true,
                    confirmButtonColor: '#0c6ee8',
                    cancelButtonColor: '#64748b',
                    confirmButtonText: 'Ya, Complete',
                    cancelButtonText: 'Batal',
                    focusCancel: true,
                });

                if (result.isConfirmed) {
                    await completeOut();
                }
            }

            async function completeOut() {
                if (scannedProcesses.size === 0 || outNoHu.value.trim()) {
                    showOutError(
                        outNoHu.value.trim()
                            ? 'No HU belum diproses. Tekan Enter atau tombol SCAN terlebih dahulu.'
                            : 'Scan minimal satu No HU sebelum Complete.',
                    );
                    return;
                }
                completeOutButton.disabled = true;
                completeOutButton.disabled = true;

                try {
                    const response = await fetch(outForm.dataset.completeUrl, {
                        method: 'POST',
                        headers: {
                            Accept: 'application/json',
                            'Content-Type': 'application/json',
                            'X-CSRF-TOKEN': outForm.querySelector('input[name="_token"]').value,
                        },
                        body: JSON.stringify({
                            location: outLocation.value,
                            process_ids: [...scannedProcesses.keys()],
                        }),
                    });
                    const payload = await response.json();

                    if (!response.ok) {
                        const validationMessage = Object.values(payload.errors || {}).flat()[0];
                        throw new Error(validationMessage || payload.message || 'Packing OUT gagal disimpan.');
                    }

                    await Swal.fire({
                        target: outModal,
                        icon: 'success',
                        title: 'Packing OUT berhasil!',
                        text: payload.message,
                        confirmButtonColor: '#1677d2',
                        confirmButtonText: 'OK',
                    });
                    window.location.reload();
                } catch (error) {
                    showOutError(error.message);
                    completeOutButton.disabled = false;
                }
            }

            function addScannedProcess(process) {
                scannedProcesses.set(Number(process.id), process);
            }

            function renderScannedProcesses() {
                outList.replaceChildren();

                if (scannedProcesses.size === 0) {
                    const row = document.createElement('tr');
                    const cell = document.createElement('td');
                    cell.className = 'packing-empty-state';
                    cell.colSpan = 6;
                    cell.textContent = 'Belum ada No HU dipindai.';
                    row.append(cell);
                    outList.append(row);
                } else {
                    [...scannedProcesses.values()].forEach((process, index) => {
                        const row = document.createElement('tr');
                        [index + 1, process.no_hu || '—', process.model, process.qty, process.box_id]
                            .forEach(value => {
                                const cell = document.createElement('td');
                                cell.textContent = value;
                                row.append(cell);
                            });

                        const actionCell = document.createElement('td');
                        const removeButton = document.createElement('button');
                        removeButton.className = 'packing-out-remove-button';
                        removeButton.type = 'button';
                        removeButton.dataset.removeScanned = process.id;
                        removeButton.setAttribute('aria-label', `Hapus ${process.no_hu || process.box_id}`);
                        removeButton.textContent = '×';
                        actionCell.append(removeButton);
                        row.append(actionCell);
                        outList.append(row);
                    });
                }

                const count = scannedProcesses.size;
                outCurrent.textContent = count;
                outTotal.textContent = `${count} ITEM`;
                completeOutButton.disabled = false;
            }

            function showOutError(message) {
                outError.textContent = message;
                outError.hidden = false;
            }

            function clearOutError() {
                outError.textContent = '';
                outError.hidden = true;
            }
            if (dashboard.dataset.reopenIn === 'true') {
                inModal.showModal();
                inBarcode.focus();
            }

            if (dashboard.dataset.reopenEditProcessId) {
                const failedEditProcessId = dashboard.dataset.reopenEditProcessId;
                const failedEditButton = [...dashboard.querySelectorAll('[data-open-edit-modal]')]
                    .find(button => button.dataset.processId === failedEditProcessId);

                if (failedEditButton) {
                    openEditModal(failedEditButton);
                    editModel.value = dashboard.dataset.oldEditModel;
                    editNoHu.value = dashboard.dataset.oldEditNoHu;
                    editQty.value = dashboard.dataset.oldEditQty;
                }
            }

            dashboard.querySelectorAll('[data-close-modal]').forEach(button => {
                button.addEventListener('click', () => button.closest('dialog').close());
            });

            [inModal, editModal, outModal].forEach(modal => {
                modal.addEventListener('click', event => {
                    if (event.target === modal) {
                        modal.close();
                    }
                });
            });
        }

        let packingNavigationRequest = 0;
        const packingDashboardCache = new Map();
        const packingDashboardCacheLimit = 10;

        function fetchPackingDashboard(url) {
            if (!packingDashboardCache.has(url)) {
                let request;
                request = fetch(url, {
                    headers: {
                        Accept: 'text/html',
                        'X-Requested-With': 'XMLHttpRequest',
                    },
                }).then(async response => {
                    if (!response.ok) {
                        throw new Error('Data Packing gagal dimuat. Silakan coba kembali.');
                    }

                    return response.text();
                }).catch(error => {
                    if (packingDashboardCache.get(url) === request) {
                        packingDashboardCache.delete(url);
                    }

                    throw error;
                });

                packingDashboardCache.set(url, request);

                if (packingDashboardCache.size > packingDashboardCacheLimit) {
                    const oldestUrl = packingDashboardCache.keys().next().value;
                    packingDashboardCache.delete(oldestUrl);
                }
            }

            return packingDashboardCache.get(url);
        }

        function prefetchPackingDashboard(url) {
            const destination = new URL(url, window.location.href);

            if (destination.origin === window.location.origin) {
                fetchPackingDashboard(destination.href).catch(() => {});
            }
        }

        async function loadPackingDashboard(url, updateHistory = true) {
            const requestId = ++packingNavigationRequest;
            const currentDashboard = document.querySelector('[data-packing-dashboard]');

            if (!currentDashboard) {
                return;
            }

            currentDashboard.setAttribute('aria-busy', 'true');

            try {
                const responseDocument = new DOMParser().parseFromString(
                    await fetchPackingDashboard(url),
                    'text/html',
                );
                const nextDashboard = responseDocument.querySelector('[data-packing-dashboard]');

                if (!nextDashboard) {
                    throw new Error('Tampilan Packing tidak ditemukan pada respons server.');
                }

                if (requestId !== packingNavigationRequest) {
                    return;
                }
                nextDashboard.setAttribute('aria-busy', 'false');
                nextDashboard.setAttribute('aria-busy', 'false');
                currentDashboard.replaceWith(nextDashboard);
                initializePackingDashboard(nextDashboard);

                if (updateHistory) {
                    window.history.pushState({}, '', url);
                }
            } catch (error) {
                if (requestId === packingNavigationRequest) {
                    currentDashboard.setAttribute('aria-busy', 'false');
                    await Swal.fire({
                        icon: 'error',
                        title: 'Gagal memuat data',
                        text: error.message,
                        confirmButtonColor: '#1677d2',
                    });
                }
            }
        }

        const initialPackingDashboard = document.querySelector('[data-packing-dashboard]');

        if (initialPackingDashboard) {
            initializePackingDashboard(initialPackingDashboard);
        }

        window.addEventListener('popstate', () => {
            loadPackingDashboard(window.location.href, false);
        });
    </script>
</x-sidebar>
