using System.Collections.Generic;
using System.Text.Json;

namespace EmpiresOfHistoryV2.Validation;

public class AssetValidator
{
    public ValidationReport ValidateAssetReferences(JsonElement records, HashSet<string> assetIds, string fieldName = "asset_id")
    {
        var report = new ValidationReport();

        foreach (var record in records.EnumerateArray())
        {
            if (!record.TryGetProperty(fieldName, out var assetValue) || assetValue.ValueKind != JsonValueKind.String)
                continue;

            var assetId = assetValue.GetString();
            if (!string.IsNullOrWhiteSpace(assetId) && !assetIds.Contains(assetId))
                report.AddError($"Unknown asset reference '{assetId}'.", fieldName);
        }

        return report;
    }
}
