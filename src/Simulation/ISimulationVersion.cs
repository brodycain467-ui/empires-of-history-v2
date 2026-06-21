namespace EmpiresOfHistoryV2.Simulation;

/// <summary>
/// Provides versioning and migration support for simulation system save data.
/// Implement alongside ISimulationSerializer to support save file upgrades.
/// </summary>
public interface ISimulationVersion : ISimulationSystem
{
    /// <summary>Current save data version for this system.</summary>
    int Version { get; }

    /// <summary>Returns true if this system can migrate from the given version.</summary>
    bool CanMigrate(int fromVersion);

    /// <summary>Migrates serialized JSON from an older version to current.</summary>
    string Migrate(int fromVersion, string json);
}
