using System.Collections.Generic;

namespace EmpiresOfHistoryV2.Simulation;

/// <summary>
/// Tracks which nations/provinces have changed this turn.
/// Systems check IsDirty() before running expensive calculations.
/// Target: 250 nations, 5000 provinces at 60 FPS — no full-world recalc every turn.
/// </summary>
public class DirtyRegionTracker
{
    private readonly HashSet<string> _dirtyNations = new();
    private readonly HashSet<string> _dirtyProvinces = new();
    private bool _globalDirty = false;

    public void MarkNationDirty(string nationId) => _dirtyNations.Add(nationId);
    public void MarkProvinceDirty(string provinceId) => _dirtyProvinces.Add(provinceId);
    public void MarkGlobalDirty() => _globalDirty = true;

    public bool IsNationDirty(string nationId) => _globalDirty || _dirtyNations.Contains(nationId);
    public bool IsProvinceDirty(string provinceId) => _globalDirty || _dirtyProvinces.Contains(provinceId);
    public bool IsGlobalDirty => _globalDirty;

    public IReadOnlySet<string> DirtyNations => _dirtyNations;
    public IReadOnlySet<string> DirtyProvinces => _dirtyProvinces;

    /// <summary>Call at start of each turn to clear previous frame's dirty state.</summary>
    public void Reset()
    {
        _dirtyNations.Clear();
        _dirtyProvinces.Clear();
        _globalDirty = false;
    }
}
