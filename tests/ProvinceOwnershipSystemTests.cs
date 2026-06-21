using EmpiresOfHistory.Map.Models;
using EmpiresOfHistory.Map.Systems;
using Xunit;

namespace EmpiresOfHistory.Tests;

public class ProvinceOwnershipSystemTests
{
    [Fact]
    public void RegisterProvince_ProvinceIsRegisteredAndQueryable()
    {
        var system = new ProvinceOwnershipSystem();
        var province = CreateProvince("prov_1", "Province 1");

        system.RegisterProvince(province);

        Assert.True(system.IsRegistered("prov_1"));
        Assert.Same(province, system.GetProvince("prov_1"));
    }

    [Fact]
    public void GetOwner_ReturnsNullForUnregisteredProvince()
    {
        var system = new ProvinceOwnershipSystem();

        Assert.Null(system.GetOwner("missing"));
    }

    [Fact]
    public void GetOwner_ReturnsNullForRegisteredButUnownedProvince()
    {
        var system = new ProvinceOwnershipSystem();
        system.RegisterProvince(CreateProvince("prov_1", "Province 1"));

        Assert.Null(system.GetOwner("prov_1"));
    }

    [Fact]
    public void TransferOwnership_Initial_SucceedsOwnerSetSnapshotRecorded()
    {
        var system = new ProvinceOwnershipSystem();
        system.RegisterProvince(CreateProvince("prov_1", "Province 1"));

        var result = system.TransferOwnership(new OwnershipTransferEvent
        {
            ProvinceId = "prov_1",
            FromNationId = null,
            ToNationId = "nat_a",
            GameYear = 1800,
            TurnNumber = 1,
            Reason = "initial"
        });

        Assert.True(result.Success);
        Assert.Equal("nat_a", system.GetOwner("prov_1"));
        Assert.NotNull(result.SnapshotCreated);
        Assert.Equal("nat_a", result.SnapshotCreated.NationId);
        Assert.Single(system.History.GetHistory("prov_1"));
    }

    [Fact]
    public void TransferOwnership_Conquest_PreviousOwnerLosesNewOwnerGains()
    {
        var system = new ProvinceOwnershipSystem();
        system.RegisterProvince(CreateProvince("prov_1", "Province 1", ownerNationId: "nat_a"));

        var result = system.TransferOwnership(new OwnershipTransferEvent
        {
            ProvinceId = "prov_1",
            FromNationId = "nat_a",
            ToNationId = "nat_b",
            GameYear = 1801,
            TurnNumber = 2,
            Reason = "conquest"
        });

        Assert.True(result.Success);
        Assert.Equal("nat_b", system.GetOwner("prov_1"));
        Assert.DoesNotContain("prov_1", system.GetProvincesByNation("nat_a"));
        Assert.Contains("prov_1", system.GetProvincesByNation("nat_b"));
    }

    [Fact]
    public void TransferOwnership_InvalidFromNationId_FailsAndNoStateChange()
    {
        var system = new ProvinceOwnershipSystem();
        system.RegisterProvince(CreateProvince("prov_1", "Province 1", ownerNationId: "nat_a"));

        var result = system.TransferOwnership(new OwnershipTransferEvent
        {
            ProvinceId = "prov_1",
            FromNationId = "nat_x",
            ToNationId = "nat_b",
            GameYear = 1801,
            TurnNumber = 2,
            Reason = "conquest"
        });

        Assert.False(result.Success);
        Assert.Equal("nat_a", system.GetOwner("prov_1"));
        Assert.Null(result.SnapshotCreated);
    }

    [Fact]
    public void TransferOwnership_ProvinceNotRegistered_Fails()
    {
        var system = new ProvinceOwnershipSystem();

        var result = system.TransferOwnership(new OwnershipTransferEvent
        {
            ProvinceId = "missing",
            FromNationId = null,
            ToNationId = "nat_a",
            GameYear = 1800,
            TurnNumber = 1,
            Reason = "initial"
        });

        Assert.False(result.Success);
    }

    [Fact]
    public void TransferOwnership_SelfTransferNonInitial_Fails()
    {
        var system = new ProvinceOwnershipSystem();
        system.RegisterProvince(CreateProvince("prov_1", "Province 1", ownerNationId: "nat_a"));

        var result = system.TransferOwnership(new OwnershipTransferEvent
        {
            ProvinceId = "prov_1",
            FromNationId = "nat_a",
            ToNationId = "nat_a",
            GameYear = 1802,
            TurnNumber = 3,
            Reason = "treaty"
        });

        Assert.False(result.Success);
    }

    [Fact]
    public void BatchTransfer_AllSucceed_AllSuccessTrue()
    {
        var system = new ProvinceOwnershipSystem();
        system.RegisterProvinces(new[]
        {
            CreateProvince("prov_1", "Province 1"),
            CreateProvince("prov_2", "Province 2")
        });

        var results = system.BatchTransfer(new[]
        {
            new OwnershipTransferEvent { ProvinceId = "prov_1", FromNationId = null, ToNationId = "nat_a", GameYear = 1800, TurnNumber = 1, Reason = "initial" },
            new OwnershipTransferEvent { ProvinceId = "prov_2", FromNationId = null, ToNationId = "nat_a", GameYear = 1800, TurnNumber = 2, Reason = "initial" }
        });

        Assert.Equal(2, results.Count);
        Assert.All(results, r => Assert.True(r.Success));
    }

    [Fact]
    public void BatchTransfer_MixedValidInvalid_ReturnsPerItemResults()
    {
        var system = new ProvinceOwnershipSystem();
        system.RegisterProvince(CreateProvince("prov_1", "Province 1"));

        var results = system.BatchTransfer(new[]
        {
            new OwnershipTransferEvent { ProvinceId = "prov_1", FromNationId = null, ToNationId = "nat_a", GameYear = 1800, TurnNumber = 1, Reason = "initial" },
            new OwnershipTransferEvent { ProvinceId = "missing", FromNationId = null, ToNationId = "nat_b", GameYear = 1800, TurnNumber = 2, Reason = "initial" }
        });

        Assert.True(results[0].Success);
        Assert.False(results[1].Success);
    }

    [Fact]
    public void GetProvincesByNation_ReturnsCorrectListAfterTransfers()
    {
        var system = new ProvinceOwnershipSystem();
        system.RegisterProvince(CreateProvince("prov_1", "Province 1"));
        system.RegisterProvince(CreateProvince("prov_2", "Province 2"));

        system.TransferOwnership(new OwnershipTransferEvent { ProvinceId = "prov_1", ToNationId = "nat_a", GameYear = 1800, TurnNumber = 1, Reason = "initial" });
        system.TransferOwnership(new OwnershipTransferEvent { ProvinceId = "prov_2", ToNationId = "nat_a", GameYear = 1800, TurnNumber = 2, Reason = "initial" });

        var provinces = system.GetProvincesByNation("nat_a");

        Assert.Equal(2, provinces.Count);
        Assert.Contains("prov_1", provinces);
        Assert.Contains("prov_2", provinces);
    }

    [Fact]
    public void GetProvinceCount_CorrectCountsThroughout()
    {
        var system = new ProvinceOwnershipSystem();
        system.RegisterProvince(CreateProvince("prov_1", "Province 1"));
        system.RegisterProvince(CreateProvince("prov_2", "Province 2"));

        Assert.Equal(0, system.GetProvinceCount("nat_a"));

        system.TransferOwnership(new OwnershipTransferEvent { ProvinceId = "prov_1", ToNationId = "nat_a", GameYear = 1800, TurnNumber = 1, Reason = "initial" });
        Assert.Equal(1, system.GetProvinceCount("nat_a"));

        system.TransferOwnership(new OwnershipTransferEvent { ProvinceId = "prov_2", ToNationId = "nat_a", GameYear = 1801, TurnNumber = 2, Reason = "initial" });
        Assert.Equal(2, system.GetProvinceCount("nat_a"));
    }

    [Fact]
    public void HistoryGetHistory_ReturnsAllSnapshotsInOrder()
    {
        var system = new ProvinceOwnershipSystem();
        system.RegisterProvince(CreateProvince("prov_1", "Province 1"));

        system.TransferOwnership(new OwnershipTransferEvent { ProvinceId = "prov_1", ToNationId = "nat_a", GameYear = 1800, TurnNumber = 2, Reason = "initial" });
        system.TransferOwnership(new OwnershipTransferEvent { ProvinceId = "prov_1", FromNationId = "nat_a", ToNationId = "nat_b", GameYear = 1801, TurnNumber = 3, Reason = "conquest" });

        var history = system.History.GetHistory("prov_1");

        Assert.Equal(2, history.Count);
        Assert.Equal(2, history[0].TurnNumber);
        Assert.Equal(3, history[1].TurnNumber);
    }

    [Fact]
    public void HistoryGetOwnerAt_CorrectOwnerForPastYear()
    {
        var system = new ProvinceOwnershipSystem();
        system.RegisterProvince(CreateProvince("prov_1", "Province 1"));

        system.TransferOwnership(new OwnershipTransferEvent { ProvinceId = "prov_1", ToNationId = "nat_a", GameYear = 1800, TurnNumber = 1, Reason = "initial" });
        system.TransferOwnership(new OwnershipTransferEvent { ProvinceId = "prov_1", FromNationId = "nat_a", ToNationId = "nat_b", GameYear = 1810, TurnNumber = 2, Reason = "conquest" });

        Assert.Equal("nat_a", system.History.GetOwnerAt("prov_1", 1805));
        Assert.Equal("nat_b", system.History.GetOwnerAt("prov_1", 1810));
    }

    [Fact]
    public void HistoryGetOwnerAt_NullForYearBeforeAnySnapshot()
    {
        var system = new ProvinceOwnershipSystem();
        system.RegisterProvince(CreateProvince("prov_1", "Province 1"));

        system.TransferOwnership(new OwnershipTransferEvent { ProvinceId = "prov_1", ToNationId = "nat_a", GameYear = 1800, TurnNumber = 1, Reason = "initial" });

        Assert.Null(system.History.GetOwnerAt("prov_1", 1700));
    }

    [Fact]
    public void HistoryGetTransfersBetween_CorrectRangeFiltering()
    {
        var system = new ProvinceOwnershipSystem();
        system.RegisterProvince(CreateProvince("prov_1", "Province 1"));

        system.TransferOwnership(new OwnershipTransferEvent { ProvinceId = "prov_1", ToNationId = "nat_a", GameYear = 1800, TurnNumber = 1, Reason = "initial" });
        system.TransferOwnership(new OwnershipTransferEvent { ProvinceId = "prov_1", FromNationId = "nat_a", ToNationId = "nat_b", GameYear = 1810, TurnNumber = 2, Reason = "conquest" });
        system.TransferOwnership(new OwnershipTransferEvent { ProvinceId = "prov_1", FromNationId = "nat_b", ToNationId = "nat_c", GameYear = 1820, TurnNumber = 3, Reason = "treaty" });

        var transfers = system.History.GetTransfersBetween("prov_1", 1805, 1815);

        Assert.Single(transfers);
        Assert.Equal(1810, transfers[0].GameYear);
    }

    [Fact]
    public void HistoryGetProvincesByNationAt_CorrectProvinceListAtHistoricalYear()
    {
        var system = new ProvinceOwnershipSystem();
        system.RegisterProvince(CreateProvince("prov_1", "Province 1"));
        system.RegisterProvince(CreateProvince("prov_2", "Province 2"));

        system.TransferOwnership(new OwnershipTransferEvent { ProvinceId = "prov_1", ToNationId = "nat_a", GameYear = 1800, TurnNumber = 1, Reason = "initial" });
        system.TransferOwnership(new OwnershipTransferEvent { ProvinceId = "prov_2", ToNationId = "nat_b", GameYear = 1800, TurnNumber = 2, Reason = "initial" });
        system.TransferOwnership(new OwnershipTransferEvent { ProvinceId = "prov_2", FromNationId = "nat_b", ToNationId = "nat_a", GameYear = 1810, TurnNumber = 3, Reason = "conquest" });

        var ownedIn1805 = system.History.GetProvincesByNationAt("nat_a", 1805);
        var ownedIn1815 = system.History.GetProvincesByNationAt("nat_a", 1815);

        Assert.Single(ownedIn1805);
        Assert.Equal("prov_1", ownedIn1805[0]);
        Assert.Equal(2, ownedIn1815.Count);
    }

    [Fact]
    public void OnOwnershipChanged_FiresOnSuccessNotOnFailure()
    {
        var system = new ProvinceOwnershipSystem();
        system.RegisterProvince(CreateProvince("prov_1", "Province 1"));
        var fireCount = 0;

        system.OnOwnershipChanged += _ => fireCount++;

        system.TransferOwnership(new OwnershipTransferEvent
        {
            ProvinceId = "prov_1",
            FromNationId = null,
            ToNationId = "nat_a",
            GameYear = 1800,
            TurnNumber = 1,
            Reason = "initial"
        });

        system.TransferOwnership(new OwnershipTransferEvent
        {
            ProvinceId = "missing",
            FromNationId = null,
            ToNationId = "nat_a",
            GameYear = 1801,
            TurnNumber = 2,
            Reason = "initial"
        });

        Assert.Equal(1, fireCount);
    }

    private static ProvinceRecord CreateProvince(string id, string name, string ownerNationId = null)
    {
        return new ProvinceRecord
        {
            Id = id,
            Name = name,
            DisplayName = name,
            Population = 1000,
            PrimaryCultureId = "cult_test",
            PrimaryReligionId = "rel_test",
            OwnerNationId = ownerNationId,
            CoreNationId = ownerNationId,
            IsCapital = false,
            RegionId = "region_test",
            Terrain = "plains",
            DevelopmentLevel = 0.5f,
            StrategicValue = 0.5f
        };
    }
}
