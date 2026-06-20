using System.Collections.Generic;
using Godot;

namespace EmpiresOfHistoryV2.Map.Rendering;

public partial class LODManager : Node
{
    private readonly List<ProvinceVisual> _visuals = new();

    public void Initialize(MapCamera camera, IEnumerable<ProvinceVisual> visuals)
    {
        _visuals.Clear();
        _visuals.AddRange(visuals);
        camera.ZoomChanged += OnZoomChanged;
    }

    private void OnZoomChanged(float zoom)
    {
        var showLabels = zoom <= 0.75f;
        foreach (var visual in _visuals)
        {
            visual.SetLabelVisible(showLabels);
        }
    }
}
