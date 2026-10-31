<?php

use App\Http\Controllers\DashboardController;
use App\Http\Controllers\MasterDataController;
use App\Http\Controllers\PackingProcessController;
use App\Models\MachineStatus;
use App\Models\MasterData;
use Illuminate\Http\Client\ConnectionException;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Route;
use Illuminate\Validation\Rule;

Route::get('/', function () {
    return to_route('machine');
})->name('home');

Route::get('/api/model-names', function () {
    $modelNames = MasterData::query()
        ->select('model_name')
        ->distinct()
        ->orderBy('model_name')
        ->pluck('model_name');

    return response()->json($modelNames);
});

Route::get('/api/model-items/{modelName}', function ($modelName) {
    $items = MasterData::where('model_name', $modelName)->get();

    return response()->json($items);
});

Route::post('/change-model/verify-pin', function (Request $request) {
    $validated = $request->validate([
        'pin' => ['required', 'string', 'digits:4'],
    ]);

    $expectedPin = config('services.interlock.model_change_pin');

    if (! is_string($expectedPin) || $expectedPin === '') {
        return response()->json([
            'success' => false,
            'message' => 'PIN Change Model belum dikonfigurasi.',
        ], 503);
    }

    if (! hash_equals($expectedPin, $validated['pin'])) {
        return response()->json([
            'success' => false,
            'message' => 'PIN tidak sesuai.',
        ], 422);
    }

    return response()->json(['success' => true]);
})->middleware('throttle:5,1')->name('change-model.verify-pin');

Route::get('/dashboard', [DashboardController::class, 'index'])->name('dashboard');
Route::prefix('packing')->name('packing.')->group(function () {
    Route::get('/', [PackingProcessController::class, 'dashboard'])->name('index');
    Route::post('/processes', [PackingProcessController::class, 'storeFromDashboard'])->name('processes.store');
    Route::get('/processes/scan-in', [PackingProcessController::class, 'scanForIn'])->name('processes.scan-in');
    Route::get('/processes/scan', [PackingProcessController::class, 'scanForOut'])->name('processes.scan');
    Route::post('/processes/complete-out', [PackingProcessController::class, 'completeOut'])->name('processes.complete-out');
    Route::patch('/processes/{packingProcess}', [PackingProcessController::class, 'updateFromDashboard'])
        ->name('processes.update');
    Route::post('/processes/{packingProcess}/out', [PackingProcessController::class, 'markOut'])
        ->name('processes.out');
    Route::view('/history', 'packing.history')->name('history');
    Route::view('/report', 'packing.report')->name('report');
});

Route::get('/machine', function () {
    $modelNames = MasterData::query()
        ->select('model_name')
        ->distinct()
        ->orderBy('model_name')
        ->pluck('model_name');

    return view('interlock.machine.index', compact('modelNames'));
})->name('machine');
Route::resource('master-data', MasterDataController::class);
Route::post('/interlock/machine-status', function (Request $request) {
    $validated = $request->validate([
        'status' => ['required', 'string', Rule::in(['Locked', 'Running'])],
        'line' => ['nullable', 'string', 'regex:/^Line ([1-9]|[12][0-9]|30)$/'],
    ]);

    $status = $validated['status'];

    $mqttCommand = $status === 'Running' ? 'ON' : 'OFF';

    try {
        $response = Http::timeout(5)->post(config('services.node_red.interlock_url'), [
            'status' => $status,
            'command' => $mqttCommand,
        ]);
    } catch (ConnectionException) {
        return response()->json([
            'success' => false,
            'message' => 'Node-RED tidak dapat dihubungi.',
            'status' => $status,
            'command' => $mqttCommand,
        ], 503);
    }

    if ($response->successful() && isset($validated['line'])) {
        $machineStatus = MachineStatus::query()->firstOrNew(['line' => $validated['line']]);
        $runningSince = $status === 'Running'
            ? ($machineStatus->status === 'Running' && $machineStatus->running_since !== null
                ? $machineStatus->running_since
                : now())
            : null;

        $machineStatus->fill([
            'status' => $status,
            'running_since' => $runningSince,
        ])->save();
    }

    return response()->json([
        'success' => $response->successful(),
        'status' => $status,
        'command' => $mqttCommand,
        'nodered_status' => $response->status(),
        'nodered_body' => $response->body(),
    ], $response->successful() ? 200 : 502);
});
