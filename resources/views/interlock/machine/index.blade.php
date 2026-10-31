<x-sidebar title="Machine">
    <style>
        html,
        body {
            overflow-x: hidden;
        }

        form {
            min-height: 0;
            padding: 0 0 .5rem;
        }

        .controls {
            display: grid;
            grid-template-columns: minmax(180px, .72fr) minmax(260px, 1.2fr) minmax(190px, .72fr);
            gap: 1rem;
            max-width: 1100px;
            margin: 0 auto;
        }

        .field-group {
            display: grid;
            gap: .45rem;
            min-width: 0;
        }

        .field-label {
            color: #d4e7fa;
            font-size: .68rem;
            font-weight: 700;
            letter-spacing: .08em;
            text-transform: uppercase;
        }

        .control {
            height: 56px;
            border: 1px solid rgba(255, 255, 255, .45);
            border-radius: 8px;
            background: #fff;
            color: var(--ink);
            font: .82rem Arial, sans-serif;
            box-shadow: 0 8px 22px rgba(3, 25, 56, .13);
            transition: border-color .2s ease, box-shadow .2s ease;
        }

        .control:focus-within,
        .control:focus {
            border-color: var(--blue);
            outline: 3px solid rgba(22, 119, 210, .14);
            outline-offset: 1px;
        }

        .model-picker {
            position: relative;
            display: flex;
            align-items: stretch;
            height: auto;
            min-height: 56px;
            padding: 0;
            overflow: visible;
        }

        .model-input-wrap {
            display: flex;
            align-items: center;
            width: 100%;
            min-height: 56px;
            border-radius: 8px;
            overflow: hidden;
        }

        .change-model {
            flex: 0 0 auto;
            height: 56px;
            padding: 0 .9rem;
            border: 0;
            border-left: 1px solid rgba(16, 36, 61, .12);
            background: #eef5fb;
            color: #135b91;
            font: 700 .7rem Arial, sans-serif;
            cursor: pointer;
        }

        .change-model:hover {
            background: #dcecf8;
        }

        .pin-dialog {
            width: min(92vw, 330px);
            max-height: calc(100dvh - 2rem);
            overflow-y: auto;
            padding: .85rem;
            border: 1px solid #d7e2ee;
            border-radius: 8px;
            color: #10243d;
            box-shadow: 0 18px 50px rgba(3, 25, 56, .24);
        }

        .pin-dialog::backdrop {
            background: rgba(3, 19, 38, .62);
            backdrop-filter: blur(3px);
        }

        .pin-dialog form {
            min-height: 0;
            max-height: none;
            padding: 0;
            overflow: visible;
        }

        .pin-dialog h2 {
            margin: 0 0 .3rem;
            font-size: .92rem;
        }

        .pin-dialog .field-label {
            color: #52657b;
        }

        .pin-dialog p {
            margin: 0 0 .55rem;
            color: #5e6e80;
            font-size: .7rem;
            line-height: 1.35;
        }

        .pin-dialog input {
            width: 100%;
            height: 38px;
            padding: 0 .65rem;
            border: 1px solid #b7c8d9;
            border-radius: 5px;
            color: #10243d;
            font: 1rem Arial, sans-serif;
            letter-spacing: .2em;
        }

        .pin-dialog input:focus {
            border-color: #1677d2;
            outline: 3px solid rgba(22, 119, 210, .15);
        }

        .pin-keypad {
            display: grid;
            grid-template-columns: repeat(3, minmax(0, 1fr));
            gap: .3rem;
            margin-top: .45rem;
        }

        .pin-keypad button {
            min-height: 34px;
            border: 1px solid #d3deea;
            border-radius: 5px;
            background: #f7fafd;
            color: #24384f;
            font: 700 .9rem Arial, sans-serif;
            cursor: pointer;
        }

        .pin-keypad button:hover {
            border-color: #9eb9d4;
            background: #edf5fc;
        }

        .pin-keypad .pin-key-action {
            font-size: .7rem;
        }

        .pin-feedback {
            min-height: 1.1rem;
            margin: .55rem 0 0 !important;
            color: #a22d35 !important;
        }

        .pin-dialog-actions {
            display: flex;
            justify-content: flex-end;
            gap: .4rem;
            margin-top: .5rem;
        }

        .pin-dialog-actions button {
            min-height: 34px;
            padding: 0 .7rem;
            border: 1px solid #c5d2df;
            border-radius: 5px;
            background: #fff;
            color: #34465c;
            font: 700 .72rem Arial, sans-serif;
            cursor: pointer;
        }

        .pin-dialog-actions button[type="submit"] {
            border-color: #1677d2;
            background: #1677d2;
            color: #fff;
        }

        .model-search-input {
            flex: 1;
            min-width: 0;
            width: 100%;
            height: 56px;
            padding: 0 1rem;
            border: 0;
            outline: 0;
            background: #fff;
            color: var(--ink);
            text-align: left;
            font: inherit;
        }

        .search-btn {
            width: 110px;
            height: 56px;
            border: 0;
            border-left: 1px solid rgba(16, 36, 61, .12);
            background: var(--blue);
            color: #fff;
            font: 700 .76rem Arial, sans-serif;
            cursor: pointer;
            transition: background .2s ease, filter .2s ease;
        }

        .search-btn:hover {
            background: #0d5cb4;
            filter: brightness(1.02);
        }

        .model-dropdown {
            position: absolute;
            z-index: 35;
            top: calc(100% + 8px);
            left: 0;
            right: 0;
            display: none;
            max-height: 260px;
            overflow-y: auto;
            padding: .35rem;
            background: rgba(9, 31, 58, .96);
            border: 1px solid rgba(255, 255, 255, .18);
            border-radius: 10px;
            box-shadow: 0 14px 30px rgba(1, 16, 39, .28);
        }

        .model-dropdown.visible {
            display: block;
        }

        .model-option {
            display: block;
            width: 100%;
            padding: .7rem .8rem;
            border: 0;
            border-radius: 6px;
            background: transparent;
            color: #edf7ff;
            text-align: left;
            font: .78rem Arial, sans-serif;
            cursor: pointer;
        }

        .model-option:hover,
        .model-option:focus-visible {
            background: rgba(255, 255, 255, .08);
            outline: none;
        }

        .line-select {
            position: relative;
            display: flex;
            align-items: center;
            justify-content: center;
            overflow: hidden;
        }

        .line-select select {
            width: 100%;
            height: 100%;
            padding: 0 3.5rem 0 1rem;
            border: 0;
            outline: 0;
            appearance: none;
            background: transparent;
            color: inherit;
            text-align: center;
            font: inherit;
            cursor: pointer;
        }

        .line-search {
            width: 100%;
            height: 100%;
            padding: 0 3.5rem 0 1rem;
            border: 0;
            outline: 0;
            background: transparent;
            color: inherit;
            text-align: center;
            font: inherit;
        }

        .line-select::after {
            content: '';
            position: absolute;
            right: 1.3rem;
            top: 50%;
            width: 8px;
            height: 8px;
            border-right: 2px solid var(--blue);
            border-bottom: 2px solid var(--blue);
            transform: translateY(-70%) rotate(45deg);
            pointer-events: none;
        }

        .search {
            width: 100%;
            padding: 0 1.25rem;
            text-align: center;
            background: #fff;
            color: var(--ink);
        }

        .search::placeholder,
        .scan-input::placeholder {
            color: #8493a3;
            opacity: 1;
        }

        .search::-webkit-calendar-picker-indicator {
            opacity: 0.8;
            cursor: pointer;
            filter: grayscale(1) brightness(0.5);
        }

        .scan-area {
            width: min(100%, 820px);
            margin: 1.7rem auto 0;
        }

        .scan-progress,
        .scan-input {
            width: 100%;
            height: 58px;
            border: 1px solid rgba(255, 255, 255, .45);
            border-radius: 9px;
            background: #fff;
            text-align: center;
            font: .82rem Arial, sans-serif;
            box-shadow: 0 8px 22px rgba(3, 25, 56, .13);
        }

        .scan-progress {
            position: relative;
            display: grid;
            place-items: center;
            height: 46px;
            margin-bottom: .65rem;
            color: var(--ink);
            font-weight: 700;
        }

        .scan-progress::after {
            content: '';
            position: absolute;
            right: 1.2rem;
            bottom: .8rem;
            left: 1.2rem;
            height: 3px;
            overflow: hidden;
            background: #dbeaf7;
            border-radius: 4px;
        }

        .scan-progress::before {
            content: '';
            position: absolute;
            z-index: 1;
            right: calc(100% - 1.2rem - var(--progress-percent, 0%));
            bottom: .8rem;
            left: 1.2rem;
            height: 3px;
            background: var(--blue);
            border-radius: 4px;
            transition: right .25s ease;
        }

        .scan-input {
            padding: 0 1.25rem;
            transition: border-color .2s ease, box-shadow .2s ease;
        }

        .scan-input:focus,
        .search:focus {
            border-color: var(--blue);
            outline: 3px solid rgba(22, 119, 210, .14);
            outline-offset: 1px;
        }

        .sparepart-list {
            margin: .9rem 0 0;
            overflow: hidden;
            border: 1px solid rgba(255, 255, 255, .2);
            border-radius: 8px;
            background: rgba(5, 28, 56, .35);
        }

        .sparepart-list h2 {
            margin: 0;
            padding: .8rem 1rem;
            color: #eaf5ff;
            font-size: .76rem;
            font-weight: 700;
        }

        .sparepart-list table {
            width: 100%;
            border-collapse: collapse;
        }

        .sparepart-list th,
        .sparepart-list td {
            padding: .7rem 1rem;
            border-top: 1px solid rgba(255, 255, 255, .1);
            text-align: left;
            font-size: .76rem;
        }

        .sparepart-list th {
            color: #d4e7fa;
            background: rgba(255, 255, 255, .06);
        }

        .scan-help {
            margin: .6rem 0 0;
            color: var(--muted);
            font-size: .7rem;
            text-align: center;
        }

        .submit {
            display: block;
            width: 160px;
            height: 44px;
            margin: 1.3rem auto 0;
            border: 0;
            border-radius: 7px;
            color: #fff;
            background: var(--blue);
            cursor: pointer;
            font: 700 .78rem Arial, sans-serif;
            box-shadow: 0 6px 16px rgba(2, 34, 78, .24);
            transition: transform .2s ease, background .2s ease, box-shadow .2s ease;
        }

        .submit:hover {
            background: #16915c;
            box-shadow: 0 6px 14px rgba(24, 121, 78, .3);
            transform: translateY(-1px);
        }

        .notice,
        .errors {
            width: min(100%, 390px);
            margin: 1rem auto;
            padding: .8rem 1rem;
            border: 1px solid;
            border-radius: 7px;
            font-size: .74rem;
            line-height: 1.4;
            text-align: center;
        }

        .notice {
            color: var(--green);
            background: var(--green-soft);
            border-color: rgba(168, 222, 193, .85);
        }

        .errors {
            color: #a22d35;
            background: #fff1f2;
            border-color: #e7b7bb;
        }

        .machine {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: .75rem;
            min-height: 56px;
            padding: .55rem .8rem;
            border: 1px solid rgba(255, 255, 255, .22);
            border-radius: 8px;
            background: rgba(255, 255, 255, .09);
            font-size: .74rem;
            font-weight: 700;
        }

        .machine strong {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            gap: .45rem;
            min-width: 88px;
            height: 36px;
            padding: 0 .65rem;
            color: #dfeeff;
            background: rgba(255, 255, 255, .12);
            border: 1px solid rgba(255, 255, 255, .22);
            border-radius: 7px;
            font-size: .78rem;
            font-weight: 700;
            box-shadow: inset 0 0 0 1px rgba(255, 255, 255, .05);
        }

        .machine strong::before {
            content: '';
            width: 7px;
            height: 7px;
            background: #8fe0ff;
            border-radius: 50%;
            box-shadow: 0 0 0 3px rgba(143, 224, 255, .18);
        }

        .machine strong.locked {
            color: #dfe8f4;
            background: rgba(255, 255, 255, .08);
            border-color: rgba(255, 255, 255, .14);
        }

        .machine strong.locked::before {
            background: #b1bfd4;
            box-shadow: 0 0 0 3px rgba(177, 191, 212, .2);
        }

        footer {
            display: none;
        }

        main {
            min-height: 100dvh;
        }

        form {
            min-height: 100dvh;
            padding: 1rem 0 2rem;
        }

        .controls {
            max-width: 1100px;
        }

        .scan-area {
            width: min(100%, 1100px);
            margin: 1.3rem auto 0;
        }

        .scan-card,
        .sparepart-list,
        .interlock-panel {
            border: 1px solid rgba(209, 224, 240, .95);
            border-radius: 8px;
            background: #fff;
            color: #10243d;
            box-shadow: 0 8px 24px rgba(3, 25, 56, .13);
        }

        .scan-card {
            padding: .9rem;
        }

        .scan-card h2,
        .sparepart-list h2,
        .interlock-panel h2 {
            margin: 0 0 .65rem;
            color: #10243d;
            font-size: .82rem;
            font-weight: 700;
        }

        .scan-input-row {
            display: grid;
            grid-template-columns: minmax(0, 1fr) auto;
            gap: .65rem;
        }

        .scan-input {
            height: 46px;
            border: 1px solid #94b8f6;
            border-radius: 5px;
            text-align: left;
            box-shadow: 0 0 0 2px rgba(59, 130, 246, .12);
        }

        .reset-scan,
        .release-interlock {
            min-height: 46px;
            padding: 0 1.1rem;
            border: 1px solid #ef7777;
            border-radius: 5px;
            background: #fff;
            color: #dc3b3b;
            font: 700 .76rem Arial, sans-serif;
            cursor: pointer;
        }

        .reset-scan:hover {
            background: #fff1f1;
        }

        .scan-workspace {
            display: grid;
            grid-template-columns: minmax(0, 1.55fr) minmax(250px, .95fr);
            align-items: start;
            gap: .8rem;
            margin-top: .8rem;
        }

        .sparepart-list {
            margin: 0;
            overflow: hidden;
            background: #fff;
        }

        .sparepart-list h2 {
            margin: 0;
            padding: .8rem .9rem;
            background: #f5f8fc;
        }

        .sparepart-list table {
            width: 100%;
            margin: 0;
            border-collapse: collapse;
            background: #fff;
        }

        .sparepart-list th,
        .sparepart-list td {
            padding: .65rem .75rem;
            border-top: 1px solid #e6edf5;
            color: #34465c;
            font-size: .72rem;
        }

        .sparepart-list th {
            background: #f8fafd;
            color: #24384f;
        }

        .sparepart-list .empty-row {
            color: #718096;
            text-align: center;
        }

        .interlock-panel {
            padding: .85rem;
        }

        .interlock-panel h2 {
            margin-bottom: .7rem;
        }

        .interlock-state {
            display: flex;
            align-items: center;
            gap: .85rem;
            min-height: 76px;
            padding: .75rem;
            border: 1px solid #ffc0bb;
            border-radius: 5px;
            background: #fff1f0;
            color: #d93636;
        }

        .interlock-state.running {
            border-color: #a8dfbf;
            background: #effaf3;
            color: #18794e;
        }

        .lock-icon {
            display: grid;
            place-items: center;
            width: 42px;
            height: 42px;
            border-radius: 6px;
            background: currentColor;
            color: #fff;
            font-size: 1.25rem;
            flex: 0 0 auto;
        }

        .interlock-state strong {
            display: block;
            margin-bottom: .2rem;
            font-size: .85rem;
        }

        .interlock-state p {
            margin: 0;
            color: #53657a;
            font-size: .67rem;
            line-height: 1.4;
        }

        .progress-summary {
            display: flex;
            justify-content: space-between;
            gap: .5rem;
            margin: .75rem 0 .35rem;
            color: #34465c;
            font-size: .68rem;
        }

        .progress-track {
            height: 10px;
            overflow: hidden;
            border-radius: 8px;
            background: #d9e0e9;
        }

        .progress-fill {
            display: block;
            width: 0;
            height: 100%;
            border-radius: inherit;
            background: #54b56b;
            transition: width .25s ease;
        }

        .release-interlock {
            width: 100%;
            margin-top: .75rem;
            border-color: #aab4c2;
            background: #aab4c2;
            color: #fff;
        }

        .release-interlock:not(:disabled) {
            border-color: #18794e;
            background: #18794e;
        }

        .release-interlock:disabled,
        .submit:disabled {
            cursor: not-allowed;
        }

        .submit {
            display: none;
        }

        .content {
            max-width: none;
            margin: 0;
            padding: 1rem 10px 1.25rem;
        }

        .controls {
            display: grid;
            grid-template-columns: repeat(4, minmax(0, 1fr));
            gap: .5rem;
            max-width: none;
            margin: 0;
        }

        .controls .field-label {
            position: absolute;
            width: 1px;
            height: 1px;
            padding: 0;
            margin: -1px;
            overflow: hidden;
            clip: rect(0, 0, 0, 0);
            white-space: nowrap;
            border: 0;
        }

        .controls .control,
        .controls .machine,
        .scan-summary {
            height: 26px;
            min-height: 26px;
            border: 1px solid var(--card-border);
            border-radius: 6px;
            background: var(--surface);
            color: var(--ink);
            box-shadow: none;
            font-size: .66rem;
        }

        .controls .field-group {
            gap: 0;
        }

        .controls .line-search,
        .controls .model-search-input {
            height: 24px;
            min-height: 24px;
            padding: 0 .55rem;
            background: transparent;
            color: var(--ink);
            text-align: left;
            font-size: .68rem;
        }

        .controls .line-search::placeholder,
        .controls .model-search-input::placeholder {
            color: var(--muted);
        }

        .controls .line-select::after {
            right: .6rem;
            border-color: var(--blue);
        }

        .controls .model-picker,
        .controls .model-input-wrap {
            min-height: 24px;
            height: 24px;
        }

        .controls .change-model {
            height: 24px;
            padding: 0 .5rem;
            font-size: .62rem;
        }

        .controls .machine {
            padding: 0 .45rem;
            gap: .35rem;
            font-size: .64rem;
        }

        .controls .machine strong {
            min-width: 58px;
            height: 20px;
            padding: 0 .35rem;
            border-radius: 0;
            font-size: .64rem;
            color: var(--blue-dark);
            background: var(--blue-soft);
            border-color: var(--card-border);
        }

        .controls .machine strong.locked {
            color: #475569;
            background: #f1f5f9;
            border-color: #cbd5e1;
        }

        .controls .machine strong:not(.locked) {
            color: var(--green);
            background: var(--green-soft);
            border-color: #a8dfbf;
        }

        .scan-summary {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: .35rem;
            padding: 0 .5rem;
            font-weight: 700;
        }

        .controls .progress-summary {
            margin: 0;
            color: var(--ink);
            font-size: .64rem;
        }

        .scan-area {
            width: 100%;
            margin: .6rem 0 0;
        }

        .scan-workspace {
            display: grid;
            grid-template-columns: minmax(0, .4fr) minmax(0, .6fr);
            grid-template-rows: minmax(0, .53fr) minmax(0, .47fr);
            align-items: stretch;
            gap: .55rem;
            height: calc(100dvh - 240px);
            min-height: 360px;
            margin: .6rem 0 0;
            padding: 1.25rem;
            border: 1px solid var(--card-border);
            border-radius: 10px;
            background: var(--surface);
        }

        .scan-card,
        .sparepart-list,
        .interlock-panel {
            min-width: 0;
            height: 100%;
            border: 1px solid var(--card-border);
            border-radius: 8px;
            background: var(--surface);
            color: var(--ink);
            box-shadow: 0 6px 18px rgba(16, 36, 61, .06);
        }

        .scan-card {
            grid-column: 1;
            grid-row: 1;
            padding: .85rem;
        }

        .interlock-panel {
            grid-column: 2;
            grid-row: 1;
            padding: .85rem;
        }

        .sparepart-list {
            grid-column: 1 / -1;
            grid-row: 2;
            overflow: auto;
        }

        .scan-card h2,
        .sparepart-list h2,
        .interlock-panel h2 {
            color: var(--ink);
        }

        .scan-input {
            box-shadow: none;
        }

        .sparepart-list h2 {
            background: #f8fafc;
        }

        .sparepart-list table {
            background: transparent;
        }

        .sparepart-list th,
        .sparepart-list td {
            border-color: #e2e8f0;
            color: var(--ink);
        }

        .sparepart-list th {
            background: #f8fafc;
        }

        .sparepart-list .empty-row {
            color: var(--muted);
        }

        .interlock-state p {
            color: #52657b;
        }

        @media (max-width: 700px) {
            .content {
                padding: .9rem .85rem 1.25rem;
            }

            form {
                min-height: 0;
            }

            .controls {
                grid-template-columns: repeat(2, minmax(0, 1fr));
                gap: .45rem;
            }

            .scan-area {
                width: 100%;
                margin-top: .6rem;
            }

            .scan-workspace {
                grid-template-columns: 1fr;
                grid-template-rows: auto;
                height: auto;
                min-height: 0;
                padding: .65rem;
            }

            .scan-card,
            .interlock-panel,
            .sparepart-list {
                grid-column: 1;
                grid-row: auto;
                min-height: 220px;
            }

            .sparepart-list {
                min-height: 280px;
            }

            .machine {
                width: 100%;
            }
        }

        @media (max-width: 420px) {
            .controls {
                grid-template-columns: 1fr;
            }

        }
    </style>
        <form id="scan">
            @csrf
            <div class="controls">
                <div class="field-group">
                    <label class="field-label" for="line">Line Produksi</label>
                    <div class="control line-select">
                        <input class="line-search" id="line" name="line" type="search"
                            value="{{ old('line') }}" list="line-options" placeholder="Cari line 1-30"
                            aria-label="Cari dan pilih line" autocomplete="off">
                        <datalist id="line-options">
                            @for ($lineNumber = 1; $lineNumber <= 30; $lineNumber++)
                                <option value="Line {{ $lineNumber }}"></option>
                            @endfor
                        </datalist>
                    </div>
                </div>
                <div class="field-group">
                    <label class="field-label" for="model_name">Model</label>
                    <div class="control model-picker" aria-label="Cari dan pilih model">
                        <div class="model-input-wrap">
                            <input class="model-search-input" id="model_name" name="model_name" type="search"
                                value="{{ old('model_name') }}" list="model-options" placeholder="Cari model..."
                                autocomplete="off" required>
                            <datalist id="model-options">
                                @foreach ($modelNames as $modelName)
                                    <option value="{{ $modelName }}"></option>
                                @endforeach
                            </datalist>
                            <button class="change-model" id="change-model" type="button" hidden>
                                Change Model
                            </button>
                        </div>
                    </div>
                </div>
                <div class="field-group">
                    <span class="field-label">Status Mesin</span>
                    <div class="machine" role="status" aria-live="polite">
                        <span>Machine Status</span>
                        <strong id="machine-status" class="locked">Locked</strong>
                    </div>
                </div>
                <div class="field-group">
                    <span class="field-label">Progres Scan</span>
                    <div class="scan-summary progress-summary" aria-live="polite">
                        <span id="scan-progress-text">0/0 Spare Part OK</span>
                        <strong id="scan-percentage">0%</strong>
                    </div>
                </div>
            </div>

            <div class="scan-area">
                @if (session('success'))
                    <div class="notice" role="status">{{ session('success') }}</div>
                @endif
                @if ($errors->any())
                    <div class="errors" role="alert">{{ $errors->first() }}</div>
                @endif

                <div class="scan-workspace">
                    <section class="scan-card" aria-labelledby="scan-heading">
                        <h2 id="scan-heading">3. Scan Spare Part</h2>
                        <div class="scan-input-row">
                            <label hidden for="component_scan">Scan barcode spare part</label>
                            <input class="scan-input" id="component_scan" name="component_scan" type="text"
                                value="{{ old('component_scan') }}" placeholder="Scan barcode spare part..." autofocus>
                            <button class="reset-scan" id="reset-scan" type="button">Reset</button>
                        </div>
                    </section>

                    <section class="interlock-panel" aria-labelledby="interlock-heading">
                        <h2 id="interlock-heading">Interlock Status (<span id="active-line-label">LINE -</span>)</h2>
                        <div class="interlock-state" id="interlock-state">
                            <span class="lock-icon" aria-hidden="true">&#128274;</span>
                            <div>
                                <strong id="interlock-status-detail">INTERLOCK</strong>
                                <p id="interlock-message">Pilih line dan model untuk memulai scan.</p>
                            </div>
                        </div>
                        <div class="progress-track" role="progressbar" aria-label="Progres scan" aria-valuemin="0"
                            aria-valuemax="100" aria-valuenow="0">
                            <span class="progress-fill" id="scan-progress-fill"></span>
                        </div>
                        <button class="release-interlock" id="release-interlock" type="button" disabled>
                            Release Interlock
                        </button>
                    </section>

                    <section id="items-table-container" class="sparepart-list">
                        <h2 id="sparepart-heading">Daftar Spare Part</h2>
                        <table>
                            <thead>
                                <tr>
                                    <th>No</th>
                                    <th>Spare Part</th>
                                    <th>Status</th>
                                    <th>Scan Time</th>
                                </tr>
                            </thead>
                            <tbody id="items-tbody">
                                <tr>
                                    <td class="empty-row" colspan="4">Pilih model untuk melihat daftar sparepart.
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </section>
                </div>
            </div>
        </form>

        <dialog class="pin-dialog" id="change-model-dialog" aria-labelledby="change-model-title">
            <form id="change-model-pin-form">
                <h2 id="change-model-title">Konfirmasi Change Model</h2>
                <p>Masukkan PIN angka untuk mengganti model. Verifikasi PIN belum dikonfigurasi.</p>
                <label class="field-label" for="change-model-pin">PIN</label>
                <input id="change-model-pin" name="pin" type="password" inputmode="numeric" pattern="[0-9]*"
                    maxlength="12" autocomplete="off" readonly aria-describedby="pin-feedback">
                <div class="pin-keypad" aria-label="Keypad PIN">
                    <button type="button" data-pin-key="1">1</button>
                    <button type="button" data-pin-key="2">2</button>
                    <button type="button" data-pin-key="3">3</button>
                    <button type="button" data-pin-key="4">4</button>
                    <button type="button" data-pin-key="5">5</button>
                    <button type="button" data-pin-key="6">6</button>
                    <button type="button" data-pin-key="7">7</button>
                    <button type="button" data-pin-key="8">8</button>
                    <button type="button" data-pin-key="9">9</button>
                    <button class="pin-key-action" type="button" data-pin-action="clear">Bersihkan</button>
                    <button type="button" data-pin-key="0">0</button>
                    <button class="pin-key-action" type="button" data-pin-action="backspace">Hapus</button>
                </div>
                <p class="pin-feedback" id="pin-feedback" role="alert"></p>
                <div class="pin-dialog-actions">
                    <button id="cancel-change-model" type="button">Batal</button>
                    <button type="submit">Konfirmasi</button>
                </div>
            </form>
        </dialog>

        <script>
            // Handle model search and display items
            const modelInput = document.getElementById('model_name');
            const changeModelButton = document.getElementById('change-model');
            const changeModelDialog = document.getElementById('change-model-dialog');
            const changeModelPinForm = document.getElementById('change-model-pin-form');
            const changeModelPinInput = document.getElementById('change-model-pin');
            const changeModelPinFeedback = document.getElementById('pin-feedback');
            const cancelChangeModelButton = document.getElementById('cancel-change-model');
            const pinKeypad = document.querySelector('.pin-keypad');
            const changeModelVerifyUrl = @json(route('change-model.verify-pin'));
            const itemsTableContainer = document.getElementById('items-table-container');
            const itemsTbody = document.getElementById('items-tbody');
            const scanProgressText = document.getElementById('scan-progress-text');
            const scanPercentage = document.getElementById('scan-percentage');
            const scanProgressFill = document.getElementById('scan-progress-fill');
            const progressTrack = document.querySelector('.progress-track');
            const machineStatus = document.getElementById('machine-status');
            const interlockStatusDetail = document.getElementById('interlock-status-detail');
            const interlockState = document.getElementById('interlock-state');
            const interlockMessage = document.getElementById('interlock-message');
            const lineInput = document.getElementById('line');
            const activeLineLabel = document.getElementById('active-line-label');
            const sparepartHeading = document.getElementById('sparepart-heading');
            const scanForm = document.querySelector('form');
            const componentInput = document.getElementById('component_scan');
            const resetScanButton = document.getElementById('reset-scan');
            const releaseButton = document.getElementById('release-interlock');
            let scannedItems = {}; // Track scanned items
            let totalItems = 0;

            componentInput.disabled = true;
            releaseButton.disabled = true;
            componentInput.placeholder = 'Pilih model terlebih dahulu';
            updateProgress();

            function updateMachineStatus(statusText = 'Locked') {
                const isLocked = statusText === 'Locked';

                machineStatus.textContent = statusText;
                machineStatus.classList.toggle('locked', isLocked);
                interlockStatusDetail.textContent = isLocked ? 'INTERLOCK' : 'RUNNING';
                interlockState.classList.toggle('running', !isLocked);
                const completedCount = Object.values(scannedItems)
                    .filter(item => item.status === 'Success')
                    .length;
                interlockMessage.textContent = !isLocked ?
                    'Semua spare part tervalidasi. Interlock telah dirilis.' :
                    totalItems === 0 ?
                    'Pilih model untuk memulai scan.' :
                    completedCount === totalItems ?
                    'Semua spare part OK. Tekan Release Interlock untuk menjalankan.' :
                    'Masih ada spare part yang belum di-scan.';

                console.log('STATUS MACHINE:', statusText);

                fetch('/interlock/machine-status', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                            'Accept': 'application/json',
                            'X-CSRF-TOKEN': document.querySelector(
                                'meta[name="csrf-token"]'
                            ).content
                        },
                        body: JSON.stringify({
                            status: statusText,
                            line: lineInput.value.trim()
                        })
                    })
                    .then(response => {
                        console.log('HTTP STATUS:', response.status);

                        return response.text().then(text => {
                            console.log('RAW RESPONSE:', text);

                            if (!response.ok) {
                                throw new Error(`HTTP Error ${response.status}: ${text}`);
                            }

                            try {
                                return JSON.parse(text);
                            } catch (error) {
                                throw new Error('Response Laravel bukan JSON: ' + text);
                            }
                        });
                    })
                    .then(data => {
                        console.log('LARAVEL RESPONSE:', data);
                    })
                    .catch(error => {
                        console.error('GAGAL KIRIM STATUS:', error);
                    });
            }

            updateMachineStatus('Locked');

            function updateProgress() {
                const completedCount = Object.values(scannedItems)
                    .filter(item => item.status === 'Success')
                    .length;

                const progressPercent =
                    totalItems > 0 ?
                    Math.round((completedCount / totalItems) * 100) :
                    0;

                scanProgressText.textContent = `${completedCount}/${totalItems} Spare Part OK`;
                scanPercentage.textContent = `${progressPercent}%`;
                scanProgressFill.style.width = `${progressPercent}%`;
                progressTrack.setAttribute('aria-valuenow', String(progressPercent));
            }

            async function loadModelItems(modelName) {
                if (!modelName) {
                    updateMachineStatus('Locked');
                    componentInput.disabled = true;
                    releaseButton.disabled = true;
                    componentInput.placeholder = 'Pilih model terlebih dahulu';
                    sparepartHeading.textContent = 'Daftar Spare Part';
                    itemsTbody.innerHTML =
                        '<tr><td class="empty-row" colspan="4">Pilih model untuk melihat daftar sparepart.</td></tr>';
                    scannedItems = {};
                    totalItems = 0;
                    updateProgress();
                    return;
                }

                try {
                    const response = await fetch(`/api/model-items/${encodeURIComponent(modelName)}`);
                    if (!response.ok) {
                        throw new Error(`Gagal mengambil item model (${response.status})`);
                    }

                    const items = await response.json();
                    itemsTbody.innerHTML = '';
                    itemsTableContainer.style.display = 'block';

                    if (items.length > 0) {
                        scannedItems = {};
                        totalItems = items.length;
                        componentInput.disabled = false;
                        releaseButton.disabled = true;
                        componentInput.placeholder = 'Scan Spareparts Here...';
                        updateMachineStatus('Locked');
                        sparepartHeading.textContent = `Daftar Spare Part (Model: ${modelName})`;

                        items.forEach((item, index) => {
                            const row = document.createElement('tr');
                            row.id = `item-row-${item.id}`;
                            row.style.cssText = 'border-bottom: 1px solid rgba(255, 255, 255, .08);';

                            const numberCell = document.createElement('td');
                            numberCell.textContent = String(index + 1);

                            const itemCell = document.createElement('td');
                            itemCell.textContent = item.item_name;

                            const statusCell = document.createElement('td');
                            statusCell.id = `status-${item.id}`;
                            statusCell.textContent = 'Pending';

                            const scanTimeCell = document.createElement('td');
                            scanTimeCell.id = `scan-time-${item.id}`;
                            scanTimeCell.textContent = '-';

                            row.appendChild(numberCell);
                            row.appendChild(itemCell);
                            row.appendChild(statusCell);
                            row.appendChild(scanTimeCell);
                            itemsTbody.appendChild(row);

                            scannedItems[item.id] = {
                                item_name: item.item_name,
                                status: 'Pending',
                                scanTime: '-'
                            };
                        });

                        itemsTableContainer.style.display = 'block';
                        updateProgress();
                        componentInput.focus();
                    } else {
                        updateMachineStatus('Locked');
                        componentInput.disabled = true;
                        releaseButton.disabled = true;
                        scannedItems = {};
                        totalItems = 0;
                        itemsTbody.innerHTML =
                            '<tr><td class="empty-row" colspan="4">Tidak ada sparepart untuk model ini.</td></tr>';
                        updateProgress();
                        Swal.fire({
                            icon: 'warning',
                            title: 'Model Tidak Ditemukan',
                            text: `Tidak ada item untuk model "${modelName}"`,
                            confirmButtonColor: '#1677d2',
                            confirmButtonText: 'OK'
                        });
                        scannedItems = {};
                        totalItems = 0;
                        updateProgress();
                    }
                } catch (error) {
                    console.error('Error fetching items:', error);
                    componentInput.disabled = true;
                    releaseButton.disabled = true;
                    scannedItems = {};
                    totalItems = 0;
                    itemsTbody.innerHTML =
                        '<tr><td class="empty-row" colspan="4">Gagal memuat daftar sparepart.</td></tr>';
                    itemsTableContainer.style.display = 'block';
                    updateProgress();
                    updateMachineStatus('Locked');
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Gagal mengambil data item',
                        confirmButtonColor: '#1677d2',
                        confirmButtonText: 'OK'
                    });
                }
            }

            modelInput.addEventListener('change', () => {
                if (!modelInput.value.trim()) {
                    return;
                }

                modelInput.readOnly = true;
                changeModelButton.hidden = false;
                loadModelItems(modelInput.value);
            });

            changeModelButton.addEventListener('click', () => {
                changeModelPinInput.value = '';
                changeModelPinFeedback.textContent = '';
                changeModelDialog.showModal();
                changeModelPinInput.focus();
            });

            changeModelPinInput.addEventListener('input', () => {
                changeModelPinInput.value = changeModelPinInput.value.replace(/\D/g, '');
                changeModelPinFeedback.textContent = '';
            });

            pinKeypad.addEventListener('click', event => {
                const button = event.target.closest('button');
                if (!button) {
                    return;
                }

                if (button.dataset.pinKey && changeModelPinInput.value.length < 12) {
                    changeModelPinInput.value += button.dataset.pinKey;
                } else if (button.dataset.pinAction === 'backspace') {
                    changeModelPinInput.value = changeModelPinInput.value.slice(0, -1);
                } else if (button.dataset.pinAction === 'clear') {
                    changeModelPinInput.value = '';
                }

                changeModelPinFeedback.textContent = '';
            });

            cancelChangeModelButton.addEventListener('click', () => {
                changeModelDialog.close();
            });

            changeModelPinForm.addEventListener('submit', event => {
                event.preventDefault();

                if (!changeModelPinInput.value) {
                    changeModelPinFeedback.textContent = 'Masukkan PIN angka.';
                    changeModelPinInput.focus();
                    return;
                }

                fetch(changeModelVerifyUrl, {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                            'Accept': 'application/json',
                            'X-CSRF-TOKEN': document.querySelector('meta[name="csrf-token"]').content
                        },
                        body: JSON.stringify({
                            pin: changeModelPinInput.value
                        })
                    })
                    .then(async response => {
                        const result = await response.json();
                        if (!response.ok) {
                            throw new Error(result.message || 'PIN tidak dapat diverifikasi.');
                        }

                        return result;
                    })
                    .then(() => {
                        changeModelDialog.close();
                        modelInput.readOnly = false;
                        modelInput.value = '';
                        changeModelButton.hidden = true;
                        changeModelPinInput.value = '';
                        loadModelItems('');
                        modelInput.focus();
                    })
                    .catch(error => {
                        changeModelPinFeedback.textContent = error.message;
                    });
            });

            function updateSelectedLine() {
                activeLineLabel.textContent = lineInput.value ?
                    lineInput.value.replace(/^Line\s*/i, 'LINE ') :
                    'LINE -';
            }

            lineInput.addEventListener('input', updateSelectedLine);
            updateSelectedLine();

            // Handle scanning validation

            componentInput.addEventListener('keydown', event => {
                if (event.key === 'Enter') {
                    event.preventDefault();
                    scanForm.requestSubmit();
                }
            });

            scanForm.addEventListener('submit', function(e) {
                e.preventDefault(); // Prevent form submission to server

                const scannedValue = componentInput.value.trim();

                if (!scannedValue || componentInput.disabled) {
                    return;
                }

                const scannedValueUpper = scannedValue.toUpperCase();
                let matched = false;

                // Allow retry: a failed attempt should not permanently lock an item.
                for (const [itemId, itemData] of Object.entries(scannedItems)) {
                    if (itemData.status === 'Success') {
                        continue;
                    }

                    if (itemData.item_name.toUpperCase() === scannedValueUpper) {
                        matched = true;
                        const statusCell = document.getElementById(`status-${itemId}`);
                        if (statusCell) {
                            statusCell.textContent = 'OK';
                            itemData.status = 'Success';
                            itemData.scanTime = new Intl.DateTimeFormat('id-ID', {
                                hour: '2-digit',
                                minute: '2-digit',
                                second: '2-digit'
                            }).format(new Date());
                            const scanTimeCell = document.getElementById(`scan-time-${itemId}`);
                            if (scanTimeCell) {
                                scanTimeCell.textContent = itemData.scanTime;
                            }
                        }

                        const modelName = document.getElementById('model_name').value;
                        Swal.fire({
                            icon: 'success',
                            title: 'Berhasil!',
                            text: `Komponen ${scannedValue} berhasil divalidasi untuk model ${modelName}.`,
                            confirmButtonColor: '#1677d2',
                            confirmButtonText: 'OK'
                        });
                        break;
                    }
                }

                if (!matched && Object.keys(scannedItems).length > 0) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Gagal!',
                        text: `Komponen ${scannedValue} tidak sesuai dengan daftar komponen yang diperlukan. Anda bisa scan ulang.`,
                        confirmButtonColor: '#1677d2',
                        confirmButtonText: 'OK'
                    });
                }

                const successCount = Object.values(scannedItems).filter(item => item.status === 'Success')
                    .length;
                const allCompleted = Object.keys(scannedItems).length > 0 && successCount === totalItems;

                if (allCompleted) {
                    componentInput.disabled = true;
                    componentInput.placeholder = 'Semua komponen sesuai';
                    releaseButton.disabled = false;
                } else {
                    updateMachineStatus('Locked');
                    releaseButton.disabled = true;
                }

                updateProgress();
                componentInput.value = '';
                componentInput.focus();
            });

            resetScanButton.addEventListener('click', () => {
                Object.entries(scannedItems).forEach(([itemId, itemData]) => {
                    itemData.status = 'Pending';
                    itemData.scanTime = '-';

                    const statusCell = document.getElementById(`status-${itemId}`);
                    const scanTimeCell = document.getElementById(`scan-time-${itemId}`);
                    if (statusCell) {
                        statusCell.textContent = 'Pending';
                    }
                    if (scanTimeCell) {
                        scanTimeCell.textContent = '-';
                    }
                });

                componentInput.value = '';
                componentInput.disabled = totalItems === 0;
                componentInput.placeholder = totalItems > 0 ?
                    'Scan barcode spare part...' :
                    'Pilih model terlebih dahulu';
                releaseButton.disabled = true;
                releaseButton.textContent = 'Release Interlock';
                updateProgress();
                updateMachineStatus('Locked');

                if (!componentInput.disabled) {
                    componentInput.focus();
                }
            });

            releaseButton.addEventListener('click', () => {
                const allCompleted = totalItems > 0 &&
                    Object.values(scannedItems).every(item => item.status === 'Success');

                if (!allCompleted) {
                    return;
                }

                updateMachineStatus('Running');
                releaseButton.disabled = true;
                releaseButton.textContent = 'Interlock Released';
            });
        </script>
</x-sidebar>
