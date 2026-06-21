using Godot;

namespace EmpiresOfHistory.Map.Models
{
    /// <summary>
    /// Represents the current runtime state of a single province.
    /// Province ID and static attributes are loaded from JSON (ProvinceData).
    /// This record holds live game state.
    /// </summary>
    public class ProvinceRecord
    {
        // ── Identity ──────────────────────────────────────────────
        public string Id { get; set; }          // e.g. "prov_usa_dc"
        public string Name { get; set; }         // e.g. "Washington D.C."
        public string DisplayName { get; set; }

        // ── Demographics ──────────────────────────────────────────
        public long Population { get; set; }     // current population
        public string PrimaryCultureId { get; set; }   // e.g. "cult_american"
        // PLACEHOLDER: culture system not yet implemented
        // Future: List<CultureGroup> CultureGroups with percentages

        public string PrimaryReligionId { get; set; }  // e.g. "rel_christianity"
        // PLACEHOLDER: religion system not yet implemented
        // Future: List<ReligionGroup> ReligionGroups with percentages

        // ── Ownership ─────────────────────────────────────────────
        public string OwnerNationId { get; set; }       // current owner
        public string CoreNationId { get; set; }        // historical core owner (may differ from current)
        // CoreNationId: a province's "home" nation — the nation that historically owns it.
        // Used for future legitimacy/happiness calculations.

        // ── Geography ─────────────────────────────────────────────
        public bool IsCapital { get; set; }
        public string RegionId { get; set; }     // broad geographic region
        public string Terrain { get; set; }      // plains, mountains, forest, desert, tundra, coastal

        // ── Economy (placeholder values until economy system) ─────
        public float DevelopmentLevel { get; set; }  // 0.0–1.0
        public float StrategicValue { get; set; }    // 0.0–1.0
        // PLACEHOLDER: full economy system comes later

        // ── Neighbors ─────────────────────────────────────────────
        public List<string> NeighborIds { get; set; } = new();
    }
}
