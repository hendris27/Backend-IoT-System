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
        if (Schema::hasTable('machine_statuses')) {
            return;
        }

        Schema::create('machine_statuses', function (Blueprint $table) {
            $table->id();
            $table->string('line', 16)->unique();
            $table->string('status', 32)->default('Locked');
            $table->timestamp('running_since')->nullable();
            $table->timestamps();
        });
    }

    /**
     * Reverse the migrations.
     */
    public function down(): void
    {
        Schema::dropIfExists('machine_statuses');
    }
};
