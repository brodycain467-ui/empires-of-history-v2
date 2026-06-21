namespace EmpiresOfHistoryV2.Simulation;

/// <summary>
/// Centralizes all ISimulationProvider query key constants.
/// Use these instead of magic strings when calling SimulationManager.QueryProvider().
/// </summary>
public static class SimulationProviderKeys
{
    // Timeline
    public const string TimelineHistoricalAccuracy = "historical_accuracy";
    public const string TimelineDivergence = "historical_divergence";
    public const string TimelineMomentum = "historical_momentum";

    // Population
    public const string PopulationTotal = "population_total";
    public const string PopulationGrowthRate = "population_growth_rate";

    // Economy
    public const string EconomyTreasury = "economy_treasury";
    public const string EconomyGdp = "economy_gdp";
    public const string EconomyGrowthRate = "economy_growth_rate";

    // Technology
    public const string TechnologyLevel = "technology_level";
    public const string TechnologyProgress = "technology_progress";

    // Government
    public const string GovernmentStability = "government_stability";
    public const string GovernmentLegitimacy = "government_legitimacy";

    // Military
    public const string MilitaryStrength = "military_strength";
    public const string MilitaryMorale = "military_morale";

    // Religion
    public const string ReligionDominant = "religion_dominant";
    public const string ReligionPiety = "religion_piety";

    // Intelligence
    public const string IntelligenceNetwork = "intelligence_network";

    // GIA
    public const string GiaAdvisoryScore = "gia_advisory_score";
}
