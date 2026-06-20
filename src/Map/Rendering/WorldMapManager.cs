using System.Collections.Generic;
using System.Linq;
using Godot;
using EmpiresOfHistoryV2.Core;
using EmpiresOfHistoryV2.Map.Models;

namespace EmpiresOfHistoryV2.Map.Rendering;

public partial class WorldMapManager : Node2D
{
    public event System.Action<NationData>? NationSelected;
    public event System.Action<ProvinceData>? ProvinceSelected;

    private readonly Dictionary<string, ProvinceData> _provinceDataById = new();
    private readonly Dictionary<string, ProvinceVisual> _activeVisuals = new();

    private ProvincePool _pool = null!;
    private ViewportCuller _culler = null!;
    private LODManager _lodManager = null!;
    private MapCamera _mapCamera = null!;

    public override void _Ready()
    {
        AddOcean();

        _pool = new ProvincePool();
        AddChild(_pool);

        _culler = new ViewportCuller();
        AddChild(_culler);

        _lodManager = new LODManager();
        AddChild(_lodManager);

        _mapCamera = GetNodeOrNull<MapCamera>("../MapCamera") ?? GetNode<MapCamera>("../../MapCamera");

        var provinces = GameManager.Instance.ContentDatabase.GetAllProvinces();
        foreach (var province in provinces)
        {
            _provinceDataById[province.Id] = province;
            CreateProvinceVisual(province);
        }

        _culler.Initialize(_mapCamera, _provinceDataById.Values, EnsureVisible, EnsureHidden);
        _lodManager.Initialize(_mapCamera, () => _activeVisuals.Values);

        if (GameManager.Instance.GameState.SelectedNationId is { } nationId)
        {
            SelectNation(nationId);
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left } mouse)
        {
            var clicked = _activeVisuals.Values.Reverse().FirstOrDefault(v => v.Visible && v.ContainsGlobalPoint(mouse.Position));
            if (clicked != null)
            {
                clicked.EmitClicked();
            }
        }

        if (@event is InputEventMouseMotion motion)
        {
            UpdateHover(motion.Position);
        }
    }

    public void SelectNation(string nationId)
    {
        foreach (var visual in _activeVisuals.Values)
        {
            var ownerId = GameManager.Instance.OwnershipSystem.GetOwner(visual.ProvinceId);
            visual.SetSelected(ownerId == nationId);
        }
    }

    private void AddOcean()
    {
        var ocean = new ColorRect
        {
            Color = Color.FromHtml("#0d1b2a"),
            Position = Vector2.Zero,
            Size = new Vector2(1920f, 1080f),
            MouseFilter = Control.MouseFilterEnum.Ignore
        };
        AddChild(ocean);
    }

    private void CreateProvinceVisual(ProvinceData province)
    {
        var visual = _pool.Get();
        _activeVisuals[province.Id] = visual;
        if (visual.GetParent() != this)
        {
            visual.Reparent(this);
        }

        var ownerId = GameManager.Instance.OwnershipSystem.GetOwner(province.Id);
        var color = GameManager.Instance.NationColorRegistry.GetColor(ownerId);
        visual.Setup(province, color);
        visual.Clicked += OnProvinceClicked;
    }

    private void EnsureVisible(string provinceId)
    {
        if (_activeVisuals.ContainsKey(provinceId) || !_provinceDataById.TryGetValue(provinceId, out var province))
        {
            return;
        }

        CreateProvinceVisual(province);
    }

    private void EnsureHidden(string provinceId)
    {
        if (!_activeVisuals.TryGetValue(provinceId, out var visual))
        {
            return;
        }

        visual.Clicked -= OnProvinceClicked;
        _activeVisuals.Remove(provinceId);
        _pool.Return(visual);
    }

    private void OnProvinceClicked(string provinceId)
    {
        var province = _provinceDataById[provinceId];
        var ownerId = GameManager.Instance.OwnershipSystem.GetOwner(provinceId);
        var nation = GameManager.Instance.ContentDatabase.GetNation(ownerId);

        GameManager.Instance.GameState.SelectedProvinceId = provinceId;
        GameManager.Instance.GameState.SelectedNationId = ownerId;

        SelectNation(ownerId);
        ProvinceSelected?.Invoke(province);
        if (nation != null)
        {
            NationSelected?.Invoke(nation);
        }
    }

    private void UpdateHover(Vector2 mousePosition)
    {
        foreach (var visual in _activeVisuals.Values)
        {
            visual.SetHovered(visual.ContainsGlobalPoint(mousePosition));
        }
    }
}
