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
        Schema::create('packing_processes', function (Blueprint $table) {
            $table->id();
            $table->string('model', 100);
            $table->string('customer', 100);
            $table->string('location', 100);
            $table->string('box_id', 100);
            $table->unsignedInteger('qty');
            $table->text('remarks')->nullable();
            $table->timestamps();
        });
    }

    /**
     * Reverse the migrations.
     */
    public function down(): void
    {
        Schema::dropIfExists('packing_processes');
    }
};
