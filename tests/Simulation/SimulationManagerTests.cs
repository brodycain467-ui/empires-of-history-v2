using System;
using System.Collections.Generic;
using EmpiresOfHistoryV2.Simulation;
using Xunit;

namespace EmpiresOfHistoryV2.Tests.Simulation;

// ---------------------------------------------------------------------------
// Test doubles
// ---------------------------------------------------------------------------

/// <summary>A simple system that implements ISimulationTick (enabled by default).</summary>
file class TestTickSystem : ISimulationTick
{
    public string SystemId { get; }
    public int TickOrder { get; }
    public bool IsEnabled { get; set; } = true;

    public List<int> TickCallOrder { get; } = new();
    private static readonly List<int> _globalOrder = new();
    public static List<int> GlobalCallOrder => _globalOrder;
    public static void ResetGlobal() => _globalOrder.Clear();

    private readonly int _index;

    public TestTickSystem(string id, int tickOrder, int index = 0)
    {
        SystemId = id;
        TickOrder = tickOrder;
        _index = index;
    }

    public void Initialize(SimulationContext context) { }
    public void Dispose() { }
    public void Tick(SimulationContext context)
    {
        TickCallOrder.Add(_index);
        _globalOrder.Add(_index);
    }
}

/// <summary>A system that implements ISimulationSerializer only (no tick).</summary>
file class TestSerializerSystem : ISimulationSerializer
{
    public string SystemId { get; }
    public int TickOrder { get; }
    public bool IsEnabled => true;
    public string LastSerialized { get; private set; } = string.Empty;
    public string LastDeserialized { get; private set; } = string.Empty;

    public TestSerializerSystem(string id, int tickOrder = 0)
    {
        SystemId = id;
        TickOrder = tickOrder;
    }

    public void Initialize(SimulationContext context) { }
    public void Dispose() { }
    public string Serialize() { LastSerialized = $"{{\"id\":\"{SystemId}\"}}"; return LastSerialized; }
    public void Deserialize(string json) { LastDeserialized = json; }
}

/// <summary>A system that implements ISimulationProvider only (no tick).</summary>
file class TestProviderSystem : ISimulationProvider
{
    public string SystemId { get; }
    public int TickOrder { get; }
    public bool IsEnabled => true;
    private readonly Dictionary<string, string> _values;

    public TestProviderSystem(string id, int tickOrder, Dictionary<string, string>? values = null)
    {
        SystemId = id;
        TickOrder = tickOrder;
        _values = values ?? new Dictionary<string, string>();
    }

    public void Initialize(SimulationContext context) { }
    public void Dispose() { }
    public string? GetValue(string key) => _values.GetValueOrDefault(key);
    public IReadOnlyList<string> GetExportedKeys() => new List<string>(_values.Keys);
}

/// <summary>Minimal SimulationContext for tests — no Godot dependencies needed.</summary>
file static class TestContext
{
    public static SimulationContext Make(int turn = 1) => new()
    {
        TurnNumber = turn,
        GameDate = new DateTime(2011, 1, 1),
        ActiveNationId = null
    };
}

// ---------------------------------------------------------------------------
// SimulationManager tests
// ---------------------------------------------------------------------------

public class SimulationManagerTests
{
    // ── Registration ──────────────────────────────────────────────────────

    [Fact]
    public void RegisterSystem_AddsSystem()
    {
        var manager = new SimulationManager();
        var system = new TestTickSystem("SysA", 100, 0);

        manager.RegisterSystem(system);

        Assert.Single(manager.AllSystems);
        Assert.Same(system, manager.GetSystem("SysA"));
    }

    [Fact]
    public void RegisterSystem_DeduplicatesBySameId_SecondIsIgnored()
    {
        var manager = new SimulationManager();
        var first = new TestTickSystem("SysA", 100, 1);
        var second = new TestTickSystem("SysA", 200, 2);

        manager.RegisterSystem(first);
        manager.RegisterSystem(second); // duplicate — should be silently ignored

        Assert.Single(manager.AllSystems);
        Assert.Same(first, manager.GetSystem("SysA")); // original is kept
    }

    // ── Tick ──────────────────────────────────────────────────────────────

    [Fact]
    public void Tick_CallsOnlyEnabledTickSystems()
    {
        var manager = new SimulationManager();
        var enabled = new TestTickSystem("Enabled", 100, 1) { IsEnabled = true };
        var disabled = new TestTickSystem("Disabled", 200, 2) { IsEnabled = false };
        manager.RegisterSystem(enabled);
        manager.RegisterSystem(disabled);

        manager.Tick(TestContext.Make());

        Assert.Single(enabled.TickCallOrder);
        Assert.Empty(disabled.TickCallOrder);
    }

    [Fact]
    public void Tick_SkipsDisabledSystems()
    {
        var manager = new SimulationManager();
        var sys = new TestTickSystem("Sys", 100, 0) { IsEnabled = false };
        manager.RegisterSystem(sys);

        manager.Tick(TestContext.Make());

        Assert.Empty(sys.TickCallOrder);
    }

    [Fact]
    public void Tick_ExecutesSystemsInTickOrderAscending()
    {
        TestTickSystem.ResetGlobal();
        var manager = new SimulationManager();
        // Register out of order intentionally
        var high = new TestTickSystem("High", 300, 3);
        var low = new TestTickSystem("Low", 100, 1);
        var mid = new TestTickSystem("Mid", 200, 2);
        manager.RegisterSystem(high);
        manager.RegisterSystem(low);
        manager.RegisterSystem(mid);

        manager.Tick(TestContext.Make());

        Assert.Equal(new[] { 1, 2, 3 }, TestTickSystem.GlobalCallOrder);
    }

    [Fact]
    public void Tick_DoesNotCallNonTickSystems()
    {
        var manager = new SimulationManager();
        var serializer = new TestSerializerSystem("Serializer", 100);
        manager.RegisterSystem(serializer);

        // Should not throw — serializer has no Tick, just verify no exception
        manager.Tick(TestContext.Make());
        Assert.Single(manager.AllSystems);
    }

    // ── Save / Load ───────────────────────────────────────────────────────

    [Fact]
    public void Save_OnlySerializesISimulationSerializerSystems()
    {
        var manager = new SimulationManager();
        var tickOnly = new TestTickSystem("Tick", 100, 0);
        var serializer = new TestSerializerSystem("Ser", 200);
        manager.RegisterSystem(tickOnly);
        manager.RegisterSystem(serializer);

        var data = manager.Save();

        Assert.Single(data);
        Assert.True(data.ContainsKey("Ser"));
        Assert.False(data.ContainsKey("Tick"));
    }

    [Fact]
    public void Load_RestoresSerializedState()
    {
        var manager = new SimulationManager();
        var serializer = new TestSerializerSystem("Ser", 100);
        manager.RegisterSystem(serializer);

        var payload = new Dictionary<string, string> { ["Ser"] = "{\"restored\":true}" };
        manager.Load(payload);

        Assert.Equal("{\"restored\":true}", serializer.LastDeserialized);
    }

    // ── Provider Query ────────────────────────────────────────────────────

    [Fact]
    public void QueryProvider_ReturnsNullForMissingKey()
    {
        var manager = new SimulationManager();
        var provider = new TestProviderSystem("Provider", 100, new Dictionary<string, string>
        {
            ["existing_key"] = "value"
        });
        manager.RegisterSystem(provider);

        var result = manager.QueryProvider("Provider", "nonexistent_key");

        Assert.Null(result);
    }

    [Fact]
    public void QueryProvider_ReturnsNullForMissingSystem()
    {
        var manager = new SimulationManager();

        var result = manager.QueryProvider("NonExistentSystem", "key");

        Assert.Null(result);
    }

    [Fact]
    public void QueryProvider_ReturnsValueForRegisteredKey()
    {
        var manager = new SimulationManager();
        var provider = new TestProviderSystem("Provider", 100, new Dictionary<string, string>
        {
            ["my_key"] = "my_value"
        });
        manager.RegisterSystem(provider);

        var result = manager.QueryProvider("Provider", "my_key");

        Assert.Equal("my_value", result);
    }
}

// ---------------------------------------------------------------------------
// DirtyRegionTracker tests
// ---------------------------------------------------------------------------

public class DirtyRegionTrackerTests
{
    [Fact]
    public void MarkNationDirty_IsNationDirty_RoundTrip()
    {
        var tracker = new DirtyRegionTracker();

        tracker.MarkNationDirty("nation_a");

        Assert.True(tracker.IsNationDirty("nation_a"));
        Assert.False(tracker.IsNationDirty("nation_b"));
    }

    [Fact]
    public void MarkProvinceDirty_IsProvinceDirty_RoundTrip()
    {
        var tracker = new DirtyRegionTracker();

        tracker.MarkProvinceDirty("prov_1");

        Assert.True(tracker.IsProvinceDirty("prov_1"));
        Assert.False(tracker.IsProvinceDirty("prov_2"));
    }

    [Fact]
    public void Reset_ClearsAllDirtyState()
    {
        var tracker = new DirtyRegionTracker();
        tracker.MarkNationDirty("nation_a");
        tracker.MarkProvinceDirty("prov_1");
        tracker.MarkGlobalDirty();

        tracker.Reset();

        Assert.False(tracker.IsNationDirty("nation_a"));
        Assert.False(tracker.IsProvinceDirty("prov_1"));
        Assert.False(tracker.IsGlobalDirty);
    }

    [Fact]
    public void MarkGlobalDirty_MakesAllNationsAndProvincesDirty()
    {
        var tracker = new DirtyRegionTracker();

        tracker.MarkGlobalDirty();

        Assert.True(tracker.IsNationDirty("any_nation"));
        Assert.True(tracker.IsProvinceDirty("any_province"));
        Assert.True(tracker.IsGlobalDirty);
    }
}

// ---------------------------------------------------------------------------
// SimulationTickOrder constants test
// ---------------------------------------------------------------------------

public class SimulationTickOrderTests
{
    [Fact]
    public void TickOrderConstants_AreInAscendingOrder()
    {
        Assert.True(SimulationTickOrder.Timeline < SimulationTickOrder.Events);
        Assert.True(SimulationTickOrder.Events < SimulationTickOrder.Population);
        Assert.True(SimulationTickOrder.Population < SimulationTickOrder.Economy);
        Assert.True(SimulationTickOrder.Economy < SimulationTickOrder.Technology);
        Assert.True(SimulationTickOrder.Technology < SimulationTickOrder.Government);
        Assert.True(SimulationTickOrder.Government < SimulationTickOrder.Elections);
        Assert.True(SimulationTickOrder.Elections < SimulationTickOrder.Religion);
        Assert.True(SimulationTickOrder.Religion < SimulationTickOrder.Intelligence);
        Assert.True(SimulationTickOrder.Intelligence < SimulationTickOrder.Military);
        Assert.True(SimulationTickOrder.Military < SimulationTickOrder.GIAAdvisor);
    }
}
