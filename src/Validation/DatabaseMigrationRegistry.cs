using System.Collections.Generic;

namespace EmpiresOfHistoryV2.Validation;

public static class DatabaseMigrationRegistry
{
    private static readonly Dictionary<int, string> Migrations = new()
    {
        [1] = "Baseline Phase 4.95 schema envelope with source tracking metadata."
    };

    public static IReadOnlyDictionary<int, string> All => Migrations;

    public static string? GetMigrationNotes(int schemaVersion) =>
        Migrations.TryGetValue(schemaVersion, out var notes) ? notes : null;
}
