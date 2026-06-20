using System.Collections.Generic;
using Godot;

namespace EmpiresOfHistory.Map.Data
{
    /// <summary>
    /// Hardcoded seed province data for the prototype (~20 provinces across 8 nations).
    /// Map positions are approximate center-points in a 1920×1080 world-map coordinate space.
    /// Province shapes will be replaced by real geographic polygon data in a future phase.
    /// </summary>
    public static class SeedProvinces
    {
        public static List<Province> GetProvinces()
        {
            return new List<Province>
            {
                // ── United States ──────────────────────────────────────────────────────
                new Province
                {
                    Id = "washington_dc",
                    Name = "Washington D.C.",
                    OwnerId = "USA",
                    CoreOwnerId = "USA",
                    MapPosition = new Vector2(420, 395),
                    NeighborIds = new List<string> { "new_york", "chicago" },
                    Type = ProvinceType.Land,
                    IsCapital = true,
                    RegionId = "north_america"
                },
                new Province
                {
                    Id = "new_york",
                    Name = "New York",
                    OwnerId = "USA",
                    CoreOwnerId = "USA",
                    MapPosition = new Vector2(440, 380),
                    NeighborIds = new List<string> { "washington_dc", "chicago" },
                    Type = ProvinceType.Land,
                    IsCapital = false,
                    RegionId = "north_america"
                },
                new Province
                {
                    Id = "chicago",
                    Name = "Chicago",
                    OwnerId = "USA",
                    CoreOwnerId = "USA",
                    MapPosition = new Vector2(390, 370),
                    NeighborIds = new List<string> { "washington_dc", "new_york", "los_angeles" },
                    Type = ProvinceType.Land,
                    IsCapital = false,
                    RegionId = "north_america"
                },
                new Province
                {
                    Id = "los_angeles",
                    Name = "Los Angeles",
                    OwnerId = "USA",
                    CoreOwnerId = "USA",
                    MapPosition = new Vector2(240, 420),
                    NeighborIds = new List<string> { "chicago" },
                    Type = ProvinceType.Land,
                    IsCapital = false,
                    RegionId = "north_america"
                },

                // ── Russia ─────────────────────────────────────────────────────────────
                new Province
                {
                    Id = "moscow",
                    Name = "Moscow",
                    OwnerId = "RUS",
                    CoreOwnerId = "RUS",
                    MapPosition = new Vector2(1040, 280),
                    NeighborIds = new List<string> { "st_petersburg", "siberia" },
                    Type = ProvinceType.Land,
                    IsCapital = true,
                    RegionId = "eastern_europe"
                },
                new Province
                {
                    Id = "st_petersburg",
                    Name = "Saint Petersburg",
                    OwnerId = "RUS",
                    CoreOwnerId = "RUS",
                    MapPosition = new Vector2(1010, 255),
                    NeighborIds = new List<string> { "moscow" },
                    Type = ProvinceType.Land,
                    IsCapital = false,
                    RegionId = "eastern_europe"
                },
                new Province
                {
                    Id = "siberia",
                    Name = "Siberia",
                    OwnerId = "RUS",
                    CoreOwnerId = "RUS",
                    MapPosition = new Vector2(1200, 240),
                    NeighborIds = new List<string> { "moscow", "beijing" },
                    Type = ProvinceType.Land,
                    IsCapital = false,
                    RegionId = "central_asia"
                },

                // ── China ──────────────────────────────────────────────────────────────
                new Province
                {
                    Id = "beijing",
                    Name = "Beijing",
                    OwnerId = "CHN",
                    CoreOwnerId = "CHN",
                    MapPosition = new Vector2(1370, 340),
                    NeighborIds = new List<string> { "shanghai", "siberia" },
                    Type = ProvinceType.Land,
                    IsCapital = true,
                    RegionId = "east_asia"
                },
                new Province
                {
                    Id = "shanghai",
                    Name = "Shanghai",
                    OwnerId = "CHN",
                    CoreOwnerId = "CHN",
                    MapPosition = new Vector2(1400, 390),
                    NeighborIds = new List<string> { "beijing" },
                    Type = ProvinceType.Land,
                    IsCapital = false,
                    RegionId = "east_asia"
                },

                // ── Germany ────────────────────────────────────────────────────────────
                new Province
                {
                    Id = "berlin",
                    Name = "Berlin",
                    OwnerId = "DEU",
                    CoreOwnerId = "DEU",
                    MapPosition = new Vector2(910, 285),
                    NeighborIds = new List<string> { "munich", "paris" },
                    Type = ProvinceType.Land,
                    IsCapital = true,
                    RegionId = "western_europe"
                },
                new Province
                {
                    Id = "munich",
                    Name = "Munich",
                    OwnerId = "DEU",
                    CoreOwnerId = "DEU",
                    MapPosition = new Vector2(915, 305),
                    NeighborIds = new List<string> { "berlin", "paris" },
                    Type = ProvinceType.Land,
                    IsCapital = false,
                    RegionId = "western_europe"
                },

                // ── Brazil ─────────────────────────────────────────────────────────────
                new Province
                {
                    Id = "brasilia",
                    Name = "Brasília",
                    OwnerId = "BRA",
                    CoreOwnerId = "BRA",
                    MapPosition = new Vector2(580, 610),
                    NeighborIds = new List<string> { "sao_paulo" },
                    Type = ProvinceType.Land,
                    IsCapital = true,
                    RegionId = "south_america"
                },
                new Province
                {
                    Id = "sao_paulo",
                    Name = "São Paulo",
                    OwnerId = "BRA",
                    CoreOwnerId = "BRA",
                    MapPosition = new Vector2(600, 650),
                    NeighborIds = new List<string> { "brasilia" },
                    Type = ProvinceType.Land,
                    IsCapital = false,
                    RegionId = "south_america"
                },

                // ── India ──────────────────────────────────────────────────────────────
                new Province
                {
                    Id = "new_delhi",
                    Name = "New Delhi",
                    OwnerId = "IND",
                    CoreOwnerId = "IND",
                    MapPosition = new Vector2(1200, 410),
                    NeighborIds = new List<string> { "mumbai" },
                    Type = ProvinceType.Land,
                    IsCapital = true,
                    RegionId = "south_asia"
                },
                new Province
                {
                    Id = "mumbai",
                    Name = "Mumbai",
                    OwnerId = "IND",
                    CoreOwnerId = "IND",
                    MapPosition = new Vector2(1175, 450),
                    NeighborIds = new List<string> { "new_delhi" },
                    Type = ProvinceType.Land,
                    IsCapital = false,
                    RegionId = "south_asia"
                },

                // ── United Kingdom ─────────────────────────────────────────────────────
                new Province
                {
                    Id = "london",
                    Name = "London",
                    OwnerId = "GBR",
                    CoreOwnerId = "GBR",
                    MapPosition = new Vector2(845, 272),
                    NeighborIds = new List<string> { "manchester", "paris" },
                    Type = ProvinceType.Land,
                    IsCapital = true,
                    RegionId = "western_europe"
                },
                new Province
                {
                    Id = "manchester",
                    Name = "Manchester",
                    OwnerId = "GBR",
                    CoreOwnerId = "GBR",
                    MapPosition = new Vector2(840, 262),
                    NeighborIds = new List<string> { "london" },
                    Type = ProvinceType.Land,
                    IsCapital = false,
                    RegionId = "western_europe"
                },

                // ── France ─────────────────────────────────────────────────────────────
                new Province
                {
                    Id = "paris",
                    Name = "Paris",
                    OwnerId = "FRA",
                    CoreOwnerId = "FRA",
                    MapPosition = new Vector2(862, 292),
                    NeighborIds = new List<string> { "lyon", "berlin", "london" },
                    Type = ProvinceType.Land,
                    IsCapital = true,
                    RegionId = "western_europe"
                },
                new Province
                {
                    Id = "lyon",
                    Name = "Lyon",
                    OwnerId = "FRA",
                    CoreOwnerId = "FRA",
                    MapPosition = new Vector2(865, 308),
                    NeighborIds = new List<string> { "paris", "munich" },
                    Type = ProvinceType.Land,
                    IsCapital = false,
                    RegionId = "western_europe"
                }
            };
        }
    }
}
