<?php

namespace Database\Seeders;

use App\Models\MachineStatus;
use Illuminate\Database\Seeder;

class MachineStatusSeeder extends Seeder
{
    /**
     * Run the database seeds.
     */
    public function run(): void
    {
        foreach (range(1, 30) as $lineNumber) {
            MachineStatus::query()->firstOrCreate(
                ['line' => "Line {$lineNumber}"],
                ['status' => 'Locked', 'running_since' => null],
            );
        }
    }
}
