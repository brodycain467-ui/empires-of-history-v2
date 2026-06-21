namespace EmpiresOfHistoryV2.Simulation.Systems;

/// <summary>
/// Phase 7 stub — Government System.
/// Implementation added in Phase 7.
/// </summary>
public class GovernmentSystem : ISimulationTick, ISimulationSerializer, ISimulationProvider
{
    public string SystemId => "GovernmentSystem";
    public int TickOrder => SimulationTickOrder.Government;
    public bool IsEnabled => false; // Disabled until Phase 7

    // ── ISimulationSystem ────────────────────────────────────────────────────
    public void Initialize(SimulationContext context) { /* Phase 7 */ }
    public void Dispose() { /* Phase 7 */ }

    // ── ISimulationTick ──────────────────────────────────────────────────────
    public void Tick(SimulationContext context) { /* Phase 7 */ }

    // ── ISimulationSerializer ────────────────────────────────────────────────
    public string Serialize() => "{}"; // Phase 7
    public void Deserialize(string json) { /* Phase 7 */ }

    // ── ISimulationProvider ──────────────────────────────────────────────────
    public string? GetValue(string key) => null; // Phase 7
    public System.Collections.Generic.IReadOnlyList<string> GetExportedKeys() => []; // Phase 7
}
