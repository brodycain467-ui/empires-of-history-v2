using System.Collections.Generic;
using Godot;
using EmpiresOfHistoryV2.Map.Models;

namespace EmpiresOfHistoryV2.Map.Systems;

public class NationColorRegistry
{
    private readonly Dictionary<string, Color> _colorsByNationId = new();

    public void Initialize(List<NationData> nations)
    {
        _colorsByNationId.Clear();
        foreach (var nation in nations)
        {
            _colorsByNationId[nation.Id] = Color.FromHtml(nation.Color);
        }
    }

    public Color GetColor(string nationId)
    {
        return _colorsByNationId.GetValueOrDefault(nationId, new Color("7a7a7a"));
    }
}
