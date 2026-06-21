namespace EmpiresOfHistoryV2.Simulation.Systems;

/// <summary>
/// Phase 5 stub — Historical Timeline System.
/// Permanent home for all historical accuracy calculations.
/// Implementation added in Phase 5.
/// </summary>
public class TimelineSystem : ISimulationTick, ISimulationSerializer, ISimulationProvider
{
    public string SystemId => "TimelineSystem";
    public int TickOrder => SimulationTickOrder.Timeline;
    public bool IsEnabled => false; // Disabled until Phase 5

    // ── ISimulationSystem ────────────────────────────────────────────────────
    public void Initialize(SimulationContext context) { /* Phase 5 */ }
    public void Dispose() { /* Phase 5 */ }

    // ── ISimulationTick ──────────────────────────────────────────────────────
    public void Tick(SimulationContext context) { /* Phase 5 */ }

    // ── Historical Accuracy API (Phase 5) ───────────────────────────────────
    /// <summary>STUB — Phase 5. Update the historical timeline for the current turn.</summary>
    public void UpdateTimeline(SimulationContext context) { /* Phase 5 */ }

    /// <summary>STUB — Phase 5. Calculate historical accuracy score for a nation (0–100).</summary>
    public double CalculateHistoricalAccuracy(string nationId) => 0.0; // Phase 5

    /// <summary>STUB — Phase 5. Calculate how much historical momentum is driving current events.</summary>
    public double CalculateHistoricalMomentum(string nationId) => 0.0; // Phase 5

    /// <summary>STUB — Phase 5. Calculate divergence from the historical timeline.</summary>
    public double CalculateHistoricalDivergence(string nationId) => 0.0; // Phase 5

    // ── ISimulationSerializer ────────────────────────────────────────────────
    public string Serialize() => "{}"; // Phase 5
    public void Deserialize(string json) { /* Phase 5 */ }

    // ── ISimulationProvider ──────────────────────────────────────────────────
    public string? GetValue(string key) => null; // Phase 5
    public System.Collections.Generic.IReadOnlyList<string> GetExportedKeys() =>
    [
        "historical_accuracy",   // double 0–100
        "historical_momentum",   // double
        "historical_divergence", // double
    ];
}
