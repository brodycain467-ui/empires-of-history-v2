using Godot;
using System.Collections.Generic;

namespace EmpiresOfHistory.Map
{
    public enum NationTier
    {
        GreatPower,
        RegionalPower,
        DevelopingNation,
        MicroState
    }

    public enum GovernmentType
    {
        PresidentialRepublic,
        ParliamentaryRepublic,
        Monarchy,
        AbsoluteMonarchy,
        Dictatorship,
        CommunistState,
        Theocracy,
        Tribal
    }

    /// <summary>
    /// A named government official. Placeholder until the full official roster system is built.
    /// </summary>
    public class Official
    {
        public string Title { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// Data model representing a playable or AI-controlled nation.
    /// </summary>
    public class Nation
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }

        /// <summary>3-letter ISO-style country tag (e.g. "USA", "RUS").</summary>
        public string Tag { get; set; }

        public NationTier Tier { get; set; }
        public GovernmentType Government { get; set; }

        /// <summary>Province ID of this nation's capital city.</summary>
        public string CapitalProvinceId { get; set; }

        /// <summary>Muted, earthy fill color used to paint this nation's territory on the map.</summary>
        public Color MapColor { get; set; }

        public long Population { get; set; }

        /// <summary>National treasury in USD (approximate real-world value for prototyping).</summary>
        public double Treasury { get; set; }

        /// <summary>Path to flag texture resource (e.g. "res://assets/flags/usa.png").</summary>
        public string FlagPath { get; set; }

        // PLACEHOLDER: Historical accuracy score (0–100).
        // This value will be calculated by the Historical Accuracy Engine in a future phase.
        // Hard-coded seed values are illustrative only.
        public double HistoricalAccuracyScore { get; set; }

        // PLACEHOLDER: Up to 3 top government officials.
        // Will be populated from the official roster system in a future phase.
        public List<Official> TopOfficials { get; set; } = new List<Official>();
    }
}
