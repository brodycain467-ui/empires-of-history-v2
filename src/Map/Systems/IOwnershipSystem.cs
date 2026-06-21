using EmpiresOfHistory.Map.Models;

namespace EmpiresOfHistory.Map.Systems
{
    public interface IOwnershipSystem
    {
        // Setup
        void RegisterProvince(ProvinceRecord province);
        void RegisterProvinces(IEnumerable<ProvinceRecord> provinces);
        bool IsRegistered(string provinceId);

        // Queries
        string GetOwner(string provinceId);                           // returns null if unowned
        IReadOnlyList<string> GetProvincesByNation(string nationId);  // all provinces owned by a nation
        bool IsOwnedBy(string provinceId, string nationId);
        int GetProvinceCount(string nationId);
        ProvinceRecord GetProvince(string provinceId);

        // Transfers
        OwnershipTransferResult TransferOwnership(OwnershipTransferEvent transfer);
        IReadOnlyList<OwnershipTransferResult> BatchTransfer(IEnumerable<OwnershipTransferEvent> transfers);

        // Events
        event Action<OwnershipTransferResult> OnOwnershipChanged;
    }
}
