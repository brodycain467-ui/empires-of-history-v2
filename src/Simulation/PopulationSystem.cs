using System;
using System.Collections.Generic;
using EmpiresOfHistoryV2.Providers;
using EmpiresOfHistoryV2.Simulation;

namespace EmpiresOfHistoryV2.Simulation.Foundation;

public interface IPopulationProvider
{
    PopulationStatistics GetPopulationStatistics();
}

public readonly struct PopulationStatistics
{
    public float BirthRate { get; init; }
    public float DeathRate { get; init; }
    public float MigrationRate { get; init; }
    public long PopulationTotal { get; init; }
}

public class PopulationSystem : ISimulationTick, ISimulationProvider, ISystemProvider, ISystemTick, ISystemStatistics, ISystemConfiguration, IPopulationProvider
{
    public string SystemId => "PopulationSystem";
    public int TickOrder => SimulationTickOrder.Population;
    public bool IsEnabled => true;

    public float birth_rate { get; private set; } = 0.012f;
    public float death_rate { get; private set; } = 0.008f;
    public float migration_rate { get; private set; } = 0.001f;
    public long population_total { get; private set; } = 1_000_000;

    public void Initialize(SimulationContext context) { }
    public void Dispose() { }

    public void PopulationTick(float deltaTime)
    {
        var netRate = (birth_rate - death_rate + migration_rate) * deltaTime;
        var updated = (long)Math.Round(population_total * (1f + netRate));
        population_total = Math.Max(0, updated);
    }

    public void Tick(SimulationContext context) => PopulationTick(1f);
    void ISystemTick.Tick(float deltaTime) => PopulationTick(deltaTime);

    public string? GetValue(string key) => key switch
    {
        "population_total" => population_total.ToString(),
        "birth_rate" => birth_rate.ToString(System.Globalization.CultureInfo.InvariantCulture),
        "death_rate" => death_rate.ToString(System.Globalization.CultureInfo.InvariantCulture),
        "migration_rate" => migration_rate.ToString(System.Globalization.CultureInfo.InvariantCulture),
        _ => null
    };

    public IReadOnlyList<string> GetExportedKeys() => ["population_total", "birth_rate", "death_rate", "migration_rate"];

    public object GetStatistics() => GetPopulationStatistics();

    public PopulationStatistics GetPopulationStatistics() => new()
    {
        BirthRate = birth_rate,
        DeathRate = death_rate,
        MigrationRate = migration_rate,
        PopulationTotal = population_total
    };

    public void Configure(IReadOnlyDictionary<string, string> settings)
    {
        if (settings.TryGetValue("birth_rate", out var birth) && float.TryParse(birth, out var birthRate))
            birth_rate = birthRate;
        if (settings.TryGetValue("death_rate", out var death) && float.TryParse(death, out var deathRate))
            death_rate = deathRate;
        if (settings.TryGetValue("migration_rate", out var migration) && float.TryParse(migration, out var migrationRate))
            migration_rate = migrationRate;
        if (settings.TryGetValue("population_total", out var total) && long.TryParse(total, out var parsedTotal))
            population_total = Math.Max(0, parsedTotal);
    }
}
