namespace EmpiresOfHistoryV2.Simulation;

/// <summary>
/// Implemented by systems that persist state across saves.
/// </summary>
public interface ISimulationSerializer : ISimulationSystem
{
    /// <summary>Serialize system state to a JSON-compatible string.</summary>
    string Serialize();

    /// <summary>Restore system state from a previously serialized string.</summary>
    void Deserialize(string json);
}
