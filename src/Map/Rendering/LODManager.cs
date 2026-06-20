using Godot;

namespace EmpiresOfHistoryV2.Map.Rendering;

public partial class LODManager : Node
{
    private System.Func<System.Collections.Generic.IEnumerable<ProvinceVisual>> _visualProvider =
        () => System.Array.Empty<ProvinceVisual>();
    private MapCamera? _camera;

    public void Initialize(
        MapCamera camera,
        System.Func<System.Collections.Generic.IEnumerable<ProvinceVisual>> visualProvider)
    {
        if (_camera != null)
        {
            _camera.ZoomChanged -= OnZoomChanged;
        }

        _camera = camera;
        _visualProvider = visualProvider;
        _camera.ZoomChanged += OnZoomChanged;
    }

    private void OnZoomChanged(float zoom)
    {
        var showLabels = zoom >= 0.75f;
        foreach (var visual in _visualProvider())
        {
            visual.SetLabelVisible(showLabels);
        }
    }
}
