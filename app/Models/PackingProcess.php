<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;

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
        'in_user_id',
        'time_out',
        'out_user_id',
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

    public function inUser(): BelongsTo
    {
        return $this->belongsTo(User::class, 'in_user_id');
    }

    public function outUser(): BelongsTo
    {
        return $this->belongsTo(User::class, 'out_user_id');
    }
}
