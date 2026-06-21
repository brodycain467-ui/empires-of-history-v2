using System;

namespace EmpiresOfHistoryV2.Simulation;

/// <summary>
/// Base contract for every simulation system.
/// All Phase 5+ systems implement this interface and register with SimulationManager.
/// </summary>
public interface ISimulationSystem
{
    /// <summary>Unique identifier for this system, e.g. "EconomySystem".</summary>
    string SystemId { get; }

    /// <summary>Execution priority within the turn order (lower = earlier).</summary>
    int TickOrder { get; }

    /// <summary>Whether this system is currently active.</summary>
    bool IsEnabled { get; }

    /// <summary>Called once on game start / new game.</summary>
    void Initialize(SimulationContext context);

    /// <summary>Called once when the system is removed.</summary>
    void Dispose();
}
