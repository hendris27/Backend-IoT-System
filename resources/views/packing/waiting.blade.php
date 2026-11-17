@vite('resources/css/packing.css')

<x-sidebar title="Waiting Packing">
    <section class="packing-waiting-page" data-waiting-packing-page>
        @if (session('success'))
            <p class="packing-feedback packing-feedback--success" role="status">{{ session('success') }}</p>
        @endif

        @if ($errors->any())
            <div class="packing-feedback packing-feedback--error" role="alert">
                <p>Perubahan belum dapat disimpan:</p>
                <ul>
                    @foreach ($errors->all() as $error)
                        <li>{{ $error }}</li>
                    @endforeach
                </ul>
            </div>
        @endif



        <section class="packing-process-panel packing-waiting-panel">
            <div class="packing-table-wrap">
                <table class="packing-table packing-waiting-table">
                    <thead>
                        <tr>
                            <th scope="col">No.</th>
                            <th scope="col">Customer</th>
                            <th scope="col">Model</th>
                            <th scope="col">No HU</th>
                            <th scope="col">Lokasi</th>
                            <th scope="col">Box</th>
                            <th scope="col">QTY</th>
                            <th scope="col">Time IN</th>
                            <th scope="col">PIC IN</th>
                            <th scope="col">Remarks</th>
                            <th scope="col">Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        @forelse ($waitingProcesses as $index => $process)
                            <tr>
                                <td>{{ $waitingProcesses->firstItem() + $index }}</td>
                                <td>{{ $process->customer }}</td>
                                <td>{{ $process->model }}</td>
                                <td>{{ $process->no_hu ?: '—' }}</td>
                                <td>{{ $process->location }}</td>
                                <td>{{ $process->box_id }}</td>
                                <td>{{ number_format($process->qty) }}</td>
                                <td>{{ $process->time_in?->copy()->timezone('Asia/Bangkok')->format('d M Y, H:i') ?: '—' }}</td>
                                <td>{{ $process->inUser?->name ?: '—' }}</td>
                                <td class="packing-remarks-cell">{{ $process->remarks ?: '—' }}</td>
                                <td>
                                    <button class="packing-row-action" type="button"
                                        data-open-remarks-modal
                                        data-update-url="{{ route('packing.processes.remarks.update', ['packingProcess' => $process->id]) }}"
                                        data-remarks="{{ $process->remarks ?? '' }}"
                                        aria-label="Edit remarks {{ $process->box_id }}">
                                        Edit Remarks
                                    </button>
                                </td>
                            </tr>
                        @empty
                            <tr>
                                <td class="packing-empty-state" colspan="11">
                                    <div class="packing-empty-content">
                                        <svg viewBox="0 0 24 24" aria-hidden="true">
                                            <circle cx="12" cy="12" r="8.5"></circle>
                                            <path d="M12 7v5l3.2 2"></path>
                                        </svg>
                                        <span>Data waiting packing belum tersedia</span>
                                    </div>
                                </td>
                            </tr>
                        @endforelse
                    </tbody>
                </table>
            </div>

            @if ($waitingProcesses->hasPages())
                <div class="packing-pagination">
                    @include('packing.pagination.pagination', ['data' => $waitingProcesses])
                </div>
            @endif
        </section>

        <dialog class="packing-modal packing-remarks-modal" data-waiting-remarks-modal
            aria-labelledby="waiting-remarks-title">
            <form class="packing-modal-form" method="POST" data-waiting-remarks-form>
                @csrf
                @method('PATCH')
                <div class="packing-modal-heading">
                    <div>
                        <p class="packing-eyebrow">Waiting Packing</p>
                        <h2 id="waiting-remarks-title">Edit Remarks</h2>
                    </div>
                    <button class="packing-modal-close" type="button" data-close-remarks-modal
                        aria-label="Tutup">×</button>
                </div>

                <div class="packing-modal-fields">
                    <label class="packing-modal-field--wide">
                        Remarks
                        <textarea name="remarks" rows="5" maxlength="5000" data-waiting-remarks-input></textarea>
                    </label>
                </div>

                <div class="packing-modal-footer">
                    <button class="packing-modal-secondary" type="button" data-close-remarks-modal>Batal</button>
                    <button class="packing-action-button packing-action-button--in" type="submit">Simpan Remarks</button>
                </div>
            </form>
        </dialog>

        <script>
            (() => {
                const page = document.querySelector('[data-waiting-packing-page]');
                const dialog = page?.querySelector('[data-waiting-remarks-modal]');
                const form = page?.querySelector('[data-waiting-remarks-form]');
                const remarksInput = page?.querySelector('[data-waiting-remarks-input]');

                if (!page || !dialog || !form || !remarksInput) {
                    return;
                }

                page.querySelectorAll('[data-open-remarks-modal]').forEach(button => {
                    button.addEventListener('click', () => {
                        form.action = button.dataset.updateUrl;
                        remarksInput.value = button.dataset.remarks || '';
                        dialog.showModal();
                        remarksInput.focus();
                    });
                });

                dialog.querySelectorAll('[data-close-remarks-modal]').forEach(button => {
                    button.addEventListener('click', () => dialog.close());
                });

                dialog.addEventListener('click', event => {
                    if (event.target === dialog) {
                        dialog.close();
                    }
                });
            })();
        </script>
    </section>
</x-sidebar>
