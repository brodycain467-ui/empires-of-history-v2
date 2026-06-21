using System;
using System.Collections.Generic;
using EmpiresOfHistoryV2.Providers;
using EmpiresOfHistoryV2.Simulation;

namespace EmpiresOfHistoryV2.Simulation.Foundation;

public interface ITreasuryProvider
{
    TreasuryStatistics GetTreasuryStatistics();
}

public readonly struct TreasuryStatistics
{
    public float Income { get; init; }
    public float Expenses { get; init; }
    public float TreasuryTotal { get; init; }
}

public class TreasurySystem : ISimulationTick, ISimulationProvider, ISystemProvider, ISystemTick, ISystemStatistics, ISystemConfiguration, ITreasuryProvider
{
    public string SystemId => "TreasurySystem";
    public int TickOrder => SimulationTickOrder.Economy;
    public bool IsEnabled => true;

    public float income { get; private set; } = 1000f;
    public float expenses { get; private set; } = 800f;
    public float treasury_total { get; private set; } = 100_000f;

    public void Initialize(SimulationContext context) { }
    public void Dispose() { }

    public void TreasuryTick(float deltaTime)
    {
        treasury_total = Math.Max(0f, treasury_total + ((income - expenses) * deltaTime));
    }

    public void Tick(SimulationContext context) => TreasuryTick(1f);
    void ISystemTick.Tick(float deltaTime) => TreasuryTick(deltaTime);

    public string? GetValue(string key) => key switch
    {
        "income" => income.ToString(System.Globalization.CultureInfo.InvariantCulture),
        "expenses" => expenses.ToString(System.Globalization.CultureInfo.InvariantCulture),
        "treasury_total" => treasury_total.ToString(System.Globalization.CultureInfo.InvariantCulture),
        _ => null
    };

    public IReadOnlyList<string> GetExportedKeys() => ["income", "expenses", "treasury_total"];

    public object GetStatistics() => GetTreasuryStatistics();

    public TreasuryStatistics GetTreasuryStatistics() => new()
    {
        Income = income,
        Expenses = expenses,
        TreasuryTotal = treasury_total
    };

    public void Configure(IReadOnlyDictionary<string, string> settings)
    {
        if (settings.TryGetValue("income", out var configuredIncome) && float.TryParse(configuredIncome, out var parsedIncome))
            income = parsedIncome;
        if (settings.TryGetValue("expenses", out var configuredExpenses) && float.TryParse(configuredExpenses, out var parsedExpenses))
            expenses = parsedExpenses;
        if (settings.TryGetValue("treasury_total", out var configuredTotal) && float.TryParse(configuredTotal, out var parsedTotal))
            treasury_total = Math.Max(0f, parsedTotal);
    }
}
