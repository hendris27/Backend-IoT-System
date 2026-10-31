<?php

namespace Database\Seeders;

use Illuminate\Database\Seeder;
use Illuminate\Support\Facades\DB;

class CustomerSeeder extends Seeder
{
    /**
     * Run the database seeds.
     */
    public function run(): void
    {
        $timestamp = now();
        $customers = [
            ['customer_code' => '1123', 'curr' => null, 'customer_name' => 'IC', 'country_key' => 'ICHIKOH'],
            ['customer_code' => '1153', 'curr' => null, 'customer_name' => 'KT', 'country_key' => 'Koito Manufacturing'],
            ['customer_code' => '1187', 'curr' => null, 'customer_name' => 'MT', 'country_key' => 'Mitsuba'],
            ['customer_code' => '1359', 'curr' => null, 'customer_name' => 'TD', 'country_key' => 'Toyodenso'],
            ['customer_code' => '1019', 'curr' => null, 'customer_name' => 'AV', 'country_key' => 'AVI Automotive'],
            ['customer_code' => '1112', 'curr' => null, 'customer_name' => 'MS', 'country_key' => 'MAS-I Corporation'],
            ['customer_code' => '1156', 'curr' => null, 'customer_name' => 'KO', 'country_key' => 'Kojima Industries'],
            ['customer_code' => '1405', 'curr' => null, 'customer_name' => 'HN', 'country_key' => 'Hino Motors'],
            ['customer_code' => '1147', 'curr' => null, 'customer_name' => 'KA', 'country_key' => 'Kawai Industries'],
            ['customer_code' => '1018', 'curr' => null, 'customer_name' => 'AJ', 'country_key' => 'AJI Manufacturing'],
            ['customer_code' => '1011', 'curr' => null, 'customer_name' => 'AP', 'country_key' => 'Ampassindo Pratama'],
            ['customer_code' => '1610', 'curr' => null, 'customer_name' => 'GW', 'country_key' => 'Green Way Co.'],
        ];

        $customers = array_map(
            fn (array $customer): array => [...$customer, 'created_at' => $timestamp, 'updated_at' => $timestamp],
            $customers,
        );

        DB::table('customers')->upsert(
            $customers,
            ['customer_code'],
            ['curr', 'customer_name', 'country_key', 'updated_at'],
        );
    }
}
