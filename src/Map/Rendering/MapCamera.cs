using Godot;

namespace EmpiresOfHistoryV2.Map.Rendering;

public partial class MapCamera : Node2D
{
    [Signal]
    public delegate void ZoomChangedEventHandler(float zoom);

    [Signal]
    public delegate void ViewportChangedEventHandler(Rect2 viewportRect);

    private const float MinZoom = 0.15f;
    private const float MaxZoom = 6.0f;

    private Camera2D _camera = null!;
    private bool _isDragging;
    private Vector2 _lastPointerPosition;

    public override void _Ready()
    {
        _camera = new Camera2D
        {
            Enabled = true,
            PositionSmoothingEnabled = true,
            PositionSmoothingSpeed = 12f,
            Zoom = Vector2.One,
            Position = new Vector2(960f, 540f)
        };
        AddChild(_camera);
        EmitViewportChanged();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.ButtonIndex is MouseButton.WheelUp or MouseButton.WheelDown && mouseButton.Pressed)
            {
                var scale = mouseButton.ButtonIndex == MouseButton.WheelUp ? 0.9f : 1.1f;
                ZoomToward(mouseButton.Position, scale);
            }

            if (mouseButton.ButtonIndex == MouseButton.Middle || mouseButton.ButtonIndex == MouseButton.Left)
            {
                _isDragging = mouseButton.Pressed;
                _lastPointerPosition = mouseButton.Position;
            }
        }
        else if (@event is InputEventMouseMotion mouseMotion && _isDragging)
        {
            var delta = mouseMotion.Position - _lastPointerPosition;
            _camera.Position -= delta * _camera.Zoom;
            _lastPointerPosition = mouseMotion.Position;
            ClampCamera();
        }
        else if (@event is InputEventScreenDrag screenDrag)
        {
            _camera.Position -= screenDrag.Relative * _camera.Zoom;
            ClampCamera();
        }
        else if (@event is InputEventMagnifyGesture magnifyGesture)
        {
            ZoomToward(magnifyGesture.Position, 1f / magnifyGesture.Factor);
        }
    }

    public void FocusOn(Vector2 worldPos, float zoom)
    {
        var targetZoom = Mathf.Clamp(zoom, MinZoom, MaxZoom);
        var tween = CreateTween();
        tween.SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
        tween.TweenProperty(_camera, "position", worldPos, 0.35f);
        tween.Parallel().TweenProperty(_camera, "zoom", new Vector2(targetZoom, targetZoom), 0.35f);
        tween.Finished += () =>
        {
            ClampCamera();
            EmitSignal(SignalName.ZoomChanged, _camera.Zoom.X);
        };
    }

    public Rect2 GetViewportWorldRect()
    {
        var viewportSize = GetViewportRect().Size * _camera.Zoom;
        return new Rect2(_camera.Position - (viewportSize / 2f), viewportSize);
    }

    private void ZoomToward(Vector2 screenPosition, float scale)
    {
        var from = GetGlobalMousePosition();
        var newZoom = Mathf.Clamp(_camera.Zoom.X * scale, MinZoom, MaxZoom);
        _camera.Zoom = new Vector2(newZoom, newZoom);
        var to = GetGlobalMousePosition();
        _camera.Position += from - to;
        ClampCamera();
        EmitSignal(SignalName.ZoomChanged, _camera.Zoom.X);
    }

    private void ClampCamera()
    {
        _camera.Position = new Vector2(
            Mathf.Clamp(_camera.Position.X, 0f, 1920f),
            Mathf.Clamp(_camera.Position.Y, 0f, 1080f));
        EmitViewportChanged();
    }

    private void EmitViewportChanged()
    {
        EmitSignal(SignalName.ViewportChanged, GetViewportWorldRect());
    }
}
