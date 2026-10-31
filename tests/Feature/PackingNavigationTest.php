<?php

namespace Tests\Feature;

use Illuminate\Foundation\Testing\RefreshDatabase;
use Tests\TestCase;

class PackingNavigationTest extends TestCase
{
    use RefreshDatabase;

    public function test_packing_dashboard_uses_index_view_and_links_to_subpages(): void
    {
        $this->get(route('packing.index'))
            ->assertOk()
            ->assertSee('Packing Dashboard')
            ->assertSee('Waiting (QTY)')
            ->assertSee('WP-1')
            ->assertSee('WP-7')
            ->assertSee('History Process')
            ->assertSee('href="'.route('packing.history').'"', false)
            ->assertSee('href="'.route('packing.report').'"', false)
            ->assertSee('aria-current="page"', false);
    }

    public function test_packing_history_page_renders(): void
    {
        $this->get(route('packing.history'))
            ->assertOk()
            ->assertSee('Histori Packing');
    }

    public function test_packing_report_page_renders(): void
    {
        $this->get(route('packing.report'))
            ->assertOk()
            ->assertSee('Report Packing');
    }
}
