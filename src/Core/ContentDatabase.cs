using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Godot;
using EmpiresOfHistoryV2.Map.Models;

namespace EmpiresOfHistoryV2.Core;

public class ContentDatabase
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly Dictionary<string, ProvinceData> _provincesById = new();
    private readonly Dictionary<string, NationData> _nationsById = new();

    public void LoadAll()
    {
        LoadNations();
        LoadProvinces();
    }

    public IReadOnlyList<ProvinceData> GetAllProvinces() => _provincesById.Values.ToList();

    public ProvinceData? GetProvince(string id) => _provincesById.GetValueOrDefault(id);

    public IReadOnlyList<NationData> GetAllNations() => _nationsById.Values.ToList();

    public NationData? GetNation(string id) => _nationsById.GetValueOrDefault(id);

    private void LoadNations()
    {
        var filePath = Path.Combine(ProjectSettings.GlobalizePath("res://"), "data", "nations", "nations.json");
        var json = File.ReadAllText(filePath);
        var payload = JsonSerializer.Deserialize<NationsDatabase>(json, _jsonOptions) ?? new NationsDatabase();

        _nationsById.Clear();
        foreach (var nation in payload.Nations)
        {
            _nationsById[nation.Id] = nation;
        }
    }

    private void LoadProvinces()
    {
        var filePath = Path.Combine(ProjectSettings.GlobalizePath("res://"), "data", "provinces", "provinces.json");
        var json = File.ReadAllText(filePath);
        var payload = JsonSerializer.Deserialize<ProvincesDatabase>(json, _jsonOptions) ?? new ProvincesDatabase();

        _provincesById.Clear();
        foreach (var province in payload.Provinces)
        {
            province.CurrentOwnerId = province.OwnerNationId;
            _provincesById[province.Id] = province;
        }
    }
}
