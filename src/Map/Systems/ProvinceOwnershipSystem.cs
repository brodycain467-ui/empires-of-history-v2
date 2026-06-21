using EmpiresOfHistory.Map.Models;

namespace EmpiresOfHistory.Map.Systems
{
    /// <summary>
    /// Core province ownership engine.
    /// Tracks current province→nation mapping and full ownership history.
    /// Thread-safe for read operations. Write operations should be called from game thread only.
    /// </summary>
    public class ProvinceOwnershipSystem : IOwnershipSystem
    {
        // Current state
        private readonly Dictionary<string, ProvinceRecord> _provinces = new();
        private readonly Dictionary<string, string> _ownership = new();            // provinceId → nationId
        private readonly Dictionary<string, List<string>> _nationProvinces = new(); // nationId → [provinceIds]

        // History engine
        private readonly OwnershipHistory _history = new();
        private readonly OwnershipValidator _validator = new();

        // Public history access (read-only interface)
        public IOwnershipHistory History => _history;

        // Event
        public event Action<OwnershipTransferResult> OnOwnershipChanged;

        // ── Registration ──────────────────────────────────────────

        public void RegisterProvince(ProvinceRecord province)
        {
            if (province == null || string.IsNullOrWhiteSpace(province.Id))
                return;

            _provinces[province.Id] = province;

            if (!string.IsNullOrWhiteSpace(province.OwnerNationId))
                ApplyTransfer(province.Id, GetOwner(province.Id), province.OwnerNationId);
        }

        public void RegisterProvinces(IEnumerable<ProvinceRecord> provinces)
        {
            foreach (var p in provinces) RegisterProvince(p);
        }

        public bool IsRegistered(string provinceId) => _provinces.ContainsKey(provinceId);

        // ── Queries ───────────────────────────────────────────────

        public string GetOwner(string provinceId)
            => _ownership.TryGetValue(provinceId, out var id) ? id : null;

        public IReadOnlyList<string> GetProvincesByNation(string nationId)
            => _nationProvinces.TryGetValue(nationId, out var list)
               ? list.AsReadOnly()
               : Array.Empty<string>();

        public bool IsOwnedBy(string provinceId, string nationId)
            => GetOwner(provinceId) == nationId;

        public int GetProvinceCount(string nationId)
            => _nationProvinces.TryGetValue(nationId, out var list) ? list.Count : 0;

        public ProvinceRecord GetProvince(string provinceId)
            => _provinces.TryGetValue(provinceId, out var p) ? p : null;

        // ── TransferOwnership ─────────────────────────────────────

        public OwnershipTransferResult TransferOwnership(OwnershipTransferEvent transfer)
        {
            _provinces.TryGetValue(transfer.ProvinceId, out var province);
            var currentOwner = GetOwner(transfer.ProvinceId);

            // Validate
            var validationError = _validator.Validate(transfer, province, currentOwner);
            if (validationError != null)
                return OwnershipTransferResult.Fail(transfer.ProvinceId, validationError);

            // Apply ownership change
            ApplyTransfer(transfer.ProvinceId, currentOwner, transfer.ToNationId);

            // Keep province runtime state synchronized
            province.OwnerNationId = transfer.ToNationId;

            // Record snapshot in history
            var snapshot = new OwnershipSnapshot
            {
                ProvinceId = transfer.ProvinceId,
                NationId = transfer.ToNationId,
                GameYear = transfer.GameYear,
                TurnNumber = transfer.TurnNumber,
                TransferReason = transfer.Reason,
                PreviousNationId = currentOwner
            };
            _history.RecordSnapshot(snapshot);

            var result = OwnershipTransferResult.Ok(transfer.ProvinceId, currentOwner, transfer.ToNationId, snapshot);
            OnOwnershipChanged?.Invoke(result);
            return result;
        }

        public IReadOnlyList<OwnershipTransferResult> BatchTransfer(IEnumerable<OwnershipTransferEvent> transfers)
        {
            var results = new List<OwnershipTransferResult>();
            foreach (var t in transfers)
                results.Add(TransferOwnership(t));
            return results;
        }

        // ── Private Helpers ───────────────────────────────────────

        private void ApplyTransfer(string provinceId, string fromNationId, string toNationId)
        {
            // Remove from old owner's list
            if (fromNationId != null && _nationProvinces.TryGetValue(fromNationId, out var oldList))
                oldList.Remove(provinceId);

            // Update ownership map
            if (toNationId != null)
                _ownership[provinceId] = toNationId;
            else
                _ownership.Remove(provinceId);

            // Add to new owner's list
            if (toNationId != null)
            {
                if (!_nationProvinces.TryGetValue(toNationId, out var newList))
                {
                    newList = new List<string>();
                    _nationProvinces[toNationId] = newList;
                }
                if (!newList.Contains(provinceId))
                    newList.Add(provinceId);
            }
        }
    }
}
