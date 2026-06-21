using Godot;
using System.Collections.Generic;

namespace EmpiresOfHistory.Map
{
    /// <summary>
    /// Central registry mapping nation IDs to their map territory fill colors.
    /// All province visuals query this registry so color changes propagate automatically.
    /// </summary>
    public class NationColorRegistry
    {
        private readonly Dictionary<string, Color> _colors = new Dictionary<string, Color>();

        /// <summary>Registers or updates the map color for a nation.</summary>
        public void Register(string nationId, Color color)
        {
            _colors[nationId] = color;
        }

        /// <summary>Returns the map color for a nation, or the default fallback if not registered.</summary>
        public Color GetColor(string nationId)
        {
            return _colors.TryGetValue(nationId, out var color) ? color : GetDefaultColor();
        }

        /// <summary>Fallback muted tan — used for unrecognized or unowned provinces.</summary>
        public Color GetDefaultColor() => new Color("#7a6a50");
    }
}
