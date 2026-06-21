using System;
using System.Collections.Generic;
using System.Linq;

namespace EmpiresOfHistoryV2.Simulation;

/// <summary>
/// Owns all registered ISimulationSystem instances.
/// Executes tick in TickOrder order.
/// GameManager calls SimulationManager.Tick() — it never calls individual systems.
///
/// Future systems register themselves in GameManager._Ready() or via a system manifest.
/// No modification to SimulationManager is needed to add a new system.
/// </summary>
public class SimulationManager
{
    private readonly List<ISimulationSystem> _systems = [];
    private readonly Dictionary<string, ISimulationSystem> _byId = new();
    private bool _initialized = false;

    // ── Registration ────────────────────────────────────────────────────────

    public void RegisterSystem(ISimulationSystem system)
    {
        if (_byId.ContainsKey(system.SystemId))
        {
            Console.Error.WriteLine($"[SimulationManager] System '{system.SystemId}' is already registered. Skipping.");
            return;
        }
        _systems.Add(system);
        _byId[system.SystemId] = system;
        // Keep sorted by TickOrder at all times
        _systems.Sort((a, b) => a.TickOrder.CompareTo(b.TickOrder));
    }

    public void UnregisterSystem(string systemId)
    {
        if (!_byId.TryGetValue(systemId, out var system)) return;
        _systems.Remove(system);
        _byId.Remove(systemId);
        system.Dispose();
    }

    public ISimulationSystem? GetSystem(string systemId) =>
        _byId.GetValueOrDefault(systemId);

    public T? GetSystem<T>() where T : class, ISimulationSystem =>
        _systems.OfType<T>().FirstOrDefault();

    public IReadOnlyList<ISimulationSystem> AllSystems => _systems;

    // ── Lifecycle ───────────────────────────────────────────────────────────

    public void Initialize(SimulationContext context)
    {
        if (_initialized) return;
        foreach (var system in _systems)
        {
            try { system.Initialize(context); }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[SimulationManager] Initialize failed for '{system.SystemId}': {ex.Message}");
            }
        }
        _initialized = true;
    }

    /// <summary>
    /// Execute one full turn. Systems tick in TickOrder order.
    /// EventSystem integration must be wired externally (TurnSystem calls EventSystem separately).
    /// </summary>
    public void Tick(SimulationContext context)
    {
        foreach (var system in _systems)
        {
            if (!system.IsEnabled) continue;
            if (system is not ISimulationTick tickable) continue;
            try { tickable.Tick(context); }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[SimulationManager] Tick failed for '{system.SystemId}': {ex.Message}");
            }
        }
    }

    // ── Serialization ───────────────────────────────────────────────────────

    /// <summary>Serialize all systems that implement ISimulationSerializer.</summary>
    public Dictionary<string, string> Save()
    {
        var result = new Dictionary<string, string>();
        foreach (var system in _systems.OfType<ISimulationSerializer>())
        {
            try { result[system.SystemId] = system.Serialize(); }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[SimulationManager] Save failed for '{system.SystemId}': {ex.Message}");
            }
        }
        return result;
    }

    /// <summary>Restore all systems from serialized data.</summary>
    public void Load(Dictionary<string, string> data)
    {
        foreach (var system in _systems.OfType<ISimulationSerializer>())
        {
            if (!data.TryGetValue(system.SystemId, out var json)) continue;
            try { system.Deserialize(json); }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[SimulationManager] Load failed for '{system.SystemId}': {ex.Message}");
            }
        }
    }

    // ── Provider Query ──────────────────────────────────────────────────────

    /// <summary>
    /// Query a named value from any registered ISimulationProvider.
    /// Used by GIA Advisor to read cross-system state without direct references.
    /// </summary>
    public string? QueryProvider(string systemId, string key)
    {
        if (_byId.TryGetValue(systemId, out var system) && system is ISimulationProvider provider)
            return provider.GetValue(key);
        return null;
    }
}
