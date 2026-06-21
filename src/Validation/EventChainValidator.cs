using System.Collections.Generic;
using System.Text.Json;

namespace EmpiresOfHistoryV2.Validation;

public class EventChainValidator
{
    public ValidationReport Validate(JsonElement eventDatabaseEnvelope)
    {
        var report = new ValidationReport();

        if (!eventDatabaseEnvelope.TryGetProperty("records", out var records) || records.ValueKind != JsonValueKind.Array)
            return report;

        var ids = new HashSet<string>();
        foreach (var record in records.EnumerateArray())
        {
            if (record.TryGetProperty("id", out var idValue) && idValue.ValueKind == JsonValueKind.String)
            {
                var id = idValue.GetString();
                if (!string.IsNullOrWhiteSpace(id))
                    ids.Add(id);
            }
        }

        foreach (var record in records.EnumerateArray())
        {
            ValidateArrayReferences(record, "metadata", ids, report);
        }

        return report;
    }

    private static void ValidateArrayReferences(JsonElement record, string metadataField, HashSet<string> ids, ValidationReport report)
    {
        if (!record.TryGetProperty(metadataField, out var metadata) || metadata.ValueKind != JsonValueKind.Object)
            return;

        if (metadata.TryGetProperty("prerequisite_event_ids", out var prerequisites) && prerequisites.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in prerequisites.EnumerateArray())
            {
                var eventId = item.GetString();
                if (!string.IsNullOrEmpty(eventId) && !ids.Contains(eventId))
                    report.AddError($"Missing prerequisite event reference '{eventId}'.", "prerequisite_event_ids");
            }
        }

        if (metadata.TryGetProperty("follow_up_event_ids", out var followUps) && followUps.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in followUps.EnumerateArray())
            {
                var eventId = item.GetString();
                if (!string.IsNullOrEmpty(eventId) && !ids.Contains(eventId))
                    report.AddError($"Missing follow-up event reference '{eventId}'.", "follow_up_event_ids");
            }
        }
    }
}
