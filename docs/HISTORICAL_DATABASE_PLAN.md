# Historical Database Plan

## Overview

All historical content for Empires of History is stored in JSON databases. This document defines the schema and structure for each historical data type.

## Database Files

```
data/
├── provinces/
│   ├── schema.json
│   └── provinces.json (5,000+ provinces)
├── nations/
│   ├── schema.json
│   └── nations.json (250+ nations)
├── leaders/
│   ├── schema.json
│   └── leaders.json
├── events/
│   ├── schema.json
│   ├── events_8300_1000bc.json
│   ├── events_1000bc_1ad.json
│   ├── events_1ad_1500ad.json
│   ├── events_1500ad_1800ad.json
│   └── events_1800ad_2100ad.json
├── technologies/
│   ├── schema.json
│   └── technologies.json
├── governments/
│   ├── schema.json
│   └── governments.json
├── religions/
│   ├── schema.json
│   └── religions.json
├── history/
│   ├── province_history.json
│   ├── border_history.json
│   └── nation_history.json
└── config/
    ├── game.json
    └── balance.json
```

## 1. Provinces Schema

### provinces.json

```json
{
  "schema_version": "1.0.0",
  "provinces": [
    {
      "id": "prov_egypt_nile_delta",
      "name": "Nile Delta",
      "display_name": "Nile Delta",
      "description": "The fertile delta region of the Nile River",
      "terrain": "plains",
      "coordinates": {
        "x": 512.5,
        "y": 384.2,
        "latitude": 31.0,
        "longitude": 31.3
      },
      "shape": "polygon",
      "vertices": [[512.5, 384.2], [520.1, 390.0], [515.3, 395.1]],
      "area_sqkm": 24000,
      "initial_population": 2500000,
      "population_density": 104.2,
      "development_level": 0.8,
      "capital": false,
      "cultural_groups": [
        {
          "id": "cult_egyptian",
          "percentage": 0.85
        },
        {
          "id": "cult_greek",
          "percentage": 0.15
        }
      ],
      "religions": [
        {
          "id": "rel_egyptian_polytheism",
          "percentage": 0.90,
          "established_year": -5000
        },
        {
          "id": "rel_christianity",
          "percentage": 0.10,
          "established_year": 50
        }
      ],
      "resources": [
        {
          "id": "res_wheat",
          "type": "agricultural",
          "quantity": 1000
        },
        {
          "id": "res_papyrus",
          "type": "unique",
          "quantity": 100
        }
      ],
      "neighbors": [
        "prov_egypt_upper_egypt",
        "prov_egypt_memphis",
        "prov_mediterranean_coast"
      ],
      "climate": "arid",
      "water_access": "river",
      "strategic_value": 0.9,
      "metadata": {
        "historical_names": ["Lower Egypt", "Kemet Lower"],
        "first_settled": -5500,
        "notes": "Most productive agricultural region"
      }
    }
  ]
}
```

### Province Schema C# Model

```csharp
public class ProvinceData
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public string Terrain { get; set; }
    public Coordinates Coordinates { get; set; }
    public string Shape { get; set; }
    public Vector2[] Vertices { get; set; }
    public float AreaSqKm { get; set; }
    public int InitialPopulation { get; set; }
    public float PopulationDensity { get; set; }
    public float DevelopmentLevel { get; set; }
    public bool Capital { get; set; }
    public List<CulturalGroup> CulturalGroups { get; set; }
    public List<ReligionData> Religions { get; set; }
    public List<ResourceData> Resources { get; set; }
    public List<string> Neighbors { get; set; }
    public string Climate { get; set; }
    public string WaterAccess { get; set; }
    public float StrategicValue { get; set; }
    public Dictionary<string, object> Metadata { get; set; }
    
    // Future expansion:
    // - Historical name variants
    // - Wonder locations
    // - Archaeological sites
    // - Trade hub status
    // - Military fortification points
}

public class Coordinates
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
}

public class CulturalGroup
{
    public string Id { get; set; }
    public float Percentage { get; set; }
}

public class ReligionData
{
    public string Id { get; set; }
    public float Percentage { get; set; }
    public int EstablishedYear { get; set; }
}

public class ResourceData
{
    public string Id { get; set; }
    public string Type { get; set; } // agricultural, mineral, unique, luxury
    public int Quantity { get; set; }
}
```

## 2. Nations Schema

### nations.json

```json
{
  "schema_version": "1.0.0",
  "nations": [
    {
      "id": "nat_egypt_ptolemaic",
      "name": "Ptolemaic Egypt",
      "display_name": "Ptolemaic Egypt",
      "description": "The Hellenic kingdom established by Ptolemy I after Alexander's conquest",
      "color": "#8B4513",
      "color_hex": "8B4513",
      "government_type": "monarchy",
      "capital_province_id": "prov_egypt_alexandria",
      "capital_name": "Alexandria",
      "government_succession_type": "hereditary",
      "primary_culture": "cult_greek",
      "secondary_cultures": ["cult_egyptian"],
      "primary_religion": "rel_hellenism",
      "secondary_religions": ["rel_egyptian_polytheism"],
      "founding_year": -305,
      "dissolution_year": -30,
      "status_at_present": "dissolved",
      "tags": ["hellenistic", "mediterranean", "african"],
      "tier": "major_power",
      "historical_period": "hellenistic",
      "diplomatic_traits": [
        {
          "id": "trait_merchant_republic",
          "influence": 0.7
        },
        {
          "id": "trait_cultural_hub",
          "influence": 0.9
        }
      ],
      "initial_tech_level": 4,
      "ai_personality": "balanced_aggressive",
      "difficulty_modifier": 1.0,
      "victory_type": "cultural",
      "metadata": {
        "historical_names": ["Egypt", "Aigyptos"],
        "alternate_names": ["Hellenistic Egypt", "Kingdom of Egypt"],
        "notes": "Major Hellenistic power controlling Mediterranean trade"
      }
    }
  ]
}
```

### Nation Schema C# Model

```csharp
public class NationData
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public string GovernmentType { get; set; }
    public string CapitalProvinceId { get; set; }
    public string CapitalName { get; set; }
    public string GovernmentSuccessionType { get; set; }
    public string PrimaryCulture { get; set; }
    public List<string> SecondaryCultures { get; set; }
    public string PrimaryReligion { get; set; }
    public List<string> SecondaryReligions { get; set; }
    public int FoundingYear { get; set; }
    public int? DissolutionYear { get; set; }
    public string StatusAtPresent { get; set; }
    public List<string> Tags { get; set; }
    public string Tier { get; set; } // major_power, regional_power, minor_nation
    public string HistoricalPeriod { get; set; }
    public List<DiplomaticTrait> DiplomaticTraits { get; set; }
    public int InitialTechLevel { get; set; }
    public string AIPersonality { get; set; }
    public float DifficultyModifier { get; set; }
    public string VictoryType { get; set; }
    public Dictionary<string, object> Metadata { get; set; }
    
    // Future expansion:
    // - Religious government variants
    // - Federated vs centralized government
    // - Economic system type
    // - Military doctrine
    // - Trade agreements database
    // - Rebellion/independence movements
}

public class DiplomaticTrait
{
    public string Id { get; set; }
    public float Influence { get; set; }
}
```

## 3. Leaders Schema

### leaders.json

```json
{
  "schema_version": "1.0.0",
  "leaders": [
    {
      "id": "lead_cleopatra_vii",
      "name": "Cleopatra VII Philopator",
      "title": "Pharaoh of Egypt",
      "nation_id": "nat_egypt_ptolemaic",
      "birth_year": -69,
      "death_year": -30,
      "reign_start": -51,
      "reign_end": -30,
      "gender": "female",
      "age_assumption": 21,
      "traits": [
        {
          "id": "trait_charismatic",
          "level": 3
        },
        {
          "id": "trait_diplomatic",
          "level": 4
        },
        {
          "id": "trait_shrewd",
          "level": 5
        }
      ],
      "abilities": [
        {
          "id": "ability_diplomatic_marriages",
          "effect": "Can form alliances through marriage"
        }
      ],
      "personality_archetype": "cunning_diplomat",
      "loyalty": 0.85,
      "competence": 0.9,
      "ambition": 0.95,
      "religious_zeal": 0.6,
      "relation_modifier": {
        "other_leaders": {},
        "cultures": {"cult_greek": 0.1, "cult_egyptian": 0.2}
      },
      "historical_events": [
        {
          "event_id": "evt_cleopatra_rome_alliance",
          "year": -48
        }
      ],
      "metadata": {
        "historical_significance": "very_high",
        "alternative_spellings": ["Kleopatra"],
        "cultural_icon": true
      }
    }
  ]
}
```

### Leader Schema C# Model

```csharp
public class LeaderData
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string NationId { get; set; }
    public int BirthYear { get; set; }
    public int? DeathYear { get; set; }
    public int ReignStart { get; set; }
    public int? ReignEnd { get; set; }
    public string Gender { get; set; }
    public int AgeAssumption { get; set; }
    public List<LeaderTrait> Traits { get; set; }
    public List<LeaderAbility> Abilities { get; set; }
    public string PersonalityArchetype { get; set; }
    public float Loyalty { get; set; }
    public float Competence { get; set; }
    public float Ambition { get; set; }
    public float ReligiousZeal { get; set; }
    public RelationModifiers RelationModifier { get; set; }
    public List<HistoricalEvent> HistoricalEvents { get; set; }
    public Dictionary<string, object> Metadata { get; set; }
    
    // Future expansion:
    // - Personal relationships
    // - Secret alliances
    // - Rumors and scandals
    // - Succession lines
    // - Military experience
    // - Economic acumen scores
}

public class LeaderTrait
{
    public string Id { get; set; }
    public int Level { get; set; } // 1-5
}

public class LeaderAbility
{
    public string Id { get; set; }
    public string Effect { get; set; }
}

public class RelationModifiers
{
    public Dictionary<string, float> OtherLeaders { get; set; }
    public Dictionary<string, float> Cultures { get; set; }
}
```

## 4. Events Schema

### events.json

```json
{
  "schema_version": "1.0.0",
  "events": [
    {
      "id": "evt_nile_flooding_8300bc",
      "name": "Annual Nile Flooding",
      "type": "natural_disaster",
      "category": "environmental",
      "severity": "low",
      "year": -8300,
      "turn": 0,
      "month": "July",
      "provinces_affected": ["prov_egypt_upper_egypt", "prov_egypt_nile_delta"],
      "nations_affected": ["nat_egypt_ptolemaic"],
      "effects": [
        {
          "type": "population_change",
          "target": "provinces",
          "value": 0.05,
          "description": "5% population growth due to fertile soil"
        }
      ],
      "historical_accuracy": 0.95,
      "is_recurring": true,
      "recurrence_pattern": "annual",
      "trigger_conditions": [],
      "prevent_conditions": [],
      "description": "The annual flooding of the Nile brings fertile soil to Egypt",
      "flavor_text": "The waters of the Nile rise once more, bringing life-giving silt to the lands of Egypt.",
      "options": [
        {
          "id": "opt_prepare_irrigation",
          "text": "Prepare irrigation systems",
          "cost": 50
        }
      ],
      "metadata": {
        "source": "historical_record",
        "confidence": 0.99
      }
    },
    {
      "id": "evt_roman_conquest_egypt",
      "name": "Roman Conquest of Egypt",
      "type": "military_conquest",
      "category": "political",
      "severity": "critical",
      "year": -30,
      "turn": 30960,
      "month": "August",
      "provinces_affected": "all_egyptian",
      "nations_affected": ["nat_egypt_ptolemaic", "nat_rome_republic"],
      "effects": [
        {
          "type": "nation_dissolution",
          "target": "nat_egypt_ptolemaic",
          "value": 1.0
        },
        {
          "type": "ownership_transfer",
          "target": "all_egyptian_provinces",
          "from_nation": "nat_egypt_ptolemaic",
          "to_nation": "nat_rome_republic"
        }
      ],
      "trigger_conditions": [
        {
          "type": "military_defeat",
          "nation": "nat_egypt_ptolemaic",
          "min_strength_ratio": 0.3
        }
      ],
      "prevent_conditions": [
        {
          "type": "alliance_active",
          "nation": "nat_egypt_ptolemaic",
          "ally": "nat_rome_republic"
        }
      ],
      "description": "The Roman Republic defeats Ptolemaic Egypt",
      "flavor_text": "The armies of Rome sweep across Egypt, ending the Ptolemaic dynasty. The land becomes a Roman province.",
      "mandatory": false,
      "can_be_prevented": true,
      "metadata": {
        "source": "historical_fact",
        "confidence": 1.0,
        "related_leaders": ["lead_cleopatra_vii", "lead_caesar", "lead_octavian"]
      }
    }
  ]
}
```

### Event Schema C# Model

```csharp
public class EventData
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; } // military_conquest, natural_disaster, etc.
    public string Category { get; set; }
    public string Severity { get; set; } // low, medium, high, critical
    public int Year { get; set; }
    public int Turn { get; set; }
    public string Month { get; set; }
    public List<string> ProvincesAffected { get; set; }
    public List<string> NationsAffected { get; set; }
    public List<EventEffect> Effects { get; set; }
    public float HistoricalAccuracy { get; set; }
    public bool IsRecurring { get; set; }
    public string RecurrencePattern { get; set; }
    public List<EventCondition> TriggerConditions { get; set; }
    public List<EventCondition> PreventConditions { get; set; }
    public string Description { get; set; }
    public string FlavorText { get; set; }
    public List<EventOption> Options { get; set; }
    public Dictionary<string, object> Metadata { get; set; }
    
    // Future expansion:
    // - Event chains and branching
    // - Random event generation
    // - Custom event creation
    // - Event modding support
}

public class EventEffect
{
    public string Type { get; set; }
    public string Target { get; set; }
    public float Value { get; set; }
    public string Description { get; set; }
}

public class EventCondition
{
    public string Type { get; set; }
    public string Target { get; set; }
    public Dictionary<string, object> Parameters { get; set; }
}

public class EventOption
{
    public string Id { get; set; }
    public string Text { get; set; }
    public int Cost { get; set; }
}
```

## 5. Technologies Schema

### technologies.json

```json
{
  "schema_version": "1.0.0",
  "technologies": [
    {
      "id": "tech_agriculture",
      "name": "Agriculture",
      "description": "Cultivation of crops and domestication of animals",
      "era": "ancient",
      "discovery_year": -8000,
      "discovery_nation": "nat_generic_tribal",
      "category": "food_production",
      "prerequisites": [],
      "unlocks_techs": ["tech_irrigation", "tech_animal_husbandry"],
      "unlocks_buildings": ["building_farm", "building_granary"],
      "unlocks_units": [],
      "effects": [
        {
          "type": "population_growth",
          "value": 0.1
        },
        {
          "type": "food_production",
          "value": 2.0
        }
      ],
      "cost": 0,
      "research_time_years": 0,
      "historical_accuracy": 0.9,
      "metadata": {
        "historical_names": ["Neolithic Revolution"],
        "cultural_significance": "very_high"
      }
    }
  ]
}
```

### Technology Schema C# Model

```csharp
public class TechnologyData
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Era { get; set; }
    public int DiscoveryYear { get; set; }
    public string DiscoveryNation { get; set; }
    public string Category { get; set; }
    public List<string> Prerequisites { get; set; }
    public List<string> UnlocksTechs { get; set; }
    public List<string> UnlocksBuildings { get; set; }
    public List<string> UnlocksUnits { get; set; }
    public List<TechEffect> Effects { get; set; }
    public int Cost { get; set; }
    public int ResearchTimeYears { get; set; }
    public float HistoricalAccuracy { get; set; }
    public Dictionary<string, object> Metadata { get; set; }
    
    // Future expansion:
    // - Multiple tech trees (Eastern, Western, African)
    // - Technology degradation in dark ages
    // - Lost technologies
    // - Espionage/technology theft
}
```

## 6. Governments Schema

### governments.json

```json
{
  "schema_version": "1.0.0",
  "governments": [
    {
      "id": "govt_absolute_monarchy",
      "name": "Absolute Monarchy",
      "description": "Single ruler with absolute power",
      "government_type": "monarchy",
      "succession_type": "hereditary",
      "stability_base": 0.7,
      "loyalty_base": 0.8,
      "corruption_base": 0.3,
      "effectiveness_base": 0.85,
      "available_from_year": -1000,
      "bonuses": {
        "military_production": 0.1,
        "culture": -0.1
      },
      "penalties": {
        "happiness": -0.15,
        "innovation": -0.2
      },
      "rebellion_risk": 0.3,
      "transition_requirements": [
        {
          "from_government": "govt_tribal_council",
          "cost": 100,
          "time_years": 50
        }
      ],
      "metadata": {
        "era_typical": ["ancient", "medieval", "early_modern"]
      }
    }
  ]
}
```

## 7. Religions Schema

### religions.json

```json
{
  "schema_version": "1.0.0",
  "religions": [
    {
      "id": "rel_christianity",
      "name": "Christianity",
      "description": "The Abrahamic faith centered on Jesus Christ",
      "color": "#FFD700",
      "founding_year": 0,
      "founding_location": "prov_judea",
      "founding_leader": "hist_jesus_nazareth",
      "religious_type": "monotheistic",
      "holy_sites": [
        {
          "id": "hs_jerusalem",
          "province_id": "prov_judea",
          "name": "Jerusalem",
          "importance": 0.95
        }
      ],
      "spreads_to_cultures": [],
      "spreads_via": "missionary",
      "spread_speed": 0.3,
      "tolerance_level": 0.2,
      "conversion_difficulty": 0.6,
      "warfare_attitude": "defensive",
      "schism_tendency": 0.5,
      "effects": {
        "culture": 0.2,
        "happiness": 0.1,
        "innovation": -0.1
      },
      "conflicts_with": [
        "rel_roman_polytheism",
        "rel_judaism"
      ],
      "compatible_with": [],
      "metadata": {
        "historical_accuracy": 0.95,
        "major_variants": ["eastern_orthodox", "roman_catholic", "protestant"]
      }
    }
  ]
}
```

## 8. Province History Schema

### province_history.json

```json
{
  "schema_version": "1.0.0",
  "province_history": [
    {
      "province_id": "prov_egypt_nile_delta",
      "history": [
        {
          "year": -8300,
          "turn": 0,
          "owner_nation": null,
          "status": "tribal",
          "population": 50000,
          "development_level": 0.1,
          "primary_culture": "cult_egyptian",
          "primary_religion": "rel_egyptian_polytheism"
        },
        {
          "year": -3100,
          "turn": 1664000,
          "owner_nation": "nat_egypt_old_kingdom",
          "status": "owned",
          "population": 2500000,
          "development_level": 0.7,
          "primary_culture": "cult_egyptian",
          "primary_religion": "rel_egyptian_polytheism",
          "event_id": "evt_unification_egypt"
        }
      ]
    }
  ]
}
```

## 9. Border History Schema

### border_history.json

```json
{
  "schema_version": "1.0.0",
  "border_history": [
    {
      "id": "border_egypt_nubia",
      "province1_id": "prov_egypt_upper_egypt",
      "province2_id": "prov_nubia_lower_nubia",
      "history": [
        {
          "year": -8300,
          "turn": 0,
          "status": "tribal_boundary",
          "tension_level": 0.2,
          "type": "natural_border"
        },
        {
          "year": -3100,
          "turn": 1664000,
          "status": "empire_boundary",
          "nation1_id": "nat_egypt_old_kingdom",
          "nation2_id": "nat_nubia_kingdom",
          "tension_level": 0.5,
          "type": "contested_border",
          "fortifications": ["fort_aswan"]
        }
      ]
    }
  ]
}
```

## Data Loading Strategy

### Async Loading

```csharp
public interface IHistoricalDataLoader
{
    Task LoadAllHistoricalDataAsync();
    Task LoadProvinceDataAsync();
    Task LoadNationDataAsync();
    Task LoadLeadersAsync();
    Task LoadEventsAsync(int startYear, int endYear);
    Task LoadTechnologiesAsync();
}

public class HistoricalDataLoader : IHistoricalDataLoader
{
    private DataLoader _dataLoader;
    private ContentDatabase _contentDb;
    private Validator _validator;
    
    public async Task LoadAllHistoricalDataAsync()
    {
        var tasks = new[]
        {
            LoadProvinceDataAsync(),
            LoadNationDataAsync(),
            LoadLeadersAsync(),
            LoadTechnologiesAsync()
        };
        
        await Task.WhenAll(tasks);
        
        GD.Print("All historical data loaded");
    }
    
    public async Task LoadProvinceDataAsync()
    {
        var data = await _dataLoader.LoadJsonAsync<ProvinceData>("data/provinces/provinces.json");
        var validationResult = _validator.Validate(data, PROVINCE_SCHEMA);
        
        if (!validationResult.Valid)
            throw new DataLoadException("Province data validation failed");
        
        _contentDb.LoadProvinces(data);
    }
    
    // Similar methods for other data types
    
    // Future expansion:
    // - Streaming large datasets
    // - Lazy loading by region
    // - Hot reloading for development
    // - Data compression
}
```

## Future Expansion Notes

- **Historical Branches**: Support alternate history data tracks
- **Dynamic Events**: AI-generated events based on game state
- **Procedural Content**: Generate historical data for missing time periods
- **Modding Support**: Allow users to add custom historical data
- **Real World Integration**: Import real historical data from sources
- **Wikipedia Integration**: Fetch historical information dynamically
