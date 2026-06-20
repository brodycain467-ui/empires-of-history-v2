using EmpiresOfHistory.Map.Models;

namespace EmpiresOfHistory.Map.Systems
{
    /// <summary>
    /// Interface for querying historical ownership data.
    /// Designed for future integration with border_history.json database.
    /// </summary>
    public interface IOwnershipHistory
    {
        // Record
        void RecordSnapshot(OwnershipSnapshot snapshot);

        // Queries — all built to support future historical database hookup
        IReadOnlyList<OwnershipSnapshot> GetHistory(string provinceId);
        OwnershipSnapshot GetSnapshotAt(string provinceId, int gameYear);  // closest snapshot at or before year
        string GetOwnerAt(string provinceId, int gameYear);                // just the nation ID at a given year
        IReadOnlyList<OwnershipSnapshot> GetTransfersBetween(string provinceId, int fromYear, int toYear);
        IReadOnlyList<string> GetProvincesByNationAt(string nationId, int gameYear); // who owned what in year X

        // Bulk
        void RecordBatchSnapshot(IEnumerable<OwnershipSnapshot> snapshots);
        IReadOnlyList<OwnershipSnapshot> GetAllSnapshots(); // for serialization/save
    }
}
