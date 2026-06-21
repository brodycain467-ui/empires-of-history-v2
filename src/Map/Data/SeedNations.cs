using System.Collections.Generic;
using Godot;

namespace EmpiresOfHistory.Map.Data
{
    /// <summary>
    /// Hardcoded prototype seed data for 8 real-world nations.
    /// All values (population, treasury, officials, historical accuracy) are approximate
    /// real-world figures used for prototyping only. They will be replaced by the
    /// Content Database and Historical Accuracy Engine in future phases.
    ///
    /// SNAPSHOT DATE: Officials and statistics reflect publicly available data as of
    /// approximately early 2024. Government positions change over time; treat these
    /// values as illustrative stand-ins, not live data.
    /// </summary>
    public static class SeedNations
    {
        public static List<Nation> GetNations()
        {
            return new List<Nation>
            {
                // ── United States ──────────────────────────────────────────────────────
                new Nation
                {
                    Id = "USA",
                    Name = "United States of America",
                    ShortName = "United States",
                    Tag = "USA",
                    Tier = NationTier.GreatPower,
                    Government = GovernmentType.PresidentialRepublic,
                    CapitalProvinceId = "washington_dc",
                    MapColor = new Color("#5a6e8a"),   // Muted slate blue
                    Population = 331_000_000,
                    Treasury = 23_400_000_000_000.0,
                    FlagPath = "res://assets/flags/usa.png",
                    HistoricalAccuracyScore = 91.3,    // PLACEHOLDER
                    TopOfficials = new List<Official>  // PLACEHOLDER
                    {
                        new Official { Title = "President",          Name = "Joseph Biden" },
                        new Official { Title = "Vice President",     Name = "Kamala Harris" },
                        new Official { Title = "Secretary of State", Name = "Antony Blinken" }
                    }
                },

                // ── Russia ─────────────────────────────────────────────────────────────
                new Nation
                {
                    Id = "RUS",
                    Name = "Russian Federation",
                    ShortName = "Russia",
                    Tag = "RUS",
                    Tier = NationTier.GreatPower,
                    Government = GovernmentType.Dictatorship,
                    CapitalProvinceId = "moscow",
                    MapColor = new Color("#8a4a4a"),   // Muted brick red
                    Population = 144_000_000,
                    Treasury = 1_800_000_000_000.0,
                    FlagPath = "res://assets/flags/russia.png",
                    HistoricalAccuracyScore = 72.0,    // PLACEHOLDER
                    TopOfficials = new List<Official>  // PLACEHOLDER
                    {
                        new Official { Title = "President",       Name = "Vladimir Putin" },
                        new Official { Title = "Prime Minister",  Name = "Mikhail Mishustin" },
                        new Official { Title = "Foreign Minister",Name = "Sergei Lavrov" }
                    }
                },

                // ── China ──────────────────────────────────────────────────────────────
                new Nation
                {
                    Id = "CHN",
                    Name = "People's Republic of China",
                    ShortName = "China",
                    Tag = "CHN",
                    Tier = NationTier.GreatPower,
                    Government = GovernmentType.CommunistState,
                    CapitalProvinceId = "beijing",
                    MapColor = new Color("#8a3a3a"),   // Deep muted crimson
                    Population = 1_412_000_000,
                    Treasury = 17_700_000_000_000.0,
                    FlagPath = "res://assets/flags/china.png",
                    HistoricalAccuracyScore = 68.5,    // PLACEHOLDER
                    TopOfficials = new List<Official>  // PLACEHOLDER
                    {
                        new Official { Title = "General Secretary", Name = "Xi Jinping" },
                        new Official { Title = "Premier",           Name = "Li Qiang" },
                        new Official { Title = "Foreign Minister",  Name = "Wang Yi" }
                    }
                },

                // ── Germany ────────────────────────────────────────────────────────────
                new Nation
                {
                    Id = "DEU",
                    Name = "Federal Republic of Germany",
                    ShortName = "Germany",
                    Tag = "DEU",
                    Tier = NationTier.RegionalPower,
                    Government = GovernmentType.ParliamentaryRepublic,
                    CapitalProvinceId = "berlin",
                    MapColor = new Color("#6a7a5a"),   // Muted olive green
                    Population = 83_000_000,
                    Treasury = 4_200_000_000_000.0,
                    FlagPath = "res://assets/flags/germany.png",
                    HistoricalAccuracyScore = 85.7,    // PLACEHOLDER
                    TopOfficials = new List<Official>  // PLACEHOLDER
                    {
                        new Official { Title = "Chancellor",       Name = "Olaf Scholz" },
                        new Official { Title = "President",        Name = "Frank-Walter Steinmeier" },
                        new Official { Title = "Foreign Minister", Name = "Annalena Baerbock" }
                    }
                },

                // ── Brazil ─────────────────────────────────────────────────────────────
                new Nation
                {
                    Id = "BRA",
                    Name = "Federative Republic of Brazil",
                    ShortName = "Brazil",
                    Tag = "BRA",
                    Tier = NationTier.RegionalPower,
                    Government = GovernmentType.PresidentialRepublic,
                    CapitalProvinceId = "brasilia",
                    MapColor = new Color("#5a8a5a"),   // Muted forest green
                    Population = 215_000_000,
                    Treasury = 1_600_000_000_000.0,
                    FlagPath = "res://assets/flags/brazil.png",
                    HistoricalAccuracyScore = 78.2,    // PLACEHOLDER
                    TopOfficials = new List<Official>  // PLACEHOLDER
                    {
                        new Official { Title = "President",       Name = "Luiz Inácio Lula da Silva" },
                        new Official { Title = "Vice President",  Name = "Geraldo Alckmin" },
                        new Official { Title = "Foreign Minister",Name = "Mauro Vieira" }
                    }
                },

                // ── India ──────────────────────────────────────────────────────────────
                new Nation
                {
                    Id = "IND",
                    Name = "Republic of India",
                    ShortName = "India",
                    Tag = "IND",
                    Tier = NationTier.RegionalPower,
                    Government = GovernmentType.ParliamentaryRepublic,
                    CapitalProvinceId = "new_delhi",
                    MapColor = new Color("#8a7a4a"),   // Muted sandy gold
                    Population = 1_428_000_000,
                    Treasury = 3_400_000_000_000.0,
                    FlagPath = "res://assets/flags/india.png",
                    HistoricalAccuracyScore = 80.1,    // PLACEHOLDER
                    TopOfficials = new List<Official>  // PLACEHOLDER
                    {
                        new Official { Title = "Prime Minister",  Name = "Narendra Modi" },
                        new Official { Title = "President",       Name = "Droupadi Murmu" },
                        new Official { Title = "Foreign Minister",Name = "S. Jaishankar" }
                    }
                },

                // ── United Kingdom ─────────────────────────────────────────────────────
                new Nation
                {
                    Id = "GBR",
                    Name = "United Kingdom of Great Britain",
                    ShortName = "United Kingdom",
                    Tag = "GBR",
                    Tier = NationTier.RegionalPower,
                    Government = GovernmentType.Monarchy,
                    CapitalProvinceId = "london",
                    MapColor = new Color("#4a6a8a"),   // Muted steel blue
                    Population = 67_000_000,
                    Treasury = 3_100_000_000_000.0,
                    FlagPath = "res://assets/flags/uk.png",
                    HistoricalAccuracyScore = 83.4,    // PLACEHOLDER
                    TopOfficials = new List<Official>  // PLACEHOLDER
                    {
                        new Official { Title = "Prime Minister",    Name = "Rishi Sunak" },
                        new Official { Title = "Monarch",           Name = "King Charles III" },
                        new Official { Title = "Foreign Secretary", Name = "David Cameron" }
                    }
                },

                // ── France ─────────────────────────────────────────────────────────────
                new Nation
                {
                    Id = "FRA",
                    Name = "French Republic",
                    ShortName = "France",
                    Tag = "FRA",
                    Tier = NationTier.RegionalPower,
                    Government = GovernmentType.PresidentialRepublic,
                    CapitalProvinceId = "paris",
                    MapColor = new Color("#6a5a8a"),   // Muted violet
                    Population = 68_000_000,
                    Treasury = 2_900_000_000_000.0,
                    FlagPath = "res://assets/flags/france.png",
                    HistoricalAccuracyScore = 82.6,    // PLACEHOLDER
                    TopOfficials = new List<Official>  // PLACEHOLDER
                    {
                        new Official { Title = "President",        Name = "Emmanuel Macron" },
                        new Official { Title = "Prime Minister",   Name = "Gabriel Attal" },
                        new Official { Title = "Foreign Minister", Name = "Stéphane Séjourné" }
                    }
                }
            };
        }
    }
}
