<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;
use Illuminate\Support\Collection;

class PackingLocation extends Model
{
    protected $fillable = [
        'name',
        'is_active',
    ];

    protected function casts(): array
    {
        return [
            'is_active' => 'boolean',
        ];
    }

    public static function activeNames(): Collection
    {
        return static::query()
            ->where('is_active', true)
            ->pluck('name')
            ->sortBy(fn (string $name): int => static::numberFromName($name))
            ->values();
    }

    public static function inactiveNames(): Collection
    {
        return static::query()
            ->where('is_active', false)
            ->pluck('name')
            ->sortBy(fn (string $name): int => static::numberFromName($name))
            ->values();
    }

    public static function nextName(): string
    {
        $highestNumber = static::query()
            ->pluck('name')
            ->map(fn (string $name): int => static::numberFromName($name))
            ->filter(fn (int $number): bool => $number !== PHP_INT_MAX)
            ->max() ?? 0;

        return 'WP-'.($highestNumber + 1);
    }

    private static function numberFromName(string $name): int
    {
        return preg_match('/^WP-(\d+)$/', $name, $matches) === 1
            ? (int) $matches[1]
            : PHP_INT_MAX;
    }
}
