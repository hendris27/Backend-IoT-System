<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    public function up(): void
    {
        Schema::create('packing_locations', function (Blueprint $table): void {
            $table->id();
            $table->string('name', 50)->unique();
            $table->boolean('is_active')->default(true);
            $table->timestamps();
        });

        $timestamp = now();

        DB::table('packing_locations')->insert(
            collect(range(1, 7))->map(fn (int $number): array => [
                'name' => "WP-{$number}",
                'is_active' => true,
                'created_at' => $timestamp,
                'updated_at' => $timestamp,
            ])->all(),
        );
    }

    public function down(): void
    {
        Schema::dropIfExists('packing_locations');
    }
};
