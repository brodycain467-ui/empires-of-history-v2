using Godot;
using System.Collections.Generic;

namespace EmpiresOfHistory.Map
{
    public enum ProvinceType
    {
        Land,
        Sea,
        Lake
    }

    /// <summary>
    /// Data model representing a single province on the world map.
    /// Provinces are the fundamental territorial unit of the game.
    /// </summary>
    public class Province
    {
        public string Id { get; set; }
        public string Name { get; set; }

        /// <summary>Nation ID of the current controlling nation.</summary>
        public string OwnerId { get; set; }

        /// <summary>Nation ID that historically owns this province (core claim).</summary>
        public string CoreOwnerId { get; set; }

        /// <summary>Center point of this province on the world map (1920×1080 coordinate space).</summary>
        public Vector2 MapPosition { get; set; }

        /// <summary>List of adjacent province IDs sharing a border.</summary>
        public List<string> NeighborIds { get; set; } = new List<string>();

        public ProvinceType Type { get; set; } = ProvinceType.Land;

        /// <summary>True if this province is the capital of its owner nation.</summary>
        public bool IsCapital { get; set; }

        /// <summary>Region group identifier (e.g., "Western Europe", "East Asia").</summary>
        public string RegionId { get; set; }
    }
}
