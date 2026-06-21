using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using EmpiresOfHistoryV2.Providers;
using EmpiresOfHistoryV2.Simulation;
using EmpiresOfHistoryV2.Simulation.Foundation;
using EmpiresOfHistoryV2.Validation;
using Xunit;

namespace EmpiresOfHistoryV2.Tests;

public class Phase495FinalTests
{
    private static readonly string RepoRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../"));

    [Fact]
    public void JsonSchemaValidation_DataDomainsHaveStandardEnvelope()
    {
        var path = Path.Combine(RepoRoot, "data", "nations", "database.json");
        using var doc = JsonDocument.Parse(File.ReadAllText(path));
        var root = doc.RootElement;

        Assert.Equal(1, root.GetProperty("schema_version").GetInt32());
        Assert.Equal("0.4.95", root.GetProperty("minimum_engine_version").GetString());
        Assert.True(root.TryGetProperty("records", out var records));
        Assert.Equal(JsonValueKind.Array, records.ValueKind);
    }

    [Fact]
    public void IdConventionValidation_UsesLockedPatterns()
    {
        var validator = new IdConventionValidator();

        Assert.True(validator.IsValid("Nation", "nat_usa"));
        Assert.True(validator.IsValid("Province", "prov_usa_dc"));
        Assert.False(validator.IsValid("Nation", "nation_usa"));
    }

    [Fact]
    public void DatabaseReferenceIntegrity_OwnershipValidatorFlagsBrokenOwner()
    {
        using var doc = JsonDocument.Parse("""[{"owner_id":"nat_missing","controller_id":null,"core_owner_id":null}]""");
        var validator = new OwnershipValidator();
        var report = validator.Validate(doc.RootElement, new HashSet<string> { "nat_usa" });

        Assert.False(report.IsValid);
        Assert.Contains(report.Issues, i => i.Context == "owner_id");
    }

    [Fact]
    public void ProviderInterfaceCompliance_PopulationAndTreasuryImplementProviderContracts()
    {
        var population = new PopulationSystem();
        var treasury = new TreasurySystem();

        Assert.IsAssignableFrom<ISystemProvider>(population);
        Assert.IsAssignableFrom<ISystemTick>(population);
        Assert.IsAssignableFrom<ISystemStatistics>(population);

        Assert.IsAssignableFrom<ISystemProvider>(treasury);
        Assert.IsAssignableFrom<ISystemTick>(treasury);
        Assert.IsAssignableFrom<ISystemStatistics>(treasury);
    }

    [Fact]
    public void PopulationTickCorrectness_UpdatesPopulationTotal()
    {
        var system = new PopulationSystem();
        var before = system.population_total;

        system.PopulationTick(1f);

        Assert.True(system.population_total > before);
    }

    [Fact]
    public void TreasuryTickCorrectness_UpdatesTreasuryTotal()
    {
        var system = new TreasurySystem();
        var before = system.treasury_total;

        system.TreasuryTick(1f);

        Assert.True(system.treasury_total > before);
    }

    [Fact]
    public void TimelineEraCoverage_Contains8300BCTo2100AD()
    {
        var path = Path.Combine(RepoRoot, "data", "timelines", "database.json");
        using var doc = JsonDocument.Parse(File.ReadAllText(path));
        var report = new TimelineValidator().Validate(doc.RootElement);

        var eras = doc.RootElement.GetProperty("records").EnumerateArray().ToList();
        Assert.Contains(eras, e => e.GetProperty("start_year").GetInt32() == -8300);
        Assert.Contains(eras, e => e.GetProperty("end_year").GetInt32() == 2100);
        Assert.True(report.IsValid);
    }

    [Fact]
    public void AssetRegistryCompleteness_AllRequiredRegistryFilesExist()
    {
        var assetsPath = Path.Combine(RepoRoot, "data", "assets");
        var required = new[]
        {
            "icons.json", "flags.json", "portraits.json", "backgrounds.json",
            "fonts.json", "music.json", "sounds.json", "videos.json"
        };

        foreach (var file in required)
            Assert.True(File.Exists(Path.Combine(assetsPath, file)), $"Missing asset registry file: {file}");
    }
}
