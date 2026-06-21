using System.Collections.Generic;

namespace EmpiresOfHistoryV2.Simulation;

/// <summary>
/// Orchestrates save/load of all ISimulationSerializer systems.
/// Called by SaveSystem to persist full simulation state.
/// Phase 4.75 Finalization — wired into SaveSystem in Phase 5.
/// </summary>
public class SimulationSaveManager
{
    private readonly SimulationManager _simulationManager;

    public SimulationSaveManager(SimulationManager simulationManager)
    {
        _simulationManager = simulationManager;
    }

    /// <summary>
    /// Serializes all ISimulationSerializer systems.
    /// Returns a dictionary of systemId → serialized JSON string.
    /// </summary>
    public Dictionary<string, string> SaveAll()
    {
        return _simulationManager.Save();
    }

    /// <summary>
    /// Deserializes all ISimulationSerializer systems from a saved dictionary.
    /// Missing keys are silently skipped (system uses default state).
    /// </summary>
    public void LoadAll(Dictionary<string, string> data)
    {
        _simulationManager.Load(data);
    }
}
