namespace EmpiresOfHistory.Map.Models
{
    /// <summary>
    /// Describes a requested province ownership transfer.
    /// Passed to ProvinceOwnershipSystem.TransferOwnership().
    /// </summary>
    public class OwnershipTransferEvent
    {
        public string ProvinceId { get; set; }
        public string FromNationId { get; set; }   // null if unowned
        public string ToNationId { get; set; }     // null to make unowned
        public int GameYear { get; set; }
        public int TurnNumber { get; set; }
        public string Reason { get; set; }         // "conquest", "treaty", "independence", "initial", "cession", "annexation"
        // Future: string EventId (historical event that triggered this)
    }
}
