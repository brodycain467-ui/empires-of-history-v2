using System.Collections.Generic;
using System.Text.Json;

namespace EmpiresOfHistoryV2.Tools.Importers;

public static class JsonBuilder
{
    public static string Build(IReadOnlyList<Dictionary<string, string>> records) => JsonSerializer.Serialize(records);
}
