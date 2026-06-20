using System;
using System.Collections.Generic;
using System.Linq;
using EmpiresOfHistoryV2.Map.Models;

namespace EmpiresOfHistoryV2.Map.Systems;

public class OwnershipSystem
{
    public event Action<string, string, string>? OwnershipChanged;

    private readonly Dictionary<string, string> _provinceOwners = new();
    private readonly Dictionary<string, HashSet<string>> _nationProvinces = new();

    public void Initialize(List<ProvinceData> provinces)
    {
        _provinceOwners.Clear();
        _nationProvinces.Clear();

        foreach (var province in provinces)
        {
            var ownerId = string.IsNullOrEmpty(province.CurrentOwnerId) ? province.OwnerNationId : province.CurrentOwnerId;
            _provinceOwners[province.Id] = ownerId;
            province.CurrentOwnerId = ownerId;

            if (!_nationProvinces.TryGetValue(ownerId, out var owned))
            {
                owned = new HashSet<string>();
                _nationProvinces[ownerId] = owned;
            }

            owned.Add(province.Id);
        }
    }

    public void SetOwnership(string provinceId, string nationId)
    {
        if (!_provinceOwners.TryGetValue(provinceId, out var fromNationId))
        {
            fromNationId = string.Empty;
        }

        if (fromNationId == nationId)
        {
            return;
        }

        if (!string.IsNullOrEmpty(fromNationId) && _nationProvinces.TryGetValue(fromNationId, out var fromOwned))
        {
            fromOwned.Remove(provinceId);
        }

        if (!_nationProvinces.TryGetValue(nationId, out var toOwned))
        {
            toOwned = new HashSet<string>();
            _nationProvinces[nationId] = toOwned;
        }

        _provinceOwners[provinceId] = nationId;
        toOwned.Add(provinceId);
        OwnershipChanged?.Invoke(provinceId, fromNationId, nationId);
    }

    public string GetOwner(string provinceId)
    {
        return _provinceOwners.GetValueOrDefault(provinceId, string.Empty);
    }

    public IReadOnlyList<string> GetProvinces(string nationId)
    {
        return _nationProvinces.TryGetValue(nationId, out var provinces)
            ? provinces.ToList()
            : [];
    }

    public void TransferProvince(string provinceId, string toNationId)
    {
        SetOwnership(provinceId, toNationId);
    }

    public void BatchTransfer(IEnumerable<string> provinceIds, string toNationId)
    {
        foreach (var provinceId in provinceIds)
        {
            SetOwnership(provinceId, toNationId);
        }
    }
}
