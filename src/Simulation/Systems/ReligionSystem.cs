namespace EmpiresOfHistoryV2.Simulation.Systems;

/// <summary>
/// Phase 8 stub — Religion System.
/// Implementation added in Phase 8.
/// </summary>
public class ReligionSystem : ISimulationTick, ISimulationSerializer, ISimulationProvider
{
    public string SystemId => "ReligionSystem";
    public int TickOrder => SimulationTickOrder.Religion;
    public bool IsEnabled => false; // Disabled until Phase 8

    // ── ISimulationSystem ────────────────────────────────────────────────────
    public void Initialize(SimulationContext context) { /* Phase 8 */ }
    public void Dispose() { /* Phase 8 */ }

    // ── ISimulationTick ──────────────────────────────────────────────────────
    public void Tick(SimulationContext context) { /* Phase 8 */ }

    // ── ISimulationSerializer ────────────────────────────────────────────────
    public string Serialize() => "{}"; // Phase 8
    public void Deserialize(string json) { /* Phase 8 */ }

    // ── ISimulationProvider ──────────────────────────────────────────────────
    public string? GetValue(string key) => null; // Phase 8
    public System.Collections.Generic.IReadOnlyList<string> GetExportedKeys() => []; // Phase 8
}
