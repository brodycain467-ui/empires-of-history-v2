using System.Collections.Generic;
using System.Text.Json;

namespace EmpiresOfHistoryV2.Validation;

public class ReferenceValidator
{
    public ValidationReport ValidateRecords(JsonElement records, IReadOnlyDictionary<string, HashSet<string>> referenceIndex, params string[] referenceFields)
    {
        var report = new ValidationReport();

        if (records.ValueKind != JsonValueKind.Array)
        {
            report.AddError("records must be an array.", "records");
            return report;
        }

        foreach (var record in records.EnumerateArray())
        {
            foreach (var field in referenceFields)
            {
                if (!record.TryGetProperty(field, out var value) || value.ValueKind != JsonValueKind.String)
                    continue;

                var referenceValue = value.GetString();
                if (referenceValue is null || referenceValue.Length == 0)
                    continue;

                if (!referenceIndex.TryGetValue(field, out var validSet) || !validSet.Contains(referenceValue))
                    report.AddError($"Broken reference '{referenceValue}' in field '{field}'.", field);
            }
        }

        return report;
    }
}
