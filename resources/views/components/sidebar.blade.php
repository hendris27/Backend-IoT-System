<!DOCTYPE html>
<html lang="id">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="csrf-token" content="{{ csrf_token() }}">
    <title>{{ $title ?? 'Dashboard' }} </title>
    @vite('resources/css/sidebar.css')
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.all.min.js"></script>
</head>

<body>
    <div class="page">
        @include('components.header')
        <aside class="sidebar" id="main-sidebar" aria-label="Navigasi utama">
            <nav>
                <ul class="nav-list">
                    @php($isInterlockActive = request()->is('/') || request()->routeIs('dashboard', 'machine', 'master-data.*'))
                    <li class="nav-group {{ $isInterlockActive ? 'is-open' : '' }}">
                        <button type="button" class="interlock-menu-card {{ $isInterlockActive ? 'active' : '' }}"
                            aria-expanded="{{ $isInterlockActive ? 'true' : 'false' }}">
                            <span class="nav-title">Interlock</span>
                            <span class="nav-caret" aria-hidden="true">&#9662;</span>
                        </button>
                        <ul class="nav-submenu">
                            <li>
                                <a class="nav-link {{ request()->routeIs('dashboard') ? 'active' : '' }}"
                                    aria-current="{{ request()->routeIs('dashboard') ? 'page' : 'false' }}"
                                    href="{{ route('dashboard') }}">
                                    <span class="status-dot"></span>
                                    Dashboard
                                </a>
                            </li>
                            <li>
                                <a class="nav-link {{ request()->routeIs('machine') ? 'active' : '' }}"
                                    aria-current="{{ request()->routeIs('machine') ? 'page' : 'false' }}"
                                    href="{{ route('machine') }}">
                                    <span class="status-dot"></span>
                                    Machine
                                </a>
                            </li>
                            <li>
                                <a class="nav-link {{ request()->routeIs('master-data.*') ? 'active' : '' }}"
                                    aria-current="{{ request()->routeIs('master-data.*') ? 'page' : 'false' }}"
                                    href="{{ route('master-data.index') }}">
                                    <span class="status-dot"></span>
                                    Master Data
                                </a>
                            </li>
                        </ul>
                    </li>
                    @php($isPackingActive = request()->routeIs('packing.*'))
                    <li class="nav-group {{ $isPackingActive ? 'is-open' : '' }}">
                        <button type="button" class="interlock-menu-card {{ $isPackingActive ? 'active' : '' }}"
                            aria-expanded="{{ $isPackingActive ? 'true' : 'false' }}">
                            <span class="nav-title">Packing</span>
                            <span class="nav-caret" aria-hidden="true">&#9662;</span>
                        </button>
                        <ul class="nav-submenu">
                            <li>
                                <a class="nav-link {{ request()->routeIs('packing.index') ? 'active' : '' }}"
                                    aria-current="{{ request()->routeIs('packing.index') ? 'page' : 'false' }}"
                                    href="{{ route('packing.index') }}">
                                    <span class="status-dot"></span>
                                    Dashboard
                                </a>
                            </li>
                            <li>
                                <a class="nav-link {{ request()->routeIs('packing.history') ? 'active' : '' }}"
                                    aria-current="{{ request()->routeIs('packing.history') ? 'page' : 'false' }}"
                                    href="{{ route('packing.history') }}">
                                    <span class="status-dot"></span>
                                    Histori
                                </a>
                            </li>
                            <li>
                                <a class="nav-link {{ request()->routeIs('packing.report') ? 'active' : '' }}"
                                    aria-current="{{ request()->routeIs('packing.report') ? 'page' : 'false' }}"
                                    href="{{ route('packing.report') }}">
                                    <span class="status-dot"></span>
                                    Report
                                </a>
                            </li>
                        </ul>
                    </li>

                </ul>
            </nav>
        </aside>
        <main>
            <div class="content">
                <div class="page-heading">
                    <div>
                        <h1>{{ $title ?? 'Dashboard' }}</h1>
                        <nav class="breadcrumbs" aria-label="Breadcrumb">
                            <a href="{{ route('dashboard') }}">Home</a>
                            <span aria-hidden="true">/</span>
                            <span aria-current="page">{{ $title ?? 'Dashboard' }}</span>
                        </nav>
                    </div>
                </div>
                @if ($errors->any())
                    <div class="errors" role="alert">
                        @foreach ($errors->all() as $error)
                            <p>{{ $error }}</p>
                        @endforeach
                    </div>
                @endif

                {{ $slot }}
            </div>
            @include('components.footer')
        </main>
    </div>
    <script>
        const appPage = document.querySelector('.page');
        const sidebarButton = document.querySelector('.sidebar-toggle');
        const topbar = document.querySelector('.topbar');
        const contentMain = document.querySelector('main');
        const profileButton = document.getElementById('profile-trigger');
        const profileDropdown = document.getElementById('profile-dropdown');
        document.querySelectorAll('.nav-group').forEach(navGroup => {
            const navToggle = navGroup.querySelector('.interlock-menu-card');

            if (navToggle) {
                navToggle.addEventListener('click', () => {
                    const isOpen = navGroup.classList.toggle('is-open');
                    navToggle.setAttribute('aria-expanded', String(isOpen));
                });
            }
        });

        function setSidebarHidden(isHidden) {
            appPage.classList.toggle('sidebar-hidden', isHidden);
            topbar.classList.toggle('sidebar-hidden', isHidden);
            sidebarButton.setAttribute('aria-expanded', String(!isHidden));
            sidebarButton.setAttribute('aria-label', isHidden ? 'Tampilkan sidebar' : 'Sembunyikan sidebar');
        }

        if (appPage && sidebarButton) {
            if (window.matchMedia('(max-width: 700px)').matches) {
                setSidebarHidden(true);
            }

            sidebarButton.addEventListener('click', () => {
                setSidebarHidden(!appPage.classList.contains('sidebar-hidden'));
            });
        }

        if (profileButton && profileDropdown) {
            profileButton.addEventListener('click', () => {
                const isExpanded = profileButton.getAttribute('aria-expanded') === 'true';
                profileButton.setAttribute('aria-expanded', String(!isExpanded));
                profileDropdown.hidden = isExpanded;
            });

            document.addEventListener('click', event => {
                if (!event.target.closest('.profile-menu')) {
                    profileButton.setAttribute('aria-expanded', 'false');
                    profileDropdown.hidden = true;
                }
            });

            document.addEventListener('keydown', event => {
                if (event.key === 'Escape') {
                    profileButton.setAttribute('aria-expanded', 'false');
                    profileDropdown.hidden = true;
                }
            });
        }

        if (contentMain && appPage && sidebarButton) {
            contentMain.addEventListener('click', () => {
                if (window.matchMedia('(max-width: 700px)').matches &&
                    !appPage.classList.contains('sidebar-hidden')) {
                    setSidebarHidden(true);
                }
            });
        }

        // SweetAlert untuk Success Message
        @if (session('success'))
            Swal.fire({
                icon: 'success',
                title: 'Berhasil!',
                text: '{{ session('success') }}',
                confirmButtonColor: '#1677d2',
                confirmButtonText: 'OK'
            });
        @endif

        // SweetAlert untuk Error Message
        @if ($errors->any())
            Swal.fire({
                icon: 'error',
                title: 'Terjadi Kesalahan!',
                html: `{!! implode('<br>', $errors->all()) !!}`,
                confirmButtonColor: '#1677d2',
                confirmButtonText: 'OK'
            });
        @endif
    </script>
</body>

</html>
