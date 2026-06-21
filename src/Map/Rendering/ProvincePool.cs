using System.Collections.Generic;
using Godot;

namespace EmpiresOfHistoryV2.Map.Rendering;

public partial class ProvincePool : Node
{
    private readonly Queue<ProvinceVisual> _pool = new();
    private PackedScene _provinceScene = null!;

    public override void _Ready()
    {
        _provinceScene = GD.Load<PackedScene>("res://scenes/Map/ProvinceVisual.tscn");
        Prewarm(500);
    }

    private void Prewarm(int count)
    {
        for (var i = 0; i < count; i++)
        {
            var visual = _provinceScene.Instantiate<ProvinceVisual>();
            visual.Visible = false;
            AddChild(visual);
            _pool.Enqueue(visual);
        }
    }

    public ProvinceVisual Get()
    {
        var visual = _pool.Count > 0 ? _pool.Dequeue() : _provinceScene.Instantiate<ProvinceVisual>();
        visual.Visible = true;
        return visual;
    }

    public void Return(ProvinceVisual visual)
    {
        visual.Visible = false;
        visual.SetSelected(false);
        visual.SetHovered(false);
        if (visual.GetParent() != this)
        {
            visual.Reparent(this);
        }

        _pool.Enqueue(visual);
    }
}
