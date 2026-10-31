@php($profileName = auth()->user()?->name ?? 'Admin')
<header class="topbar">
    <a class="topbar-brand" href="{{ route('dashboard') }}" aria-label="Backend - SIIX, ke Dashboard">
        <span class="topbar-brand-label">Backend - SIIX</span>
        <img class="topbar-brand-logo" src="{{ asset('assets/img/logo-siix.png') }}" alt="SIIX">
    </a>
    <button type="button" class="sidebar-toggle" aria-controls="main-sidebar" aria-expanded="true"
        aria-label="Sembunyikan sidebar">
        <span class="hamburger-icon" aria-hidden="true"></span>
    </button>
    <span class="topbar-spacer" aria-hidden="true"></span>
    <div class="profile-menu">
        <button type="button" class="profile-trigger" id="profile-trigger" aria-controls="profile-dropdown"
            aria-expanded="false">
            <span class="profile-avatar" aria-hidden="true">{{ strtoupper(substr($profileName, 0, 1)) }}</span>
            <span class="profile-name">{{ $profileName }}</span>
            <span class="profile-caret" aria-hidden="true">&#9662;</span>
        </button>
        <div class="profile-dropdown" id="profile-dropdown" hidden>
            <p>Signed in as</p>
            <strong>{{ $profileName }}</strong>
            <a class="profile-home" href="{{ route('dashboard') }}">Open Dashboard</a>
        </div>
    </div>
</header>
