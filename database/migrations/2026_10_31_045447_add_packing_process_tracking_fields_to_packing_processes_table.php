<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    /**
     * Run the migrations.
     */
    public function up(): void
    {
        Schema::table('packing_processes', function (Blueprint $table) {
            $table->string('process_status', 3)->default('IN')->after('qty');
            $table->timestamp('time_in')->nullable()->after('process_status');
            $table->timestamp('time_out')->nullable()->after('time_in');
        });

        DB::table('packing_processes')
            ->whereNull('time_in')
            ->update(['time_in' => DB::raw('created_at')]);
    }

    /**
     * Reverse the migrations.
     */
    public function down(): void
    {
        Schema::table('packing_processes', function (Blueprint $table) {
            $table->dropColumn(['process_status', 'time_in', 'time_out']);
        });
    }
};
