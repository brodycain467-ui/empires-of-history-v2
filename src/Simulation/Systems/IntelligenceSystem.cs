namespace EmpiresOfHistoryV2.Simulation.Systems;

/// <summary>
/// Phase 9 stub — Intelligence System.
/// Implementation added in Phase 9.
/// </summary>
public class IntelligenceSystem : ISimulationTick, ISimulationSerializer, ISimulationProvider
{
    public string SystemId => "IntelligenceSystem";
    public int TickOrder => SimulationTickOrder.Intelligence;
    public bool IsEnabled => false; // Disabled until Phase 9

    // ── ISimulationSystem ────────────────────────────────────────────────────
    public void Initialize(SimulationContext context) { /* Phase 9 */ }
    public void Dispose() { /* Phase 9 */ }

    // ── ISimulationTick ──────────────────────────────────────────────────────
    public void Tick(SimulationContext context) { /* Phase 9 */ }

    // ── ISimulationSerializer ────────────────────────────────────────────────
    public string Serialize() => "{}"; // Phase 9
    public void Deserialize(string json) { /* Phase 9 */ }

    // ── ISimulationProvider ──────────────────────────────────────────────────
    public string? GetValue(string key) => null; // Phase 9
    public System.Collections.Generic.IReadOnlyList<string> GetExportedKeys() => []; // Phase 9
}
