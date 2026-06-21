namespace EmpiresOfHistory.Map.Models
{
    /// <summary>
    /// Result of a TransferOwnership() call.
    /// Always returned — never throws on validation failure.
    /// </summary>
    public class OwnershipTransferResult
    {
        public bool Success { get; init; }
        public string ProvinceId { get; init; }
        public string FromNationId { get; init; }
        public string ToNationId { get; init; }
        public string FailureReason { get; init; }  // null if Success=true
        public OwnershipSnapshot SnapshotCreated { get; init; } // null if failed

        public static OwnershipTransferResult Ok(string provinceId, string from, string to, OwnershipSnapshot snapshot)
            => new() { Success = true, ProvinceId = provinceId, FromNationId = from, ToNationId = to, SnapshotCreated = snapshot };

        public static OwnershipTransferResult Fail(string provinceId, string reason)
            => new() { Success = false, ProvinceId = provinceId, FailureReason = reason };
    }
}
