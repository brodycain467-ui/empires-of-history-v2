using System.Collections.Generic;
using System.Text.Json;

namespace EmpiresOfHistoryV2.Validation;

public class OwnershipValidator
{
    public ValidationReport Validate(JsonElement records, HashSet<string> validNationIds)
    {
        var report = new ValidationReport();

        foreach (var record in records.EnumerateArray())
        {
            ValidateOwnershipField(record, "owner_id", validNationIds, report);
            ValidateOwnershipField(record, "controller_id", validNationIds, report);
            ValidateOwnershipField(record, "core_owner_id", validNationIds, report);
        }

        return report;
    }

    private static void ValidateOwnershipField(JsonElement record, string field, HashSet<string> validNationIds, ValidationReport report)
    {
        if (!record.TryGetProperty(field, out var value) || value.ValueKind == JsonValueKind.Null)
            return;

        var id = value.GetString();
        if (!string.IsNullOrEmpty(id) && !validNationIds.Contains(id))
            report.AddError($"Invalid {field} reference '{id}'.", field);
    }
}
