using EmpiresOfHistory.Map.Models;

namespace EmpiresOfHistory.Map.Systems
{
    public class OwnershipHistory : IOwnershipHistory
    {
        // provinceId → sorted list of snapshots (by TurnNumber)
        private readonly Dictionary<string, List<OwnershipSnapshot>> _history = new();

        public void RecordSnapshot(OwnershipSnapshot snapshot)
        {
            if (snapshot == null || string.IsNullOrWhiteSpace(snapshot.ProvinceId))
                return;

            if (!_history.TryGetValue(snapshot.ProvinceId, out var snapshots))
            {
                snapshots = new List<OwnershipSnapshot>();
                _history[snapshot.ProvinceId] = snapshots;
            }

            var insertIndex = snapshots.BinarySearch(snapshot, TurnNumberComparer.Instance);
            if (insertIndex < 0)
                insertIndex = ~insertIndex;

            snapshots.Insert(insertIndex, snapshot);
        }

        public void RecordBatchSnapshot(IEnumerable<OwnershipSnapshot> snapshots)
        {
            if (snapshots == null)
                return;

            foreach (var snapshot in snapshots)
                RecordSnapshot(snapshot);
        }

        public IReadOnlyList<OwnershipSnapshot> GetHistory(string provinceId)
        {
            if (provinceId == null || !_history.TryGetValue(provinceId, out var snapshots))
                return Array.Empty<OwnershipSnapshot>();

            return snapshots.AsReadOnly();
        }

        public OwnershipSnapshot GetSnapshotAt(string provinceId, int gameYear)
        {
            if (provinceId == null || !_history.TryGetValue(provinceId, out var snapshots) || snapshots.Count == 0)
                return null;

            OwnershipSnapshot candidate = null;
            for (var i = 0; i < snapshots.Count; i++)
            {
                var snapshot = snapshots[i];
                if (snapshot.GameYear <= gameYear &&
                    (candidate == null || snapshot.GameYear >= candidate.GameYear))
                {
                    candidate = snapshot;
                }
            }

            return candidate;
        }

        public string GetOwnerAt(string provinceId, int gameYear)
            => GetSnapshotAt(provinceId, gameYear)?.NationId;

        public IReadOnlyList<OwnershipSnapshot> GetTransfersBetween(string provinceId, int fromYear, int toYear)
        {
            if (provinceId == null || !_history.TryGetValue(provinceId, out var snapshots))
                return Array.Empty<OwnershipSnapshot>();

            return snapshots
                .Where(s => s.GameYear >= fromYear && s.GameYear <= toYear)
                .ToList()
                .AsReadOnly();
        }

        public IReadOnlyList<string> GetProvincesByNationAt(string nationId, int gameYear)
        {
            if (nationId == null)
                return Array.Empty<string>();

            var provinces = new List<string>();
            foreach (var provinceId in _history.Keys)
            {
                var owner = GetOwnerAt(provinceId, gameYear);
                if (owner == nationId)
                    provinces.Add(provinceId);
            }

            return provinces.AsReadOnly();
        }

        public IReadOnlyList<OwnershipSnapshot> GetAllSnapshots()
            => _history.Values.SelectMany(x => x).OrderBy(x => x.TurnNumber).ToList().AsReadOnly();

        private sealed class TurnNumberComparer : IComparer<OwnershipSnapshot>
        {
            public static readonly TurnNumberComparer Instance = new();

            public int Compare(OwnershipSnapshot x, OwnershipSnapshot y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (x is null) return -1;
                if (y is null) return 1;
                return x.TurnNumber.CompareTo(y.TurnNumber);
            }
        }
    }
}
