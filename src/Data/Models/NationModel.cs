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
        public string Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        
        // Visual representation
        public string Color { get; set; }
        
        // Government
        public string GovernmentType { get; set; }
        public string CapitalProvinceId { get; set; }
        
        // Culture and religion
        public string PrimaryCulture { get; set; }
        public List<string> SecondaryCultures { get; set; }
        public string PrimaryReligion { get; set; }
        public List<string> SecondaryReligions { get; set; }
        
        // Timeline
        public int FoundingYear { get; set; }
        public int? DissolutionYear { get; set; }
        public string StatusAtPresent { get; set; }
        
        // Classification
        public List<string> Tags { get; set; }
        public string Tier { get; set; } // major_power, regional_power, minor_nation
        public string HistoricalPeriod { get; set; }
        
        // Game settings
        public int InitialTechLevel { get; set; }
        public string AIPersonality { get; set; }
        public float DifficultyModifier { get; set; }
        
        // State
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Metadata
        public Dictionary<string, object> Metadata { get; set; }

        public NationModel()
        {
            SecondaryCultures = new List<string>();
            SecondaryReligions = new List<string>();
            Tags = new List<string>();
            Metadata = new Dictionary<string, object>();
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
