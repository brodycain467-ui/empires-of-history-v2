using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Godot;

namespace EmpiresOfHistoryV2.Events.Definitions;

public class EventDefinitionLoader
{
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    /// <summary>
    /// Load all event definitions from res://data/events/events.json.
    /// Returns dictionary keyed by EventId for O(1) lookup.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the events file is missing or contains invalid JSON.
    /// </exception>
    public Dictionary<string, EventDefinition> LoadAll()
    {
        var filePath = Path.Combine(ProjectSettings.GlobalizePath("res://"), "data", "events", "events.json");
        string json;
        try
        {
            json = File.ReadAllText(filePath);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            throw new InvalidOperationException(
                $"[EventDefinitionLoader] Failed to read event definitions from '{filePath}'. " +
                "Verify the file exists at res://data/events/events.json.", ex);
        }

        EventActionsDatabase db;
        try
        {
            db = JsonSerializer.Deserialize<EventActionsDatabase>(json, _jsonOptions) ?? new EventActionsDatabase();
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException(
                $"[EventDefinitionLoader] events.json at '{filePath}' contains invalid JSON. " +
                "Verify the file matches the expected schema.", ex);
        }

        var result = new Dictionary<string, EventDefinition>(db.Events.Count);
        foreach (var def in db.Events)
        {
            result[def.EventId] = def;
        }

        return result;
    }
}
