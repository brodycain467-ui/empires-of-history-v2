using System;
using System.Collections.Generic;
using Godot;
using EmpiresOfHistoryV2.Map.Models;

namespace EmpiresOfHistoryV2.Map.Rendering;

public partial class ViewportCuller : Node
{
    private readonly Dictionary<string, ProvinceData> _provincesById = new();
    private Action<string>? _showProvince;
    private Action<string>? _hideProvince;
    private MapCamera? _camera;

    public void Initialize(
        MapCamera camera,
        IEnumerable<ProvinceData> provinces,
        Action<string> showProvince,
        Action<string> hideProvince)
    {
        if (_camera != null)
        {
            _camera.ViewportChanged -= OnViewportChanged;
        }

        _camera = camera;
        _provincesById.Clear();
        foreach (var province in provinces)
        {
            _provincesById[province.Id] = province;
        }

        _showProvince = showProvince;
        _hideProvince = hideProvince;
        _camera.ViewportChanged += OnViewportChanged;
    }

    private void OnViewportChanged(Rect2 viewport)
    {
        foreach (var province in _provincesById.Values)
        {
            var point = new Vector2(province.Coordinates.X, province.Coordinates.Y);
            if (viewport.Grow(200f).HasPoint(point))
            {
                _showProvince?.Invoke(province.Id);
            }
            else
            {
                _hideProvince?.Invoke(province.Id);
            }
        }
    }
}
