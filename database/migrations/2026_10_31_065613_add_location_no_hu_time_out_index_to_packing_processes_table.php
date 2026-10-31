<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    /**
     * Run the migrations.
     */
    public function up(): void
    {
        Schema::table('packing_processes', function (Blueprint $table) {
            $table->index(['location', 'no_hu', 'time_out'], 'packing_processes_out_scan_index');
        });
    }

    /**
     * Reverse the migrations.
     */
    public function down(): void
    {
        Schema::table('packing_processes', function (Blueprint $table) {
            $table->dropIndex('packing_processes_out_scan_index');
        });
    }
};
