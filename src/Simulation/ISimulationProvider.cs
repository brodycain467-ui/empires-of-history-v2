namespace EmpiresOfHistoryV2.Simulation;

/// <summary>
/// Implemented by systems that expose queryable state to other systems.
/// Systems read data through providers — never by direct system references.
/// </summary>
public interface ISimulationProvider : ISimulationSystem
{
    /// <summary>
    /// Returns a named value. Callers use string keys; systems define their own key contracts.
    /// Returns null if key not found.
    /// </summary>
    string? GetValue(string key);

    /// <summary>
    /// Returns all exported key names for documentation / GIA consumption.
    /// </summary>
    System.Collections.Generic.IReadOnlyList<string> GetExportedKeys();
}
