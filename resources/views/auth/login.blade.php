<!DOCTYPE html>
<html lang="id">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="csrf-token" content="{{ csrf_token() }}">
    <title>Sign In | SIIX - Backend</title>
    @vite('resources/css/auth.css')
</head>

<body class="login-page">
    <main class="login-card" aria-labelledby="login-title">
        <div class="login-card-content">
            <img class="login-logo" src="{{ asset('assets/img/logo-siix-be.png') }}" alt="SIIX - Backend">

            <h1 id="login-title" class="visually-hidden">Sign in ke SIIX - Backend</h1>

            <form class="login-form" method="POST" action="{{ route('login.store') }}">
                @csrf

                <label class="login-input-row" for="username">
                    <svg viewBox="0 0 24 24" aria-hidden="true">
                        <circle cx="12" cy="8" r="3.3"></circle>
                        <path d="M5.5 20c.3-3.3 2.7-5.2 6.5-5.2s6.2 1.9 6.5 5.2"></path>
                    </svg>
                    <input id="username" name="username" type="text" value="{{ old('username') }}"
                        placeholder="NIK / Username" autocomplete="username" required autofocus>
                </label>

                <label class="login-input-row" for="password">
                    <svg viewBox="0 0 24 24" aria-hidden="true">
                        <rect x="6.5" y="10" width="11" height="10.5" rx="1.5"></rect>
                        <path d="M8.8 10V7.2a3.2 3.2 0 0 1 6.4 0V10"></path>
                    </svg>
                    <input id="password" name="password" type="password" placeholder="Password"
                        autocomplete="current-password" required>
                    <button class="login-password-toggle" type="button" data-toggle-password
                        aria-label="Tampilkan password" aria-pressed="false">
                        <svg viewBox="0 0 24 24" aria-hidden="true">
                            <path d="M2.5 12s3.2-5.4 9.5-5.4 9.5 5.4 9.5 5.4-3.2 5.4-9.5 5.4S2.5 12 2.5 12Z"></path>
                            <circle cx="12" cy="12" r="2.5"></circle>
                        </svg>
                    </button>
                </label>

                @if ($errors->any())
                    <p class="login-error" role="alert">{{ $errors->first() }}</p>
                @endif

                <button class="login-submit" type="submit">
                    <svg viewBox="0 0 24 24" aria-hidden="true">
                        <path d="M13.5 5h4A1.5 1.5 0 0 1 19 6.5v11a1.5 1.5 0 0 1-1.5 1.5h-4"></path>
                        <path d="M4 12h10m-3.5-3.5L14 12l-3.5 3.5"></path>
                    </svg>
                    <span>Sign in</span>
                </button>
            </form>

            <footer class="login-footer">
                <span>Copyright © 2026 SIIX Access</span>
                <span>V1.0</span>
            </footer>
        </div>
    </main>

    <script>
        document.querySelector('[data-toggle-password]')?.addEventListener('click', event => {
            const button = event.currentTarget;
            const password = document.querySelector('#password');
            const isVisible = password.type === 'text';

            password.type = isVisible ? 'password' : 'text';
            button.setAttribute('aria-pressed', String(!isVisible));
            button.setAttribute('aria-label', isVisible ? 'Tampilkan password' : 'Sembunyikan password');
        });
    </script>
</body>

</html>
