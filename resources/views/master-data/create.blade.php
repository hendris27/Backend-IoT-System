<x-sidebar title="Create Master Data CLL">
    <div style="max-width: 500px; margin: 0 auto;">
        <a href="{{ route('master-data.index') }}"
            style="color: var(--blue); text-decoration: none; margin-bottom: 1.5rem; display: inline-block;">← Back</a>

        <div class="surface-card" style="padding: 1.5rem; margin-bottom: 1.5rem;">
            <h2 style="font-size: 1.1rem; font-weight: 700; margin-bottom: 1.5rem;">Create Master Data</h2>

            <form action="{{ route('master-data.store') }}" method="POST">
                @csrf

                <div style="margin-bottom: 1.5rem;">
                    <label for="model_name"
                        style="display: block; font-size: .85rem; font-weight: 700; margin-bottom: 0.5rem;">Model
                        Name</label>
                    <input type="text" name="model_name" id="model_name"
                        style="width: 100%; padding: 0.6rem; border: 1px solid var(--card-border); border-radius: 7px; background: #fff; color: var(--ink); font-size: .85rem;"
                        value="{{ old('model_name') }}" required>
                    @error('model_name')
                        <p style="color: #b42318; font-size: 0.75rem; margin-top: 0.3rem;">{{ $message }}</p>
                    @enderror
                </div>

                <div style="margin-bottom: 1.5rem;">
                    <label for="item_name"
                        style="display: block; font-size: .85rem; font-weight: 700; margin-bottom: 0.5rem;">Item Name /
                        Component</label>
                    <input type="text" name="item_name" id="item_name"
                        style="width: 100%; padding: 0.6rem; border: 1px solid var(--card-border); border-radius: 7px; background: #fff; color: var(--ink); font-size: .85rem;"
                        value="{{ old('item_name') }}" required>
                    @error('item_name')
                        <p style="color: #b42318; font-size: 0.75rem; margin-top: 0.3rem;">{{ $message }}</p>
                    @enderror
                </div>

                <div style="display: flex; gap: 0.75rem;">
                    <button type="submit" class="btn btn-primary">Create</button>
                    <a href="{{ route('master-data.index') }}" class="btn"
                        style="background: #f1f5f9; border: 1px solid var(--card-border); color: var(--ink);">Cancel</a>
                </div>
            </form>
        </div>
    </div>
</x-sidebar>
