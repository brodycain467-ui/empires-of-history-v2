using EmpiresOfHistory.Map.Models;

namespace EmpiresOfHistory.Map.Systems
{
    internal class OwnershipValidator
    {
        /// <summary>
        /// Validates a transfer. Returns null if valid, or a human-readable failure reason.
        /// </summary>
        public string Validate(OwnershipTransferEvent transfer, ProvinceRecord province, string currentOwner)
        {
            if (province == null)
                return $"Province '{transfer.ProvinceId}' is not registered.";

            if (string.IsNullOrWhiteSpace(transfer.ToNationId) && string.IsNullOrWhiteSpace(transfer.Reason))
                return "ToNationId and Reason cannot both be null/empty.";

            if (transfer.FromNationId != null && currentOwner != transfer.FromNationId)
                return $"Province '{transfer.ProvinceId}' is not owned by '{transfer.FromNationId}' (current owner: '{currentOwner ?? "unowned"}').";

            // Allow self-transfer only if it's an explicit "initial" assignment
            if (transfer.FromNationId == transfer.ToNationId && transfer.Reason != "initial")
                return $"Province '{transfer.ProvinceId}' is already owned by '{transfer.ToNationId}'.";

            // Future validations (leave as TODO comments):
            // TODO: Check if ToNationId exists in NationRegistry
            // TODO: Check if transfer is valid given current diplomacy state
            // TODO: Check if province is locked (treaty protection)

            return null; // valid
        }
    }
}
