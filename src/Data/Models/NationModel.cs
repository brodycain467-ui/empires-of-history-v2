using System;
using System.Collections.Generic;

namespace EmpiresOfHistory.Data.Models
{
    /// <summary>
    /// NationModel represents a playable nation or faction
    /// Data-driven from nations.json
    /// </summary>
    public class NationModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        // Visual representation
        public string Color { get; set; } = string.Empty;
        
        // Government
        public string GovernmentType { get; set; } = string.Empty;
        public string CapitalProvinceId { get; set; } = string.Empty;
        
        // Culture and religion
        public string PrimaryCulture { get; set; } = string.Empty;
        public List<string> SecondaryCultures { get; set; } = new();
        public string PrimaryReligion { get; set; } = string.Empty;
        public List<string> SecondaryReligions { get; set; } = new();
        
        // Timeline
        public int FoundingYear { get; set; }
        public int? DissolutionYear { get; set; }
        public string StatusAtPresent { get; set; } = string.Empty;
        
        // Classification
        public List<string> Tags { get; set; } = new();
        public string Tier { get; set; } = string.Empty;
        public string HistoricalPeriod { get; set; } = string.Empty;
        
        // Game settings
        public int InitialTechLevel { get; set; }
        public string AIPersonality { get; set; } = string.Empty;
        public float DifficultyModifier { get; set; }
        
        // State
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Metadata
        public Dictionary<string, object> Metadata { get; set; } = new();

        public NationModel()
        {
            IsActive = true;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }

    // Future expansion:
    // - Diplomatic relations
    // - Technology tree state
    // - Military forces
    // - Economic systems
    // - Succession rules
    // - Government transitions
}
