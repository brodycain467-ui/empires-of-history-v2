namespace EmpiresOfHistoryV2.Simulation.Systems;

/// <summary>
/// Phase 6 stub — Economy System.
/// Implementation added in Phase 6.
/// </summary>
public class EconomySystem : ISimulationTick, ISimulationSerializer, ISimulationProvider
{
    public string SystemId => "EconomySystem";
    public int TickOrder => SimulationTickOrder.Economy;
    public bool IsEnabled => false; // Disabled until Phase 6

    // ── ISimulationSystem ────────────────────────────────────────────────────
    public void Initialize(SimulationContext context) { /* Phase 6 */ }
    public void Dispose() { /* Phase 6 */ }

    // ── ISimulationTick ──────────────────────────────────────────────────────
    public void Tick(SimulationContext context) { /* Phase 6 */ }

    // ── ISimulationSerializer ────────────────────────────────────────────────
    public string Serialize() => "{}"; // Phase 6
    public void Deserialize(string json) { /* Phase 6 */ }

    // ── ISimulationProvider ──────────────────────────────────────────────────
    public string? GetValue(string key) => null; // Phase 6
    public System.Collections.Generic.IReadOnlyList<string> GetExportedKeys() => []; // Phase 6
}
