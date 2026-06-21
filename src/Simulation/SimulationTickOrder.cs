namespace EmpiresOfHistoryV2.Simulation;

/// <summary>
/// Canonical turn execution order.
/// All systems must use these constants for TickOrder — never magic numbers.
/// </summary>
public static class SimulationTickOrder
{
    public const int Timeline      = 100;
    public const int Events        = 200;   // EventSystem is called by TurnSystem, not SimulationManager
    public const int Population    = 300;
    public const int Economy       = 400;
    public const int Technology    = 500;
    public const int Government    = 600;
    public const int Elections     = 700;
    public const int Religion      = 800;
    public const int Intelligence  = 900;
    public const int Military      = 1000;
    public const int GIAAdvisor    = 1100;
    // UI Refresh is handled by TurnSystem.TurnAdvanced event — not a simulation system
}
