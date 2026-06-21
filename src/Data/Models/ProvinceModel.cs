using System;
using System.Collections.Generic;

namespace EmpiresOfHistory.Data.Models
{
    /// <summary>
    /// ProvinceModel represents a geographical/political region
    /// Data-driven from provinces.json
    /// </summary>
    public class ProvinceModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        // Geographical data
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Terrain { get; set; } = string.Empty;
        public float Area { get; set; }
        
        // Population and development
        public int Population { get; set; }
        public float DevelopmentLevel { get; set; }
        
        // Ownership
        public string? OwnerId { get; set; } // Nation ID or null if unowned
        public DateTime LastOwnershipChange { get; set; }
        
        // Cultural and religious composition
        public List<CulturalComposition> Cultures { get; set; } = new();
        public List<ReligiousComposition> Religions { get; set; } = new();
        
        // Resources
        public List<ResourceAmount> Resources { get; set; } = new();
        
        // Relationships
        public List<string> NeighboringProvinces { get; set; } = new();
        
        // Metadata
        public Dictionary<string, object> Metadata { get; set; } = new();

    }

    public class CulturalComposition
    {
        public string CultureId { get; set; } = string.Empty;
        public float Percentage { get; set; }
    }

    public class ReligiousComposition
    {
        public string ReligionId { get; set; } = string.Empty;
        public float Percentage { get; set; }
    }

    public class ResourceAmount
    {
        public string ResourceId { get; set; } = string.Empty;
        public string ResourceType { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }

    // Future expansion:
    // - Infrastructure levels
    // - Military garrison
    // - Trade routes
    // - Wonders
    // - Archaeological sites
}
