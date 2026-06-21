using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EmpiresOfHistoryV2.Validation;

public class TimelineValidator
{
    public ValidationReport Validate(JsonElement timelineDatabaseEnvelope)
    {
        var report = new ValidationReport();

        if (!timelineDatabaseEnvelope.TryGetProperty("records", out var records) || records.ValueKind != JsonValueKind.Array)
        {
            report.AddError("Timeline database must contain records array.", "records");
            return report;
        }

        var eras = new List<(string EraId, int StartYear, int EndYear)>();
        foreach (var record in records.EnumerateArray())
        {
            var eraId = record.GetProperty("era_id").GetString() ?? string.Empty;
            var start = record.GetProperty("start_year").GetInt32();
            var end = record.GetProperty("end_year").GetInt32();
            if (end < start)
                report.AddError($"Era '{eraId}' has end_year before start_year.", eraId);
            eras.Add((eraId, start, end));
        }

        if (eras.Count == 0)
        {
            report.AddError("Timeline records are empty.", "records");
            return report;
        }

        var ordered = eras.OrderBy(e => e.StartYear).ToList();
        if (ordered[0].StartYear > -8300)
            report.AddError("Timeline must start at or before 8300 BC (-8300).", "timeline_start");
        if (ordered[^1].EndYear < 2100)
            report.AddError("Timeline must extend to at least 2100.", "timeline_end");

        for (var i = 1; i < ordered.Count; i++)
        {
            if (ordered[i].StartYear > ordered[i - 1].EndYear + 1)
                report.AddError($"Gap detected between {ordered[i - 1].EraId} and {ordered[i].EraId}.", "timeline_gap");
        }

        return report;
    }
}
