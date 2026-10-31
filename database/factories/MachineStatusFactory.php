<?php

namespace Database\Factories;

use App\Models\MachineStatus;
use Illuminate\Database\Eloquent\Factories\Factory;

/**
 * @extends Factory<MachineStatus>
 */
class MachineStatusFactory extends Factory
{
    /**
     * Define the model's default state.
     *
     * @return array<string, mixed>
     */
    public function definition(): array
    {
        return [
            'line' => 'Line '.$this->faker->unique()->numberBetween(1, 30),
            'status' => 'Locked',
            'running_since' => null,
        ];
    }

    public function running(): static
    {
        return $this->state(fn (): array => [
            'status' => 'Running',
            'running_since' => now(),
        ]);
    }
}
