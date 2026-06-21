namespace EmpiresOfHistoryV2.Simulation.Systems;

/// <summary>
/// Phase 10 stub — GIA Advisor System.
/// Implementation added in Phase 10.
/// </summary>
public class GIAAdvisorSystem : ISimulationTick, ISimulationSerializer, ISimulationProvider
{
    public string SystemId => "GIAAdvisorSystem";
    public int TickOrder => SimulationTickOrder.GIAAdvisor;
    public bool IsEnabled => false; // Disabled until Phase 10

    // ── ISimulationSystem ────────────────────────────────────────────────────
    public void Initialize(SimulationContext context) { /* Phase 10 */ }
    public void Dispose() { /* Phase 10 */ }

    // ── ISimulationTick ──────────────────────────────────────────────────────
    public void Tick(SimulationContext context) { /* Phase 10 */ }

    // ── ISimulationSerializer ────────────────────────────────────────────────
    public string Serialize() => "{}"; // Phase 10
    public void Deserialize(string json) { /* Phase 10 */ }

    // ── ISimulationProvider ──────────────────────────────────────────────────
    public string? GetValue(string key) => null; // Phase 10
    public System.Collections.Generic.IReadOnlyList<string> GetExportedKeys() => []; // Phase 10
}
