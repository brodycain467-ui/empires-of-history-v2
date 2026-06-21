namespace EmpiresOfHistoryV2.Tools.Importers;

public class ImportPipeline
{
    public string Run(string csvInput) => JsonBuilder.Build(RecordValidator.Validate(CsvImporter.Parse(csvInput)));
}
