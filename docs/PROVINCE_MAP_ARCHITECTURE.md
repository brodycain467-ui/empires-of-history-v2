# Province Map Architecture

## Overview

The Province Map system handles rendering 5,000+ provinces with dynamic ownership changes, border updates, and interactive features on mobile and desktop platforms.

## Architecture Design

### Core Components

```
MapManager (Main coordinator)
├── ProvinceRenderer (Rendering system)
│   ├── ProvinceVisualPool (Object pooling)
│   ├── BorderRenderer (Dynamic borders)
│   └── LabelRenderer (Province names/labels)
├── MapInput (Input handling)
│   ├── TouchInput (Mobile/tablet)
│   └── MouseInput (Desktop)
├── MapCamera (View management)
│   ├── ZoomController
│   ├── PanController
│   └── BoundConstraint
├── ProvinceData (Data layer)
│   └── OwnershipTracker
└── PerformanceOptimizer
    ├── ViewportCulling
    ├── LODSystem
    └── CacheManager
```

## Province Rendering

### Data Flow

```
JSON Province Data
    ↓
ContentDatabase (Load & Cache)
    ↓
ProvinceManager (State management)
    ↓
ProvinceRenderer (Visual representation)
    ↓
ProvinceVisual (Individual province visual)
    ↓
Godot Node2D (Rendered on screen)
```

### Rendering Strategy

#### 1. Data-Driven Province Definition

```csharp
public interface IProvinceVisualRenderer
{
    // Render province based on data
    void RenderProvince(ProvinceData data, Vector2 position);
    void UpdateOwnership(string provinceId, string newOwnerId);
    void UpdateBorders(string provinceId, List<string> neighbors);
    void HighlightProvince(string provinceId);
    void ClearHighlight(string provinceId);
}

public class ProvinceRenderer : IProvinceVisualRenderer
{
    private Dictionary<string, ProvinceVisual> _provinceVisuals;
    private ProvinceVisualPool _visualPool;
    private ContentDatabase _contentDb;
    private MapCamera _camera;
    
    public void RenderProvince(ProvinceData data, Vector2 position)
    {
        // Get visual from pool
        var visual = _visualPool.Get();
        
        // Configure based on data
        visual.SetProvinceData(data);
        visual.SetPosition(position);
        visual.SetColor(GetNationColor(data.OwnerId));
        visual.SetShape(data.Shape); // hex, polygon, etc.
        visual.SetBorders(GetBorderVisuals(data.Id, data.Neighbors));
        
        // Add to rendered dictionary
        _provinceVisuals[data.Id] = visual;
    }
    
    // Future expansion:
    // - Procedural border generation
    // - Dynamic color blending
    // - Terrain overlay system
    // - Cultural/religious indicators
}
```

#### 2. Object Pooling for Performance

```csharp
public interface IProvinceVisualPool
{
    ProvinceVisual Get();
    void Return(ProvinceVisual visual);
    void PreWarm(int count);
}

public class ProvinceVisualPool : IProvinceVisualPool
{
    private Queue<ProvinceVisual> _available;
    private HashSet<ProvinceVisual> _inUse;
    private const int POOL_SIZE = 500; // Visible provinces at once
    
    public ProvinceVisual Get()
    {
        if (_available.Count == 0)
            CreateNew();
        
        var visual = _available.Dequeue();
        _inUse.Add(visual);
        return visual;
    }
    
    public void Return(ProvinceVisual visual)
    {
        visual.Clear();
        _inUse.Remove(visual);
        _available.Enqueue(visual);
    }
    
    // Future expansion:
    // - Dynamic pool sizing
    // - Memory pressure handling
    // - LOD-based pool management
}
```

#### 3. Viewport Culling

```csharp
public interface IViewportOptimizer
{
    void UpdateVisibleProvinces(Rect2 viewportBounds);
    List<string> GetVisibleProvinceIds();
}

public class ViewportOptimizer : IViewportOptimizer
{
    private List<string> _visibleProvinces = new();
    private ProvinceRenderer _renderer;
    private ContentDatabase _contentDb;
    
    public void UpdateVisibleProvinces(Rect2 viewportBounds)
    {
        var newVisible = new List<string>();
        
        foreach (var province in _contentDb.GetAllProvinces())
        {
            if (viewportBounds.HasPoint(province.Position))
            {
                newVisible.Add(province.Id);
            }
        }
        
        // Hide provinces no longer visible
        foreach (var provinceId in _visibleProvinces)
        {
            if (!newVisible.Contains(provinceId))
                _renderer.HideProvince(provinceId);
        }
        
        // Show new visible provinces
        foreach (var provinceId in newVisible)
        {
            if (!_visibleProvinces.Contains(provinceId))
                _renderer.ShowProvince(provinceId);
        }
        
        _visibleProvinces = newVisible;
    }
    
    // Future expansion:
    // - Spatial partitioning (quadtree)
    // - Progressive rendering
    // - LOD distance-based culling
}
```

## Ownership Changes

### Update Flow

```csharp
public interface IOwnershipUpdateHandler
{
    void OnOwnershipTransfer(OwnershipTransfer transfer);
    void OnBatchTransfer(BatchOwnershipTransfer batch);
}

public class OwnershipUpdateHandler : IOwnershipUpdateHandler
{
    private ProvinceRenderer _renderer;
    private BorderUpdateQueue _borderQueue;
    private OwnershipEngine _ownershipEngine;
    
    public void OnOwnershipTransfer(OwnershipTransfer transfer)
    {
        // Update visual
        var newColor = GetNationColor(transfer.NewOwner);
        _renderer.UpdateOwnership(transfer.ProvinceId, newColor);
        
        // Queue border updates for affected neighbors
        QueueBorderUpdates(transfer.ProvinceId);
        
        // Emit UI event
        EventBus.Emit("OwnershipChanged", transfer);
    }
    
    public void OnBatchTransfer(BatchOwnershipTransfer batch)
    {
        var affectedProvinces = new HashSet<string>();
        
        foreach (var transfer in batch.Transfers)
        {
            affectedProvinces.Add(transfer.ProvinceId);
            _renderer.UpdateOwnership(transfer.ProvinceId, GetNationColor(transfer.NewOwner));
        }
        
        // Update all affected borders in one pass
        foreach (var provinceId in affectedProvinces)
        {
            QueueBorderUpdates(provinceId);
        }
    }
    
    private void QueueBorderUpdates(string provinceId)
    {
        var province = _contentDb.GetProvince(provinceId);
        if (province?.Neighbors == null) return;
        
        foreach (var neighborId in province.Neighbors)
        {
            _borderQueue.Enqueue(provinceId, neighborId);
        }
    }
    
    // Future expansion:
    // - Animation system for territory changes
    // - Visual effects for conquests
    // - Sound effects for ownership changes
    // - Historical event triggers
}
```

## Border Rendering

### Dynamic Border System

```csharp
public interface IBorderRenderer
{
    void RenderBorders(string provinceId, List<string> neighborIds);
    void UpdateBorder(string provinceId, string neighborId);
    void ClearBorders(string provinceId);
}

public class BorderRenderer : IBorderRenderer
{
    private Dictionary<string, Line2D> _borderLines;
    private ContentDatabase _contentDb;
    private NationColorProvider _colorProvider;
    
    public void RenderBorders(string provinceId, List<string> neighborIds)
    {
        var province = _contentDb.GetProvince(provinceId);
        var borderKey = $"{provinceId}_borders";
        
        foreach (var neighborId in neighborIds)
        {
            var line = new Line2D();
            line.Width = 2.0f;
            line.Color = GetBorderColor(provinceId, neighborId);
            
            // Generate border line from province geometry
            var borderPoints = CalculateBorderPoints(province, _contentDb.GetProvince(neighborId));
            line.Points = borderPoints;
            
            _borderLines[$"{borderKey}_{neighborId}"] = line;
        }
    }
    
    public void UpdateBorder(string provinceId, string neighborId)
    {
        // Re-render single border
        ClearBorder(provinceId, neighborId);
        RenderBorders(provinceId, new List<string> { neighborId });
    }
    
    private Color GetBorderColor(string provinceId, string neighborId)
    {
        var province = _contentDb.GetProvince(provinceId);
        var neighbor = _contentDb.GetProvince(neighborId);
        
        // Same owner = no border, same color
        if (province.OwnerId == neighbor.OwnerId)
            return Color.Transparent;
        
        // Different owners = war border or peace border
        return IsAtWar(province.OwnerId, neighbor.OwnerId) 
            ? Color.FromHtml("#FF0000")  // Red for war
            : Color.FromHtml("#D4AF37"); // Gold for peace
    }
    
    // Future expansion:
    // - Procedural border generation from coordinates
    // - Animated border pulsing during war
    // - Border thickness based on military presence
    // - Cultural/religious border indicators
    // - Trade route visualization
}
```

## Map Labels (Province Names)

### Label Management

```csharp
public interface ILabelRenderer
{
    void RenderLabel(string provinceId, string label, Vector2 position);
    void UpdateLabel(string provinceId, string newLabel);
    void HideLabel(string provinceId);
    void ShowLabel(string provinceId);
}

public class LabelRenderer : ILabelRenderer
{
    private Dictionary<string, Label> _labels;
    private Label3D _labelTemplate;
    private float _labelMinZoom = 0.5f; // Show labels when zoomed in enough
    
    public void RenderLabel(string provinceId, string label, Vector2 position)
    {
        var labelNode = _labelTemplate.Duplicate() as Label3D;
        labelNode.Text = label;
        labelNode.GlobalPosition = new Vector3(position.X, position.Y, 0);
        labelNode.FontSize = 24;
        labelNode.TextOverrunBehavior = TextServer.OverrunBehavior.TrimEllipsis;
        
        _labels[provinceId] = labelNode;
    }
    
    public void UpdateVisibility(float zoomLevel)
    {
        foreach (var (provinceId, label) in _labels)
        {
            label.Visible = zoomLevel >= _labelMinZoom;
        }
    }
    
    // Future expansion:
    // - Multi-language support
    // - Font size scaling with zoom
    // - Label collision detection
    // - Dynamic label positioning
    // - Historical name variants
}
```

## Zoom and Pan System

### Camera Control

```csharp
public interface IMapCamera
{
    void Zoom(float zoomDelta, Vector2 zoomCenter);
    void Pan(Vector2 panDelta);
    void SetZoomLimits(float min, float max);
    void FocusProvince(string provinceId);
}

public class MapCamera : IMapCamera
{
    private Camera2D _camera;
    private float _currentZoom = 1.0f;
    private float _minZoom = 0.1f;  // Zoomed out to see whole world
    private float _maxZoom = 5.0f;  // Zoomed in for details
    private Rect2 _cameraBounds;
    
    public void Zoom(float zoomDelta, Vector2 zoomCenter)
    {
        var newZoom = Mathf.Clamp(_currentZoom + zoomDelta, _minZoom, _maxZoom);
        
        // Adjust camera position to zoom toward center
        var worldPos = _camera.GetGlobalMousePosition();
        _camera.Zoom = new Vector2(newZoom, newZoom);
        
        _currentZoom = newZoom;
    }
    
    public void Pan(Vector2 panDelta)
    {
        var newPos = _camera.GlobalPosition + panDelta;
        newPos = _cameraBounds.Abs().Clamp(newPos);
        _camera.GlobalPosition = newPos;
    }
    
    public void FocusProvince(string provinceId)
    {
        var province = _contentDb.GetProvince(provinceId);
        if (province != null)
        {
            _camera.GlobalPosition = province.Position;
            _currentZoom = 2.0f; // Zoom to detail level
        }
    }
    
    // Future expansion:
    // - Animated camera movement
    // - Keyboard shortcuts for camera control
    // - Camera preset positions
    // - Touch gesture support (pinch zoom)
    // - Double-tap zoom
}
```

## Input Handling

### Mobile-First Input

```csharp
public interface IMapInput
{
    void RegisterProvinceClickListener(Action<string> onProvinceClick);
    void RegisterCameraGestureListener(Action<CameraGesture> onGesture);
}

public class MapInput : IMapInput
{
    private MapCamera _camera;
    private ProvinceRenderer _renderer;
    private Action<string> _onProvinceClick;
    
    private Vector2 _touchStart;
    private float _pinchDistanceStart;
    
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            HandleMouseClick(mouseEvent);
        }
        else if (@event is InputEventScreenTouch touchEvent)
        {
            HandleTouchEvent(touchEvent);
        }
        else if (@event is InputEventScreenDrag dragEvent)
        {
            HandleDragEvent(dragEvent);
        }
    }
    
    private void HandleMouseClick(InputEventMouseButton mouseEvent)
    {
        if (mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            var clickPos = mouseEvent.Position;
            var provinceId = _renderer.GetProvinceAtPosition(clickPos);
            if (provinceId != null)
                _onProvinceClick?.Invoke(provinceId);
        }
    }
    
    private void HandleTouchEvent(InputEventScreenTouch touchEvent)
    {
        if (touchEvent.Pressed)
        {
            _touchStart = touchEvent.Position;
            var provinceId = _renderer.GetProvinceAtPosition(touchEvent.Position);
            if (provinceId != null)
                _onProvinceClick?.Invoke(provinceId);
        }
    }
    
    private void HandleDragEvent(InputEventScreenDrag dragEvent)
    {
        // Pan camera with drag
        _camera.Pan(-dragEvent.Relative);
    }
    
    // Future expansion:
    // - Multi-touch gesture detection
    // - Pinch zoom for mobile
    // - Long-press context menus
    // - Keyboard shortcuts
}
```

## Performance Targets

### Memory Management

```
Target Configuration:
- Provinces: 5,000
- Nations: 250
- Visible at once: ~300-500
- Memory per province: ~2KB (data) + ~5KB (visual node)

Memory Budget:
- Province data: ~10 MB
- Visual nodes (pooled): ~5 MB (500 active)
- Borders: ~2 MB
- Labels: ~1 MB
- Total: ~18 MB
```

### Rendering Performance

```csharp
public interface IPerformanceMonitor
{
    void RecordFrameTime();
    float GetAverageFPS();
    void LogPerformanceMetrics();
}

public class PerformanceMonitor : IPerformanceMonitor
{
    private Queue<float> _frameTimes = new(300); // Last 300 frames
    private float _frameTimeTarget = 1.0f / 60.0f; // 60 FPS target
    
    public void RecordFrameTime()
    {
        var frameTime = (float)GetPhysicsProcess(0);
        _frameTimes.Enqueue(frameTime);
        
        if (_frameTimes.Count > 300)
            _frameTimes.Dequeue();
        
        if (_frameTimes.Average() > _frameTimeTarget * 1.5f)
        {
            GD.PrintErr("Performance warning: FPS dropping below 40");
            TriggerLODReduction();
        }
    }
    
    // Targets:
    // - Desktop: 60 FPS minimum
    // - Tablet: 30-60 FPS
    // - Mobile: 30 FPS minimum
    // - Ownership change update: <100ms
    // - Province click detection: <50ms
}
```

## LOD (Level of Detail) System

```csharp
public interface ILODManager
{
    void UpdateLOD(float zoomLevel);
    LODLevel GetCurrentLOD();
}

public enum LODLevel
{
    VeryHigh,   // Zoom > 3.0
    High,       // Zoom > 2.0
    Medium,     // Zoom > 1.0
    Low,        // Zoom > 0.5
    VeryLow     // Zoom <= 0.5
}

public class LODManager : ILODManager
{
    public void UpdateLOD(float zoomLevel)
    {
        var newLOD = zoomLevel switch
        {
            > 3.0f => LODLevel.VeryHigh,
            > 2.0f => LODLevel.High,
            > 1.0f => LODLevel.Medium,
            > 0.5f => LODLevel.Low,
            _ => LODLevel.VeryLow
        };
        
        if (newLOD != _currentLOD)
        {
            ApplyLODChanges(newLOD);
        }
    }
    
    private void ApplyLODChanges(LODLevel lod)
    {
        // Adjust visual quality, label visibility, border detail, etc.
        _labelRenderer.UpdateLabelDensity(lod);
        _borderRenderer.UpdateBorderDetail(lod);
        _renderer.UpdateProvinceDetail(lod);
    }
    
    // Future expansion:
    // - Terrain rendering detail
    // - Military unit visibility
    // - Building indicators
    // - Trade route visualization
}
```

## Future Expansion Notes

- **3D Terrain**: Consider upgrade to 3D for globe view
- **Animated Transitions**: Smooth animations for territory changes
- **Weather System**: Visual weather overlays
- **Day/Night Cycle**: Dynamic lighting
- **Military Visualization**: Unit icons and army movements
- **Trade Routes**: Visual trade network display
- **Religion/Culture**: Visual indicators on map
- **Population Indicators**: Heat map visualization
- **Strategic Resources**: Special resource indicators
- **Fog of War**: Vision/scouting system
