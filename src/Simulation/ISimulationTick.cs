namespace EmpiresOfHistoryV2.Simulation;

/// <summary>
/// Implemented by systems that execute logic each turn.
/// Systems that only react to events do NOT need to implement this.
/// </summary>
public interface ISimulationTick : ISimulationSystem
{
    /// <summary>
    /// Execute one turn of simulation logic.
    /// Must NOT call other systems directly — communicate via EventSystem or SimulationContext.
    /// </summary>
    void Tick(SimulationContext context);
}
