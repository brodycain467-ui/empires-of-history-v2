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
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        // Geographical data
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Terrain { get; set; }
        public float Area { get; set; }
        
        // Population and development
        public int Population { get; set; }
        public float DevelopmentLevel { get; set; }
        
        // Ownership
        public string OwnerId { get; set; } // Nation ID or null if unowned
        public DateTime LastOwnershipChange { get; set; }
        
        // Cultural and religious composition
        public List<CulturalComposition> Cultures { get; set; }
        public List<ReligiousComposition> Religions { get; set; }
        
        // Resources
        public List<ResourceAmount> Resources { get; set; }
        
        // Relationships
        public List<string> NeighboringProvinces { get; set; }
        
        // Metadata
        public Dictionary<string, object> Metadata { get; set; }

        public ProvinceModel()
        {
            Cultures = new List<CulturalComposition>();
            Religions = new List<ReligiousComposition>();
            Resources = new List<ResourceAmount>();
            NeighboringProvinces = new List<string>();
            Metadata = new Dictionary<string, object>();
        }
    }

    public class CulturalComposition
    {
        public string CultureId { get; set; }
        public float Percentage { get; set; }
    }

    public class ReligiousComposition
    {
        public string ReligionId { get; set; }
        public float Percentage { get; set; }
    }

    public class ResourceAmount
    {
        public string ResourceId { get; set; }
        public string ResourceType { get; set; }
        public int Quantity { get; set; }
    }

    // Future expansion:
    // - Infrastructure levels
    // - Military garrison
    // - Trade routes
    // - Wonders
    // - Archaeological sites
}
