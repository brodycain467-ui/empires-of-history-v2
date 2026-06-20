using Godot;

namespace EmpiresOfHistory.Map
{
    /// <summary>
    /// Visual representation of a single province on the world map.
    /// Renders as a colored rectangle; supports hover brightening and a gold selection border.
    /// Province shape data (polygon outlines) will replace the rectangle in a future phase.
    /// </summary>
    [GlobalClass]
    public partial class ProvinceVisual : ColorRect
    {
        // Gold border color used when this province is selected — matches the approved UI accent.
        private static readonly Color SelectionBorderColor = new Color("#c9a84c");
        private const float HoverBrightenAmount = 0.15f;
        private const float SelectionBorderWidth = 2.0f;

        [Signal]
        public delegate void ClickedEventHandler(string provinceId);

        private string _provinceId;
        private Color _baseColor;
        private bool _isHovered = false;
        private bool _isSelected = false;

        /// <summary>
        /// Initialises this visual with its province ID and nation fill color.
        /// Must be called before adding the node to the scene tree.
        /// </summary>
        public void Initialize(string provinceId, Color nationColor, Vector2 position, Vector2 size)
        {
            _provinceId = provinceId;
            _baseColor = nationColor;
            Color = nationColor;
            Position = position;
            Size = size;
            MouseFilter = MouseFilterEnum.Stop;
        }

        public string ProvinceId => _provinceId;

        // ── Input ──────────────────────────────────────────────────────────────────────

        public override void _GuiInput(InputEvent @event)
        {
            if (@event is InputEventMouseButton mouseButton
                && mouseButton.ButtonIndex == MouseButton.Left
                && mouseButton.Pressed)
            {
                EmitSignal(SignalName.Clicked, _provinceId);
                AcceptEvent();
            }
        }

        // ── Hover ──────────────────────────────────────────────────────────────────────

        public override void _Notification(int what)
        {
            base._Notification(what);
            if (what == NotificationMouseEnter)
            {
                _isHovered = true;
                if (!_isSelected)
                    Color = _baseColor.Lightened(HoverBrightenAmount);
                QueueRedraw();
            }
            else if (what == NotificationMouseExit)
            {
                _isHovered = false;
                if (!_isSelected)
                    Color = _baseColor;
                QueueRedraw();
            }
        }

        // ── Selection state ────────────────────────────────────────────────────────────

        /// <summary>Marks this province as selected or deselected and redraws.</summary>
        public void SetSelected(bool selected)
        {
            _isSelected = selected;
            if (!selected && !_isHovered)
                Color = _baseColor;
            QueueRedraw();
        }

        /// <summary>Updates the fill color (e.g., after an ownership transfer).</summary>
        public void UpdateColor(Color newColor)
        {
            _baseColor = newColor;
            if (!_isSelected && !_isHovered)
                Color = newColor;
        }

        // ── Custom draw: selection border ──────────────────────────────────────────────

        public override void _Draw()
        {
            base._Draw();
            if (_isSelected)
            {
                // Draw a gold border inside the rectangle bounds.
                DrawRect(new Rect2(Vector2.Zero, Size), SelectionBorderColor, false, SelectionBorderWidth);
            }
        }
    }
}
