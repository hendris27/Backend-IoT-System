<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class PackingProcess extends Model
{
    protected $fillable = [
        'model',
        'no_hu',
        'customer',
        'location',
        'box_id',
        'qty',
        'process_status',
        'time_in',
        'time_out',
        'remarks',
    ];

    protected function casts(): array
    {
        return [
            'qty' => 'integer',
            'time_in' => 'datetime',
            'time_out' => 'datetime',
        ];
    }
}
