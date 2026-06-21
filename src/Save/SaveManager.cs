using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Godot;

namespace EmpiresOfHistory.Save
{
    /// <summary>
    /// SaveManager handles persistence of game state
    /// Saves to JSON format for data-driven approach
    /// </summary>
    public interface ISaveManager
    {
        Task SaveGameAsync(string saveName, SaveData saveData);
        Task<SaveData> LoadGameAsync(string saveId);
        Task DeleteSaveAsync(string saveId);
        Task<List<SaveInfo>> GetSavesAsync();
        Task<bool> SaveExistsAsync(string saveId);
    }

    public class SaveInfo
    {
        public string SaveId { get; set; } = string.Empty;
        public string SaveName { get; set; } = string.Empty;
        public DateTime SaveTime { get; set; }
        public int Turn { get; set; }
        public int Year { get; set; }
        public string PlayerNation { get; set; } = string.Empty;
        public long FileSizeBytes { get; set; }
    }

    public class SaveManager : ISaveManager
    {
        private readonly string _savePath;
        private readonly JsonSerializerOptions _jsonOptions;

        public SaveManager(string savePath = "data/saves")
        {
            _savePath = savePath;
            
            // Create save directory if it doesn't exist
            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }
            
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = false
            };
        }

        /// <summary>
        /// Save game state to JSON file
        /// </summary>
        public async Task SaveGameAsync(string saveName, SaveData saveData)
        {
            try
            {
                saveData.SaveName = saveName;
                saveData.SaveTime = DateTime.Now;
                
                string fileName = $"{saveData.SaveId}.json";
                string filePath = Path.Combine(_savePath, fileName);
                
                string json = JsonSerializer.Serialize(saveData, _jsonOptions);
                await File.WriteAllTextAsync(filePath, json);
                
                Console.WriteLine($"Game saved: {saveName} (ID: {saveData.SaveId})");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save game: {ex.Message}");
                throw new SaveException($"Failed to save game: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Load game state from JSON file
        /// </summary>
        public async Task<SaveData> LoadGameAsync(string saveId)
        {
            try
            {
                string fileName = $"{saveId}.json";
                string filePath = Path.Combine(_savePath, fileName);
                
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"Save file not found: {saveId}");
                }
                
                string json = await File.ReadAllTextAsync(filePath);
                SaveData saveData = JsonSerializer.Deserialize<SaveData>(json, _jsonOptions)
                    ?? throw new InvalidOperationException($"Failed to deserialize save file: {saveId}");
                
                Console.WriteLine($"Game loaded: {saveData.SaveName}");
                return saveData;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load game: {ex.Message}");
                throw new SaveException($"Failed to load game: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Delete a save file
        /// </summary>
        public async Task DeleteSaveAsync(string saveId)
        {
            try
            {
                string fileName = $"{saveId}.json";
                string filePath = Path.Combine(_savePath, fileName);
                
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Console.WriteLine($"Save deleted: {saveId}");
                }
                
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to delete save: {ex.Message}");
                throw new SaveException($"Failed to delete save: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get list of all available saves
        /// </summary>
        public async Task<List<SaveInfo>> GetSavesAsync()
        {
            var saves = new List<SaveInfo>();
            
            try
            {
                var files = Directory.GetFiles(_savePath, "*.json");
                
                foreach (var filePath in files)
                {
                    try
                    {
                        string json = await File.ReadAllTextAsync(filePath);
                        SaveData? saveData = JsonSerializer.Deserialize<SaveData>(json, _jsonOptions);
                        if (saveData is null) continue;
                        
                        var info = new SaveInfo
                        {
                            SaveId = saveData.SaveId,
                            SaveName = saveData.SaveName,
                            SaveTime = saveData.SaveTime,
                            Turn = saveData.CurrentTurn,
                            Year = saveData.CurrentYear,
                            PlayerNation = saveData.PlayerNationId,
                            FileSizeBytes = new FileInfo(filePath).Length
                        };
                        
                        saves.Add(info);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to load save info from {filePath}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to get saves list: {ex.Message}");
            }
            
            return saves.OrderByDescending(s => s.SaveTime).ToList();
        }

        /// <summary>
        /// Check if a save exists
        /// </summary>
        public async Task<bool> SaveExistsAsync(string saveId)
        {
            string fileName = $"{saveId}.json";
            string filePath = Path.Combine(_savePath, fileName);
            return await Task.FromResult(File.Exists(filePath));
        }
    }

    public class SaveException : Exception
    {
        public SaveException(string message) : base(message) { }
        public SaveException(string message, Exception innerException) 
            : base(message, innerException) { }
    }

    // Future expansion:
    // - Async loading with progress
    // - Cloud save sync
    // - Save file encryption
    // - Backup system
    // - Save versioning and migration
}
