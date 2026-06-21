using System;
using System.Linq;
using Godot;
using EmpiresOfHistoryV2.Map.Models;

namespace EmpiresOfHistoryV2.Map.Rendering;

public partial class ProvinceVisual : Polygon2D
{
    [Signal]
    public delegate void ClickedEventHandler(string provinceId);

    private static readonly Color SelectedBorderColor = Color.FromHtml("#c9a84c");

    private ProvinceData? _province;
    private Color _baseColor;
    private Line2D _selectionBorder = null!;
    private Label _label = null!;

    public string ProvinceId => _province?.Id ?? string.Empty;

    public override void _Ready()
    {
        ZIndex = 2;
        _selectionBorder = new Line2D
        {
            DefaultColor = SelectedBorderColor,
            Width = 2f,
            Closed = true,
            Visible = false,
            ZIndex = 5
        };
        AddChild(_selectionBorder);

        _label = new Label
        {
            Visible = false,
            HorizontalAlignment = HorizontalAlignment.Center,
            Modulate = Color.FromHtml("#f0e6cc"),
            MouseFilter = Control.MouseFilterEnum.Ignore
        };
        AddChild(_label);
    }

    public void Setup(ProvinceData data, Color nationColor)
    {
        _province = data;
        _baseColor = data.Capital ? nationColor.Lightened(0.08f) : nationColor;
        Polygon = data.Vertices.Select(v => new Vector2(v[0], v[1])).ToArray();
        Color = _baseColor;

        _selectionBorder.Points = Polygon;
        _label.Text = data.DisplayName;
        _label.Position = CalculateCentroid() + new Vector2(-50, -10);
        QueueRedraw();
    }

    public void SetHovered(bool hovered)
    {
        if (_province == null)
        {
            return;
        }

        Color = hovered ? _baseColor.Lightened(0.15f) : _baseColor;
    }

    public void SetSelected(bool selected)
    {
        _selectionBorder.Visible = selected;
    }

    public void SetLabelVisible(bool visible)
    {
        _label.Visible = visible;
    }

    public bool ContainsGlobalPoint(Vector2 globalPoint)
    {
        var localPoint = ToLocal(globalPoint);
        return Geometry2D.IsPointInPolygon(localPoint, Polygon);
    }

    public void EmitClicked()
    {
        if (_province != null)
        {
            EmitSignal(SignalName.Clicked, _province.Id);
        }
    }

    public override void _Draw()
    {
        if (_province?.Capital == true)
        {
            DrawCircle(CalculateCentroid(), 4f, SelectedBorderColor);
        }
    }

    private Vector2 CalculateCentroid()
    {
        if (Polygon.Length == 0)
        {
            return Vector2.Zero;
        }

        var sum = Vector2.Zero;
        foreach (var point in Polygon)
        {
            sum += point;
        }

        return sum / Polygon.Length;
    }
}
