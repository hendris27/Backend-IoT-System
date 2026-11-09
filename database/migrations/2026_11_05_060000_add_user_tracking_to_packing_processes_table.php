<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    public function up(): void
    {
        Schema::table('packing_processes', function (Blueprint $table): void {
            $table->foreignId('in_user_id')
                ->nullable()
                ->after('time_in')
                ->constrained('users')
                ->nullOnDelete();
            $table->foreignId('out_user_id')
                ->nullable()
                ->after('time_out')
                ->constrained('users')
                ->nullOnDelete();
        });
    }

    public function down(): void
    {
        Schema::table('packing_processes', function (Blueprint $table): void {
            $table->dropConstrainedForeignId('in_user_id');
            $table->dropConstrainedForeignId('out_user_id');
        });
    }
};
