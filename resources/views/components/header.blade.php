@php
    $profileUser = auth()->user();
    $profileName = $profileUser?->employee?->name ?: ($profileUser?->name ?? 'Admin');
    $profilePhoto = trim((string) ($profileUser?->employee?->photo ?? ''));
    $profilePhotoPath = ltrim(str_replace('\\', '/', $profilePhoto), '/');
    $employeePhotoPrefix = 'photos/employee/';

    if (str_starts_with(strtolower($profilePhotoPath), $employeePhotoPrefix)) {
        $profilePhotoPath = substr($profilePhotoPath, strlen($employeePhotoPrefix));
    }

    $profilePhotoPath = implode('/', array_map('rawurlencode', explode('/', $profilePhotoPath)));
    $profilePhotoUrl = $profilePhoto === ''
        ? null
        : (filter_var($profilePhoto, FILTER_VALIDATE_URL)
            ? $profilePhoto
            : 'http://192.168.61.8/photos/employee/' . $profilePhotoPath);
@endphp
<header class="topbar">
    <a class="topbar-brand" href="{{ route('dashboard') }}" aria-label="Backend - SIIX, ke Dashboard">
        <img class="topbar-brand-logo" src="{{ asset('assets/img/logo-siix-be.png') }}" alt="SIIX Backend">
        <img class="topbar-brand-icon" src="{{ asset('assets/img/logo-siix.png') }}" alt="" aria-hidden="true">
    </a>
    <button type="button" class="sidebar-toggle" aria-controls="main-sidebar" aria-expanded="true"
        aria-label="Sembunyikan sidebar">
        <span class="hamburger-icon" aria-hidden="true"></span>
    </button>
    <span class="topbar-spacer" aria-hidden="true"></span>
    <div class="profile-menu">
        <button type="button" class="profile-trigger" id="profile-trigger" aria-controls="profile-dropdown"
            aria-expanded="false" aria-label="Buka menu profil {{ $profileName }}">
            @if ($profilePhotoUrl)
                <img class="profile-avatar profile-avatar-image"
                    src="{{ $profilePhotoUrl }}" alt=""
                    onerror="this.hidden = true; this.nextElementSibling.hidden = false;">
                <span class="profile-avatar" aria-hidden="true" hidden>
                    {{ strtoupper(substr($profileName, 0, 1)) }}
                </span>
            @else
                <span class="profile-avatar" aria-hidden="true">{{ strtoupper(substr($profileName, 0, 1)) }}</span>
            @endif
            <span class="profile-caret" aria-hidden="true">&#9662;</span>
        </button>
        <div class="profile-dropdown" id="profile-dropdown" hidden>
            <p>Signed in as</p>
            <strong>{{ $profileName }}</strong>
            <a class="profile-home" href="{{ route('dashboard') }}">Open Dashboard</a>
            <form class="profile-logout-form" method="POST" action="{{ route('logout') }}">
                @csrf
                <button class="profile-logout" type="submit">Sign out</button>
            </form>
        </div>
    </div>
</header>
