<?php

namespace Tests\Feature;

use App\Models\PackingProcess;
use Illuminate\Foundation\Testing\RefreshDatabase;
use Illuminate\Support\Facades\DB;
use Tests\TestCase;

class PackingProcessWorkflowTest extends TestCase
{
    use RefreshDatabase;

    public function test_packing_index_lists_active_records_for_selected_wp_location(): void
    {
        PackingProcess::query()->create([
            'model' => 'MODEL-A',
            'no_hu' => 'HU-001',
            'customer' => 'Customer A',
            'location' => 'WP-1',
            'box_id' => 'BOX-001',
            'qty' => 12,
            'process_status' => 'IN',
            'time_in' => now(),
        ]);

        PackingProcess::query()->create([
            'model' => 'MODEL-B',
            'customer' => 'Customer B',
            'location' => 'WP-2',
            'box_id' => 'BOX-002',
            'qty' => 4,
            'process_status' => 'IN',
            'time_in' => now(),
        ]);

        PackingProcess::query()->create([
            'model' => 'MODEL-HISTORY',
            'no_hu' => 'HU-HISTORY',
            'customer' => 'Customer History',
            'location' => 'WP-1',
            'box_id' => 'BOX-HISTORY',
            'qty' => 3,
            'process_status' => 'OUT',
            'time_in' => now()->subHour(),
            'time_out' => now(),
        ]);

        $this->get(route('packing.index', ['location' => 'WP-1']))
            ->assertOk()
            ->assertSee('MODEL-A')
            ->assertSee('Customer A')
            ->assertSee('HU-001')
            ->assertSee('Customer History')
            ->assertSee('MODEL-HISTORY')
            ->assertSee('value="WP-1-001"', false)
            ->assertSee('Customer', false)
            ->assertDontSee('ADD OUT')
            ->assertDontSee('data-add-box', false)
            ->assertSee('data-box-field="customer"', false)
            ->assertSee('data-open-edit-modal', false)
            ->assertSee('Edit Packing')
            ->assertSee('boxes[0][no_hu]', false)
            ->assertSee('data-in-barcode', false)
            ->assertSee('data-in-customer', false)
            ->assertSee('inCustomer.readOnly = true', false)
            ->assertSee('hasDuplicateNoHu', false)
            ->assertSee('No HU sudah ditambahkan di modal IN ini.', false)
            ->assertSee('packing-out-no-hu')
            ->assertSee('LIST PART / CODE YANG DI SCAN')
            ->assertSee('No HU')
            ->assertSee('COMPLETE')
            ->assertSee('Swal.fire', false)
            ->assertSee('target: outModal', false)
            ->assertSee('Konfirmasi Packing OUT?', false)
            ->assertSee('Ya, Complete', false)
            ->assertSee('data-copy-qty', false)
            ->assertDontSee('data-open-out-modal disabled', false)
            ->assertDontSee('MODEL-B');
    }

    public function test_active_packing_and_history_use_separate_pagination_with_five_and_ten_records(): void
    {
        foreach (range(1, 6) as $number) {
            PackingProcess::query()->create([
                'model' => sprintf('ACTIVE-%02d', $number),
                'customer' => 'Customer A',
                'location' => 'WP-1',
                'box_id' => sprintf('WP-1-A%02d', $number),
                'qty' => 1,
            ]);
        }

        foreach (range(1, 11) as $number) {
            PackingProcess::query()->create([
                'model' => sprintf('OUT-%02d', $number),
                'customer' => 'Customer A',
                'location' => 'WP-1',
                'box_id' => sprintf('WP-1-O%02d', $number),
                'qty' => 1,
                'process_status' => 'OUT',
                'time_out' => now(),
            ]);
        }

        $this->get(route('packing.index', [
            'location' => 'WP-1',
            'active_page' => 2,
            'history_page' => 1,
        ]))
            ->assertOk()
            ->assertSee('ACTIVE-01')
            ->assertDontSee('ACTIVE-02')
            ->assertSee('OUT-11')
            ->assertSee('OUT-02')
            ->assertDontSee('OUT-01')
            ->assertSee('active_page=1', false)
            ->assertSee('history_page=2', false)
            ->assertSee('pcsPagination-active_page', false)
            ->assertSee('pcsPagination-history_page', false)
            ->assertSee('data-packing-async-navigation', false)
            ->assertSee('loadPackingDashboard(destination.href)', false)
            ->assertSee('prefetchPackingDashboard(link.href)', false)
            ->assertSee('fetchPackingDashboard(url)', false)
            ->assertSee("window.history.pushState({}, '', url)", false)
            ->assertSee("window.addEventListener('popstate'", false);
    }

    public function test_packing_in_barcode_populates_customer_model_and_no_hu(): void
    {
        DB::table('customers')->insert([
            'customer_code' => '1405',
            'customer_name' => 'HN',
            'country_key' => 'Hino Motors',
            'created_at' => now(),
            'updated_at' => now(),
        ]);

        $this->getJson(route('packing.processes.scan-in', [
            'barcode' => '@1405-01676@00000050000011174213@89630-EW010-C@@3@@',
        ]))
            ->assertOk()
            ->assertJsonPath('customer_code', '1405')
            ->assertJsonPath('customer', 'HN')
            ->assertJsonPath('no_hu', '50000011174213')
            ->assertJsonPath('model', '89630-EW010-C');
    }

    public function test_packing_in_barcode_rejects_an_invalid_format(): void
    {
        $this->getJson(route('packing.processes.scan-in', [
            'barcode' => 'invalid-barcode',
        ]))
            ->assertUnprocessable()
            ->assertJsonPath('message', 'Format barcode tidak valid.');
    }

    public function test_packing_in_barcode_rejects_an_unknown_customer_code(): void
    {
        $this->getJson(route('packing.processes.scan-in', [
            'barcode' => '@9999-01676@00000050000011174213@89630-EW010-C@@3@@',
        ]))
            ->assertNotFound()
            ->assertJsonPath('message', 'Customer dengan kode 9999 tidak ditemukan.');
    }

    public function test_packing_in_barcode_rejects_a_no_hu_already_saved_at_any_location(): void
    {
        PackingProcess::query()->create([
            'model' => 'MODEL-A',
            'no_hu' => '50000011174213',
            'customer' => 'Customer A',
            'location' => 'WP-7',
            'box_id' => 'WP-7-001',
            'qty' => 1,
            'process_status' => 'OUT',
            'time_out' => now(),
        ]);
        DB::table('customers')->insert([
            'customer_code' => '1405',
            'customer_name' => 'HN',
            'country_key' => 'Hino Motors',
            'created_at' => now(),
            'updated_at' => now(),
        ]);

        $this->getJson(route('packing.processes.scan-in', [
            'barcode' => '@1405-01676@00000050000011174213@89630-EW010-C@@3@@',
        ]))
            ->assertConflict()
            ->assertJsonPath('message', 'No HU sudah terdaftar pada data Packing.');
    }

    public function test_out_buttons_are_disabled_when_location_has_no_scannable_active_data(): void
    {
        $this->get(route('packing.index', ['location' => 'WP-1']))
            ->assertOk()
            ->assertSee('data-open-out-modal disabled', false);

        PackingProcess::query()->create([
            'model' => 'MODEL-A',
            'customer' => 'Customer A',
            'location' => 'WP-1',
            'box_id' => 'WP-1-001',
            'qty' => 12,
            'process_status' => 'IN',
            'time_in' => now(),
        ]);

        $response = $this->get(route('packing.index', ['location' => 'WP-1']));
        $response->assertOk()
            ->assertSee('EDIT', false);

        $this->assertSame(
            1,
            preg_match_all('/<button\b(?=[^>]*data-open-out-modal)(?=[^>]*disabled)[^>]*>/i', $response->getContent()),
        );
    }

    public function test_packing_in_renders_all_checkbox_only_on_first_box_row(): void
    {
        $response = $this->get(route('packing.index'));

        $response->assertOk()
            ->assertSee('value="WP-1-001"', false);

        $this->assertSame(
            1,
            preg_match_all('/<input\b[^>]*\bdata-copy-qty\b[^>]*>/i', $response->getContent()),
        );
    }

    public function test_box_ids_use_the_first_available_number_for_each_selected_location(): void
    {
        PackingProcess::query()->create([
            'model' => 'MODEL-A',
            'customer' => 'Customer A',
            'location' => 'WP-1',
            'box_id' => 'WP-1-004',
            'qty' => 1,
        ]);
        PackingProcess::query()->create([
            'model' => 'MODEL-B',
            'customer' => 'Customer B',
            'location' => 'WP-2',
            'box_id' => 'WP-2-012',
            'qty' => 1,
        ]);

        $this->get(route('packing.index', ['location' => 'WP-1']))
            ->assertOk()
            ->assertSee('value="WP-1-001"', false);

        $this->get(route('packing.index', ['location' => 'WP-2']))
            ->assertOk()
            ->assertSee('value="WP-2-001"', false);
    }

    public function test_box_id_from_out_process_is_available_for_the_next_packing_in(): void
    {
        PackingProcess::query()->create([
            'model' => 'MODEL-A',
            'customer' => 'Customer A',
            'location' => 'WP-1',
            'box_id' => 'WP-1-001',
            'qty' => 1,
        ]);
        PackingProcess::query()->create([
            'model' => 'MODEL-B',
            'customer' => 'Customer B',
            'location' => 'WP-1',
            'box_id' => 'WP-1-002',
            'qty' => 1,
            'process_status' => 'OUT',
            'time_out' => now(),
        ]);
        PackingProcess::query()->create([
            'model' => 'MODEL-C',
            'customer' => 'Customer C',
            'location' => 'WP-1',
            'box_id' => 'WP-1-003',
            'qty' => 1,
        ]);

        $this->get(route('packing.index', ['location' => 'WP-1']))
            ->assertOk()
            ->assertSee('value="WP-1-002"', false);
    }

    public function test_packing_in_creates_a_record_with_location_and_time(): void
    {
        $this->post(route('packing.processes.store'), [
            'boxes' => [[
                'model' => 'MODEL-A',
                'no_hu' => 'HU-001',
                'box_id' => 'WP-3-001',
                'qty' => 12,
            ]],
            'customer' => 'Customer A',
            'location' => 'WP-3',
            'remarks' => 'PIC A',
        ])
            ->assertRedirect(route('packing.index', ['location' => 'WP-3']));

        $this->assertDatabaseHas('packing_processes', [
            'model' => 'MODEL-A',
            'no_hu' => 'HU-001',
            'location' => 'WP-3',
            'box_id' => 'WP-3-001',
            'qty' => 12,
            'process_status' => 'IN',
        ]);

        $process = PackingProcess::query()->firstOrFail();
        $this->assertNotNull($process->time_in);
        $this->assertNull($process->time_out);
    }

    public function test_dashboard_edit_updates_only_model_no_hu_and_qty_for_active_packing(): void
    {
        $process = PackingProcess::query()->create([
            'model' => 'MODEL-A',
            'no_hu' => 'HU-001',
            'customer' => 'Customer A',
            'location' => 'WP-2',
            'box_id' => 'WP-2-001',
            'qty' => 12,
            'remarks' => 'PIC A',
            'time_in' => now()->subMinutes(5),
        ]);

        $this->patch(route('packing.processes.update', $process), [
            'edit_process_id' => $process->id,
            'model' => 'MODEL-UPDATED',
            'no_hu' => 'HU-UPDATED',
            'qty' => 25,
            'customer' => 'Tampered Customer',
            'location' => 'WP-5',
            'box_id' => 'WP-5-999',
        ])
            ->assertRedirect(route('packing.index', ['location' => 'WP-2']))
            ->assertSessionHas('success', 'Data packing berhasil diperbarui.');

        $this->assertDatabaseHas('packing_processes', [
            'id' => $process->id,
            'model' => 'MODEL-UPDATED',
            'no_hu' => 'HU-UPDATED',
            'qty' => 25,
            'customer' => 'Customer A',
            'location' => 'WP-2',
            'box_id' => 'WP-2-001',
            'remarks' => 'PIC A',
            'process_status' => 'IN',
        ]);
    }

    public function test_dashboard_edit_rejects_invalid_qty_without_updating_packing(): void
    {
        $process = PackingProcess::query()->create([
            'model' => 'MODEL-A',
            'no_hu' => 'HU-001',
            'customer' => 'Customer A',
            'location' => 'WP-2',
            'box_id' => 'WP-2-001',
            'qty' => 12,
        ]);

        $this->from(route('packing.index', ['location' => 'WP-2']))
            ->patch(route('packing.processes.update', $process), [
                'edit_process_id' => $process->id,
                'model' => 'MODEL-UPDATED',
                'no_hu' => 'HU-UPDATED',
                'qty' => 0,
            ])
            ->assertRedirect(route('packing.index', ['location' => 'WP-2']))
            ->assertSessionHasErrors('qty');

        $this->assertDatabaseHas('packing_processes', [
            'id' => $process->id,
            'model' => 'MODEL-A',
            'no_hu' => 'HU-001',
            'qty' => 12,
        ]);
    }

    public function test_dashboard_edit_rejects_a_no_hu_already_used_by_another_packing(): void
    {
        $process = PackingProcess::query()->create([
            'model' => 'MODEL-A',
            'no_hu' => 'HU-001',
            'customer' => 'Customer A',
            'location' => 'WP-2',
            'box_id' => 'WP-2-001',
            'qty' => 12,
        ]);
        PackingProcess::query()->create([
            'model' => 'MODEL-B',
            'no_hu' => 'HU-002',
            'customer' => 'Customer B',
            'location' => 'WP-7',
            'box_id' => 'WP-7-001',
            'qty' => 8,
        ]);

        $this->from(route('packing.index', ['location' => 'WP-2']))
            ->patch(route('packing.processes.update', $process), [
                'model' => 'MODEL-UPDATED',
                'no_hu' => 'HU-002',
                'qty' => 12,
            ])
            ->assertRedirect(route('packing.index', ['location' => 'WP-2']))
            ->assertSessionHasErrors([
                'no_hu' => 'No HU sudah terdaftar pada data Packing.',
            ]);

        $this->assertDatabaseHas('packing_processes', [
            'id' => $process->id,
            'model' => 'MODEL-A',
            'no_hu' => 'HU-001',
        ]);
    }

    public function test_dashboard_edit_rejects_out_packing_without_updating_it(): void
    {
        $process = PackingProcess::query()->create([
            'model' => 'MODEL-A',
            'no_hu' => 'HU-001',
            'customer' => 'Customer A',
            'location' => 'WP-2',
            'box_id' => 'WP-2-001',
            'qty' => 12,
            'process_status' => 'OUT',
            'time_out' => now(),
        ]);

        $this->patch(route('packing.processes.update', $process), [
            'edit_process_id' => $process->id,
            'model' => 'MODEL-UPDATED',
            'no_hu' => 'HU-UPDATED',
            'qty' => 25,
        ])
            ->assertRedirect(route('packing.index', ['location' => 'WP-2']))
            ->assertSessionHasErrors('packing_process');

        $this->assertDatabaseHas('packing_processes', [
            'id' => $process->id,
            'model' => 'MODEL-A',
            'no_hu' => 'HU-001',
            'qty' => 12,
            'process_status' => 'OUT',
        ]);
    }

    public function test_packing_in_saves_each_scanned_customer_with_its_box_and_location(): void
    {
        $this->post(route('packing.processes.store'), [
            'location' => 'WP-2',
            'customer' => 'Customer B',
            'remarks' => 'PIC B',
            'boxes' => [
                [
                    'box_id' => 'WP-2-001',
                    'model' => 'MODEL-B',
                    'no_hu' => 'HU-101',
                    'qty' => 8,
                ],
                [
                    'box_id' => 'WP-2-002',
                    'customer' => 'Customer C',
                    'model' => 'MODEL-C',
                    'no_hu' => 'HU-102',
                    'qty' => 8,
                ],
            ],
        ])
            ->assertRedirect(route('packing.index', ['location' => 'WP-2']));

        $this->assertDatabaseCount('packing_processes', 2);
        $this->assertDatabaseHas('packing_processes', [
            'box_id' => 'WP-2-001',
            'model' => 'MODEL-B',
            'no_hu' => 'HU-101',
            'qty' => 8,
            'customer' => 'Customer B',
            'location' => 'WP-2',
            'remarks' => 'PIC B',
            'process_status' => 'IN',
        ]);
        $this->assertDatabaseHas('packing_processes', [
            'box_id' => 'WP-2-002',
            'model' => 'MODEL-C',
            'no_hu' => 'HU-102',
            'qty' => 8,
            'customer' => 'Customer C',
            'location' => 'WP-2',
            'remarks' => 'PIC B',
            'process_status' => 'IN',
        ]);
    }

    public function test_packing_in_rejects_duplicate_no_hu_values_within_the_same_submission(): void
    {
        $this->from(route('packing.index'))
            ->post(route('packing.processes.store'), [
                'location' => 'WP-2',
                'customer' => 'Customer B',
                'boxes' => [
                    [
                        'box_id' => 'WP-2-001',
                        'model' => 'MODEL-B',
                        'no_hu' => 'HU-DUPLICATE',
                        'qty' => 8,
                    ],
                    [
                        'box_id' => 'WP-2-002',
                        'customer' => 'Customer C',
                        'model' => 'MODEL-C',
                        'no_hu' => 'HU-DUPLICATE',
                        'qty' => 8,
                    ],
                ],
            ])
            ->assertRedirect(route('packing.index'))
            ->assertSessionHasErrors([
                'boxes.1.no_hu' => 'No HU tidak boleh digunakan lebih dari satu kali dalam proses Packing IN.',
            ]);

        $this->assertDatabaseCount('packing_processes', 0);
    }

    public function test_packing_in_rejects_a_no_hu_already_saved_at_any_location(): void
    {
        PackingProcess::query()->create([
            'model' => 'MODEL-A',
            'no_hu' => 'HU-EXISTING',
            'customer' => 'Customer A',
            'location' => 'WP-1',
            'box_id' => 'WP-1-001',
            'qty' => 1,
        ]);

        $this->from(route('packing.index', ['location' => 'WP-2']))
            ->post(route('packing.processes.store'), [
                'location' => 'WP-2',
                'customer' => 'Customer B',
                'boxes' => [[
                    'box_id' => 'WP-2-001',
                    'model' => 'MODEL-B',
                    'no_hu' => 'HU-EXISTING',
                    'qty' => 8,
                ]],
            ])
            ->assertRedirect(route('packing.index', ['location' => 'WP-2']))
            ->assertSessionHasErrors([
                'boxes.0.no_hu' => 'No HU sudah terdaftar pada data Packing.',
            ]);

        $this->assertDatabaseCount('packing_processes', 1);
    }

    public function test_packing_in_does_not_save_any_box_when_a_row_is_invalid(): void
    {
        $this->from(route('packing.index'))
            ->post(route('packing.processes.store'), [
                'location' => 'WP-2',
                'customer' => 'Customer B',
                'boxes' => [
                    [
                        'box_id' => 'BOX-101',
                        'model' => 'MODEL-B',
                        'qty' => 8,
                    ],
                    [
                        'box_id' => 'BOX-102',
                        'model' => '',
                        'qty' => 8,
                    ],
                ],
            ])
            ->assertRedirect(route('packing.index'))
            ->assertSessionHasErrors('boxes.1.model');

        $this->assertDatabaseCount('packing_processes', 0);
    }

    public function test_packing_out_marks_the_active_record_as_complete(): void
    {
        $process = PackingProcess::query()->create([
            'model' => 'MODEL-A',
            'customer' => 'Customer A',
            'location' => 'WP-1',
            'box_id' => 'BOX-001',
            'qty' => 12,
            'process_status' => 'IN',
            'time_in' => now()->subMinutes(5),
        ]);

        $this->post(route('packing.processes.out', $process))
            ->assertRedirect(route('packing.index', ['location' => 'WP-1']));

        $process->refresh();
        $this->assertSame('OUT', $process->process_status);
        $this->assertNotNull($process->time_out);
    }

    public function test_packing_out_modal_scans_an_active_record_by_no_hu_in_selected_location(): void
    {
        $process = PackingProcess::query()->create([
            'model' => 'MODEL-A',
            'no_hu' => 'HU-OUT-001',
            'customer' => 'Customer A',
            'location' => 'WP-1',
            'box_id' => 'WP-1-001',
            'qty' => 25,
            'time_in' => now(),
        ]);
        PackingProcess::query()->create([
            'model' => 'MODEL-C',
            'no_hu' => 'HU-OUT-003',
            'customer' => 'Customer C',
            'location' => 'WP-1',
            'box_id' => 'WP-1-003',
            'qty' => 5,
            'process_status' => 'OUT',
            'time_in' => now()->subHour(),
            'time_out' => now(),
        ]);
        PackingProcess::query()->create([
            'model' => 'MODEL-B',
            'no_hu' => 'HU-OUT-002',
            'customer' => 'Customer B',
            'location' => 'WP-2',
            'box_id' => 'WP-2-001',
            'qty' => 10,
            'time_in' => now(),
        ]);

        $this->getJson(route('packing.processes.scan', [
            'location' => 'WP-1',
            'no_hu' => 'HU-OUT-001',
        ]))
            ->assertOk()
            ->assertJsonPath('process.id', $process->id)
            ->assertJsonPath('process.no_hu', 'HU-OUT-001')
            ->assertJsonPath('process.model', 'MODEL-A')
            ->assertJsonPath('process.qty', 25)
            ->assertJsonPath('process.box_id', 'WP-1-001');

        $this->getJson(route('packing.processes.scan', [
            'location' => 'WP-1',
            'no_hu' => 'HU-OUT-002',
        ]))->assertNotFound();

        $this->getJson(route('packing.processes.scan', [
            'location' => 'WP-1',
            'no_hu' => 'HU-OUT-003',
        ]))->assertNotFound();
    }

    public function test_complete_out_marks_all_scanned_processes_as_out(): void
    {
        $firstProcess = PackingProcess::query()->create([
            'model' => 'MODEL-A',
            'no_hu' => 'HU-OUT-101',
            'customer' => 'Customer A',
            'location' => 'WP-3',
            'box_id' => 'WP-3-001',
            'qty' => 11,
            'time_in' => now(),
        ]);
        $secondProcess = PackingProcess::query()->create([
            'model' => 'MODEL-B',
            'no_hu' => 'HU-OUT-102',
            'customer' => 'Customer B',
            'location' => 'WP-3',
            'box_id' => 'WP-3-002',
            'qty' => 14,
            'time_in' => now(),
        ]);

        $this->postJson(route('packing.processes.complete-out'), [
            'location' => 'WP-3',
            'process_ids' => [$firstProcess->id, $secondProcess->id],
        ])
            ->assertOk()
            ->assertJsonPath('completed', 2);

        $this->assertDatabaseHas('packing_processes', [
            'id' => $firstProcess->id,
            'process_status' => 'OUT',
        ]);
        $this->assertDatabaseHas('packing_processes', [
            'id' => $secondProcess->id,
            'process_status' => 'OUT',
        ]);
        $this->assertNotNull($firstProcess->fresh()->time_out);
        $this->assertNotNull($secondProcess->fresh()->time_out);
    }

    public function test_complete_out_rejects_a_process_from_another_location_without_partial_updates(): void
    {
        $firstProcess = PackingProcess::query()->create([
            'model' => 'MODEL-A',
            'customer' => 'Customer A',
            'location' => 'WP-3',
            'box_id' => 'WP-3-001',
            'qty' => 11,
            'time_in' => now(),
        ]);
        $otherLocationProcess = PackingProcess::query()->create([
            'model' => 'MODEL-B',
            'customer' => 'Customer B',
            'location' => 'WP-4',
            'box_id' => 'WP-4-001',
            'qty' => 14,
            'time_in' => now(),
        ]);

        $this->postJson(route('packing.processes.complete-out'), [
            'location' => 'WP-3',
            'process_ids' => [$firstProcess->id, $otherLocationProcess->id],
        ])
            ->assertConflict()
            ->assertJsonPath('message', 'Sebagian packing sudah OUT, tidak memiliki No HU, atau tidak termasuk lokasi ini. Muat ulang halaman dan scan kembali.');

        $this->assertDatabaseHas('packing_processes', [
            'id' => $firstProcess->id,
            'process_status' => 'IN',
            'time_out' => null,
        ]);
        $this->assertDatabaseHas('packing_processes', [
            'id' => $otherLocationProcess->id,
            'process_status' => 'IN',
            'time_out' => null,
        ]);
    }

    public function test_complete_out_requires_at_least_one_scanned_process(): void
    {
        $process = PackingProcess::query()->create([
            'model' => 'MODEL-A',
            'no_hu' => 'HU-OUT-201',
            'customer' => 'Customer A',
            'location' => 'WP-3',
            'box_id' => 'WP-3-001',
            'qty' => 11,
            'time_in' => now(),
        ]);

        $this->postJson(route('packing.processes.complete-out'), [
            'location' => 'WP-3',
            'process_ids' => [],
        ])
            ->assertUnprocessable()
            ->assertJsonValidationErrors('process_ids');

        $this->assertDatabaseHas('packing_processes', [
            'id' => $process->id,
            'process_status' => 'IN',
            'time_out' => null,
        ]);
    }

    public function test_complete_out_rejects_duplicate_process_ids(): void
    {
        $process = PackingProcess::query()->create([
            'model' => 'MODEL-A',
            'no_hu' => 'HU-OUT-202',
            'customer' => 'Customer A',
            'location' => 'WP-3',
            'box_id' => 'WP-3-001',
            'qty' => 11,
            'time_in' => now(),
        ]);

        $this->postJson(route('packing.processes.complete-out'), [
            'location' => 'WP-3',
            'process_ids' => [$process->id, $process->id],
        ])
            ->assertUnprocessable()
            ->assertJsonValidationErrors('process_ids.1');

        $this->assertDatabaseHas('packing_processes', [
            'id' => $process->id,
            'process_status' => 'IN',
            'time_out' => null,
        ]);
    }

    public function test_complete_out_rejects_active_process_without_no_hu(): void
    {
        $process = PackingProcess::query()->create([
            'model' => 'MODEL-A',
            'customer' => 'Customer A',
            'location' => 'WP-3',
            'box_id' => 'WP-3-001',
            'qty' => 11,
            'time_in' => now(),
        ]);

        $this->postJson(route('packing.processes.complete-out'), [
            'location' => 'WP-3',
            'process_ids' => [$process->id],
        ])
            ->assertConflict()
            ->assertJsonPath('message', 'Sebagian packing sudah OUT, tidak memiliki No HU, atau tidak termasuk lokasi ini. Muat ulang halaman dan scan kembali.');

        $this->assertDatabaseHas('packing_processes', [
            'id' => $process->id,
            'process_status' => 'IN',
            'time_out' => null,
        ]);
    }

    public function test_packing_in_rejects_locations_outside_wp_range(): void
    {
        $this->from(route('packing.index'))
            ->post(route('packing.processes.store'), [
                'location' => 'WP-8',
                'customer' => 'Customer A',
                'boxes' => [[
                    'model' => 'MODEL-A',
                    'box_id' => 'BOX-001',
                    'qty' => 12,
                ]],
            ])
            ->assertRedirect(route('packing.index'))
            ->assertSessionHasErrors('location');

        $this->assertDatabaseCount('packing_processes', 0);
    }
}
