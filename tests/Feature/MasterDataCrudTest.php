<?php

namespace Tests\Feature;

use App\Models\MachineStatus;
use App\Models\MasterData;
use Illuminate\Foundation\Testing\RefreshDatabase;
use Illuminate\Support\Facades\Http;
use Tests\TestCase;

class MasterDataCrudTest extends TestCase
{
    use RefreshDatabase;

    public function test_machine_route_renders_the_moved_interlock_view(): void
    {
        $this->get(route('machine'))
            ->assertOk()
            ->assertSee('Backend IoT System')
            ->assertSee('Menu Interlock')
            ->assertSee('Dashboard')
            ->assertSee('Machine')
            ->assertSee('Master Data')
            ->assertSee('href="'.route('dashboard').'"', false)
            ->assertSee('href="'.route('machine').'"', false)
            ->assertSee('href="'.route('master-data.index').'"', false)
            ->assertSee('class="interlock-menu-card active"', false);

        $this->get(route('home'))
            ->assertRedirect(route('machine'));
    }

    public function test_dashboard_displays_the_latest_machine_status_per_line(): void
    {
        Http::preventStrayRequests();
        Http::fake([
            '127.0.0.1:1880/api/interlock/material' => Http::response([], 200),
        ]);

        $this->postJson('/interlock/machine-status', [
            'line' => 'Line 1',
            'status' => 'Running',
        ])->assertOk();

        $this->get(route('dashboard'))
            ->assertOk()
            ->assertSee('Machine Status')
            ->assertSee('Line 1')
            ->assertSee('Running')
            ->assertSee('Line 2')
            ->assertSee('Tidak Running');
    }

    public function test_dashboard_paginates_machine_lines_ten_per_page(): void
    {
        $this->get(route('dashboard'))
            ->assertOk()
            ->assertSee('Line 10')
            ->assertDontSee('Line 11')
            ->assertSeeText('Halaman 1 dari 3');

        $this->get(route('dashboard', ['page' => 2]))
            ->assertOk()
            ->assertSee('Line 11')
            ->assertDontSee('Line 21')
            ->assertSeeText('Halaman 2 dari 3');
    }

    public function test_dashboard_shows_and_resets_the_machine_start_time(): void
    {
        Http::preventStrayRequests();
        Http::fake([
            '127.0.0.1:1880/api/interlock/material' => Http::response([], 200),
        ]);
        $this->travelTo('2026-10-07 08:30:00');

        $this->postJson('/interlock/machine-status', [
            'line' => 'Line 1',
            'status' => 'Running',
        ])->assertOk();

        $this->assertDatabaseHas('machine_statuses', [
            'line' => 'Line 1',
            'running_since' => '2026-10-07 08:30:00',
        ]);

        $this->travelTo('2026-10-07 08:45:00');
        $this->postJson('/interlock/machine-status', [
            'line' => 'Line 1',
            'status' => 'Running',
        ])->assertOk();

        $this->get(route('dashboard'))
            ->assertOk()
            ->assertSee('08:30');

        $this->postJson('/interlock/machine-status', [
            'line' => 'Line 1',
            'status' => 'Locked',
        ])->assertOk();

        $this->assertDatabaseHas('machine_statuses', [
            'line' => 'Line 1',
            'status' => 'Locked',
            'running_since' => null,
        ]);
    }

    public function test_failed_node_red_response_does_not_update_machine_status(): void
    {
        Http::preventStrayRequests();
        Http::fake([
            '127.0.0.1:1880/api/interlock/material' => Http::response([], 502),
        ]);

        MachineStatus::create([
            'line' => 'Line 1',
            'status' => 'Locked',
        ]);

        $this->postJson('/interlock/machine-status', [
            'line' => 'Line 1',
            'status' => 'Running',
        ])->assertStatus(502);

        $this->assertDatabaseHas('machine_statuses', [
            'line' => 'Line 1',
            'status' => 'Locked',
        ]);
    }

    public function test_master_data_edit_route_loads_and_updates_record(): void
    {
        $record = MasterData::create([
            'model_name' => 'Laptop Dell',
            'item_name' => 'Battery 6 Cell',
        ]);

        $editResponse = $this->get('/master-data/'.$record->id.'/edit');
        $editResponse->assertOk()
            ->assertSee('Edit Master Data')
            ->assertSee('Laptop Dell');

        $updateResponse = $this->put('/master-data/'.$record->id, [
            'model_name' => 'Laptop Dell',
            'item_name' => 'Keyboard Mechanical',
        ]);

        $updateResponse->assertRedirect('/master-data');
        $this->assertDatabaseHas('master_data', [
            'id' => $record->id,
            'item_name' => 'Keyboard Mechanical',
        ]);
    }

    public function test_master_data_delete_route_removes_record(): void
    {
        $record = MasterData::create([
            'model_name' => 'Printer Canon',
            'item_name' => 'Toner Cartridge Black',
        ]);

        $deleteResponse = $this->delete('/master-data/'.$record->id);

        $deleteResponse->assertRedirect('/master-data');
        $this->assertDatabaseMissing('master_data', [
            'id' => $record->id,
        ]);
    }
}
