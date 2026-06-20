namespace EmpiresOfHistory.Map.Models
{
    /// <summary>
    /// Immutable snapshot of province ownership at a specific game date.
    /// Stored in OwnershipHistory for historical border database integration.
    /// </summary>
    public class OwnershipSnapshot
    {
        public string ProvinceId { get; init; }
        public string NationId { get; init; }      // owner at this moment
        public int GameYear { get; init; }         // in-game year (negative = BC)
        public int TurnNumber { get; init; }        // absolute turn index
        public string TransferReason { get; init; } // e.g. "conquest", "treaty", "independence", "initial"
        public string PreviousNationId { get; init; } // null if initial/unowned

        // Future: link to EventId that caused this transfer
        // Future: link to LeaderId who authorized it
    }
}
