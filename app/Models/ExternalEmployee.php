<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class ExternalEmployee extends Model
{
    protected $connection = 'external';

    protected $table = '_users';

    protected $primaryKey = 'ID';

    public $timestamps = false;
}
