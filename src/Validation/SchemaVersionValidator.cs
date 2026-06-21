using System.Text.Json;

namespace EmpiresOfHistoryV2.Validation;

public class SchemaVersionValidator
{
    public int RequiredSchemaVersion { get; }
    public string MinimumEngineVersion { get; }

    public SchemaVersionValidator(int requiredSchemaVersion = 1, string minimumEngineVersion = "0.4.95")
    {
        RequiredSchemaVersion = requiredSchemaVersion;
        MinimumEngineVersion = minimumEngineVersion;
    }

    public ValidationReport Validate(JsonElement databaseEnvelope)
    {
        var report = new ValidationReport();

        if (!databaseEnvelope.TryGetProperty("schema_version", out var schemaVersion) || schemaVersion.GetInt32() != RequiredSchemaVersion)
            report.AddError($"schema_version must be {RequiredSchemaVersion}.", "schema_version");

        if (!databaseEnvelope.TryGetProperty("minimum_engine_version", out var minimumVersion) || minimumVersion.GetString() != MinimumEngineVersion)
            report.AddError($"minimum_engine_version must be {MinimumEngineVersion}.", "minimum_engine_version");

        return report;
    }
}
