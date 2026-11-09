<?php

namespace App\Console\Commands;

use App\Models\Employee;
use App\Models\ExternalEmployee;
use App\Models\User;
use Illuminate\Console\Command;
use Illuminate\Support\Facades\Hash;

class SyncEmployees extends Command
{
    protected $signature = 'app:sync-employees';

    protected $description = 'Sinkronkan nama, NIK, dan departemen karyawan aktif dari database eksternal';

    public function handle(): int
    {
        $this->info('Memulai sinkronisasi karyawan aktif...');
        $synced = 0;

        foreach (
            ExternalEmployee::query()
                ->select(['user_login', 'display_name', 'Departement', 'Section', 'Photo', 'email', 'employee_status'])
                ->whereIn('employee_status', [1, 2, 3])
                ->whereNotNull('user_login')
                ->where('user_login', '<>', '')
                ->cursor() as $externalEmployee
        ) {
            $nik = trim((string) $externalEmployee->user_login);

            if ($nik === '') {
                continue;
            }

            $email = trim((string) $externalEmployee->email);
            $email = $email !== '' ? $email : "{$nik}@siix-global.com";

            $user = User::query()->where('nik', $nik)->first();

            if (! $user) {
                $user = User::query()->where('email', $email)->first();

                if ($user && filled($user->nik) && $user->nik !== $nik) {
                    $user = null;
                    $email = "{$nik}@siix-global.com";
                }
            }

            $user ??= new User;
            $isNewUser = ! $user->exists;

            $user->fill([
                'nik' => $nik,
                'name' => $externalEmployee->display_name,
                'email' => $email,
            ]);

            if ($isNewUser) {
                $user->password = Hash::make($nik);
            }

            $user->save();

            $employee = Employee::query()->firstOrNew(['nik' => $nik]);
            $employee->fill([
                'user_id' => $user->id,
                'name' => $externalEmployee->display_name,
                'department' => $externalEmployee->Departement,
                'section' => $externalEmployee->Section,
                'photo' => $externalEmployee->Photo,
            ])->save();

            $synced++;
        }

        $this->info("Sinkronisasi selesai: {$synced} karyawan diproses.");

        return self::SUCCESS;
    }
}
