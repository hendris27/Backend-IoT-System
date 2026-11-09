<?php

namespace App\Http\Controllers\Auth;

use App\Http\Controllers\Controller;
use Illuminate\Http\RedirectResponse;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use Illuminate\View\View;

class AuthenticatedSessionController extends Controller
{
    public function create(): View
    {
        return view('auth.login');
    }

    public function store(Request $request): RedirectResponse
    {
        $credentials = $request->validate([
            'username' => ['required', 'string', 'max:255'],
            'password' => ['required', 'string', 'max:255'],
        ]);

        $loginIdentifier = $credentials['username'];
        $password = $credentials['password'];

        $authenticated = filter_var($loginIdentifier, FILTER_VALIDATE_EMAIL)
            ? Auth::attempt(['email' => $loginIdentifier, 'password' => $password])
            : Auth::attempt(['nik' => $loginIdentifier, 'password' => $password]);

        if (! $authenticated && ! filter_var($loginIdentifier, FILTER_VALIDATE_EMAIL)) {
            $authenticated = Auth::attempt(['name' => $loginIdentifier, 'password' => $password]);
        }

        if (! $authenticated) {
            return back()
                ->withErrors(['username' => 'Username atau password tidak sesuai.'])
                ->onlyInput('username');
        }

        $request->session()->regenerate();

        return redirect()->intended(route('machine'));
    }

    public function destroy(Request $request): RedirectResponse
    {
        Auth::logout();

        $request->session()->invalidate();
        $request->session()->regenerateToken();

        return to_route('login');
    }
}
