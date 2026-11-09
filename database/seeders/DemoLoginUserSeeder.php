<?php

namespace Database\Seeders;

use App\Models\User;
use Illuminate\Database\Seeder;

class DemoLoginUserSeeder extends Seeder
{
    public function run(): void
    {
        User::query()->updateOrCreate(
            ['email' => 'admin@siix.local'],
            [
                'name' => 'admin',
                'password' => '123',
            ],
        );
    }
}
