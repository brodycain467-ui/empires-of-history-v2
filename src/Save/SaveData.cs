using System;
using System.Collections.Generic;
using EmpiresOfHistory.Core;
using EmpiresOfHistory.Data.Models;

namespace EmpiresOfHistory.Save
{
    /// <summary>
    /// SaveData represents the complete game state to be persisted
    /// Includes all necessary data to restore game to exact point
    /// </summary>
    public class SaveData
    {
        public string SaveName { get; set; }
        public string SaveId { get; set; }
        public DateTime SaveTime { get; set; }
        public string Version { get; set; }
        
        // Game state
        public int CurrentTurn { get; set; }
        public int CurrentYear { get; set; }
        public int CurrentMonth { get; set; }
        public int TurnLengthMonths { get; set; }
        
        // Gameplay state
        public string PlayerNationId { get; set; }
        public GameDifficulty Difficulty { get; set; }
        public Dictionary<string, int> PlayerStats { get; set; }
        
        // Province and nation states
        public List<ProvinceModel> Provinces { get; set; }
        public List<NationModel> Nations { get; set; }
        
        // Ownership history
        public List<OwnershipRecord> OwnershipHistory { get; set; }
        
        // Metadata
        public Dictionary<string, object> GameSettings { get; set; }
        public Dictionary<string, object> CustomData { get; set; }

        public SaveData()
        {
            SaveId = Guid.NewGuid().ToString();
            SaveTime = DateTime.Now;
            Version = "0.1.0";
            PlayerStats = new Dictionary<string, int>();
            Provinces = new List<ProvinceModel>();
            Nations = new List<NationModel>();
            OwnershipHistory = new List<OwnershipRecord>();
            GameSettings = new Dictionary<string, object>();
            CustomData = new Dictionary<string, object>();
        }
    }

    public class OwnershipRecord
    {
        public string ProvinceId { get; set; }
        public string NationId { get; set; }
        public int StartYear { get; set; }
        public int? EndYear { get; set; }
        public string Reason { get; set; }
    }

    public enum GameDifficulty
    {
        Easy,
        Normal,
        Hard,
        Impossible
    }

    // Future expansion:
    // - Compressed save format
    // - Incremental saves
    // - Cloud save sync
    // - Save file verification
}
