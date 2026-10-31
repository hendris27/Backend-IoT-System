<?php

namespace App\Models;

use Database\Factories\MachineStatusFactory;
use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class MachineStatus extends Model
{
    /** @use HasFactory<MachineStatusFactory> */
    use HasFactory;

    protected $fillable = [
        'line',
        'status',
        'running_since',
    ];

    protected function casts(): array
    {
        return [
            'running_since' => 'datetime',
        ];
    }
}
