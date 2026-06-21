using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmpiresOfHistory.Core;
using EmpiresOfHistory.Data.Models;
using EmpiresOfHistory.Save;
using Xunit;

namespace EmpiresOfHistory.Tests
{
    /// <summary>
    /// Phase 1 Validation Tests
    /// Validates core systems: Date, Turn Manager, Nation, Province, Save/Load
    /// Test Scenario:
    /// - Start Date: January 1, 2011
    /// - Nation: United States
    /// - 10 Sample Provinces
    /// - Advance 4 turns (3 months each)
    /// - Save Game
    /// - Load Game
    /// - Verify all data persists
    /// </summary>
    public class Phase1ValidationTests
    {
        private DateSystem _dateSystem;
        private TurnManager _turnManager;
        private SaveManager _saveManager;
        private SaveData _testSaveData;

        public Phase1ValidationTests()
        {
            _dateSystem = new DateSystem();
            _turnManager = new TurnManager();
            _saveManager = new SaveManager();
        }

        [Fact]
        public void Test_DateSystem_Initialization()
        {
            // Arrange
            _dateSystem.SetDate(2011, 1);
            
            // Act
            var (year, month) = _dateSystem.GetCurrentDate();
            string formatted = _dateSystem.GetFormattedDate();
            
            // Assert
            Assert.Equal(2011, year);
            Assert.Equal(1, month);
            Assert.Contains("2011", formatted);
            Assert.Contains("AD", formatted);
        }

        [Fact]
        public void Test_DateSystem_AdvanceMonths()
        {
            // Arrange
            _dateSystem.SetDate(2011, 1);
            
            // Act
            _dateSystem.AdvanceMonths(3);
            
            // Assert
            Assert.Equal(2011, _dateSystem.Year);
            Assert.Equal(4, _dateSystem.Month);
        }

        [Fact]
        public void Test_DateSystem_YearRollover()
        {
            // Arrange
            _dateSystem.SetDate(2011, 10);
            
            // Act
            _dateSystem.AdvanceMonths(6); // 10 + 6 = 16 months = 1 year + 4 months
            
            // Assert
            Assert.Equal(2012, _dateSystem.Year);
            Assert.Equal(4, _dateSystem.Month);
        }

        [Fact]
        public void Test_TurnManager_Initialization()
        {
            // Arrange & Act
            _turnManager.Initialize(2011, 1, 3);
            
            // Assert
            Assert.Equal(0, _turnManager.CurrentTurn);
            Assert.Equal(2011, _turnManager.CurrentYear);
            Assert.Equal(1, _turnManager.CurrentMonth);
            Assert.Equal(3, _turnManager.TurnLengthMonths);
        }

        [Fact]
        public void Test_TurnManager_AdvanceTurns()
        {
            // Arrange
            _turnManager.Initialize(2011, 1, 3);
            int turnAdvancedCount = 0;
            _turnManager.TurnAdvanced += (s, e) => turnAdvancedCount++;
            
            // Act - Advance 4 turns
            for (int i = 0; i < 4; i++)
            {
                _turnManager.AdvanceTurn();
            }
            
            // Assert
            Assert.Equal(4, _turnManager.CurrentTurn);
            Assert.Equal(4, turnAdvancedCount); // Event fired 4 times
            Assert.Equal(2012, _turnManager.CurrentYear); // Rolls into January of the next year
            Assert.Equal(1, _turnManager.CurrentMonth); // Back to January (1 + 12 months)
        }

        [Fact]
        public void Test_ProvinceModel_Creation()
        {
            // Arrange & Act
            var province = new ProvinceModel
            {
                Id = "prov_california",
                Name = "California",
                Description = "State on the West Coast",
                Latitude = 36.7783f,
                Longitude = -119.4179f,
                Terrain = "varied",
                Area = 423970,
                Population = 39500000,
                DevelopmentLevel = 0.95f,
                OwnerId = "nat_usa"
            };
            
            // Assert
            Assert.Equal("prov_california", province.Id);
            Assert.Equal("California", province.Name);
            Assert.Equal("nat_usa", province.OwnerId);
            Assert.Equal(39500000, province.Population);
        }

        [Fact]
        public void Test_NationModel_Creation()
        {
            // Arrange & Act
            var nation = new NationModel
            {
                Id = "nat_usa",
                Name = "United States of America",
                DisplayName = "United States",
                Color = "#4169E1",
                GovernmentType = "federal_republic",
                PrimaryCulture = "cult_american",
                PrimaryReligion = "rel_protestantism",
                FoundingYear = 1776,
                IsActive = true
            };
            
            // Assert
            Assert.Equal("nat_usa", nation.Id);
            Assert.Equal("United States of America", nation.Name);
            Assert.Equal(1776, nation.FoundingYear);
            Assert.True(nation.IsActive);
        }

        [Fact]
        public void Test_SaveData_Creation()
        {
            // Arrange & Act
            var saveData = new SaveData
            {
                SaveName = "Test Save",
                CurrentTurn = 4,
                CurrentYear = 2011,
                CurrentMonth = 1,
                TurnLengthMonths = 3,
                PlayerNationId = "nat_usa",
                Difficulty = GameDifficulty.Normal
            };
            
            // Assert
            Assert.Equal("Test Save", saveData.SaveName);
            Assert.Equal(4, saveData.CurrentTurn);
            Assert.NotEqual(Guid.Empty.ToString(), saveData.SaveId);
            Assert.NotNull(saveData.SaveTime);
        }

        [Fact]
        public void Test_CreateSampleGameData()
        {
            // Arrange - Create USA nation
            var usa = new NationModel
            {
                Id = "nat_usa",
                Name = "United States of America",
                Color = "#4169E1",
                GovernmentType = "federal_republic",
                PrimaryCulture = "cult_american",
                IsActive = true
            };
            
            // Create 10 sample provinces (US states)
            var provinces = new List<ProvinceModel>
            {
                new ProvinceModel { Id = "prov_ca", Name = "California", OwnerId = "nat_usa", Population = 39500000 },
                new ProvinceModel { Id = "prov_tx", Name = "Texas", OwnerId = "nat_usa", Population = 29145505 },
                new ProvinceModel { Id = "prov_fl", Name = "Florida", OwnerId = "nat_usa", Population = 21538187 },
                new ProvinceModel { Id = "prov_ny", Name = "New York", OwnerId = "nat_usa", Population = 19453561 },
                new ProvinceModel { Id = "prov_pa", Name = "Pennsylvania", OwnerId = "nat_usa", Population = 12802503 },
                new ProvinceModel { Id = "prov_il", Name = "Illinois", OwnerId = "nat_usa", Population = 12671821 },
                new ProvinceModel { Id = "prov_oh", Name = "Ohio", OwnerId = "nat_usa", Population = 11799448 },
                new ProvinceModel { Id = "prov_ga", Name = "Georgia", OwnerId = "nat_usa", Population = 10711908 },
                new ProvinceModel { Id = "prov_nc", Name = "North Carolina", OwnerId = "nat_usa", Population = 10439388 },
                new ProvinceModel { Id = "prov_mi", Name = "Michigan", OwnerId = "nat_usa", Population = 9984072 }
            };
            
            // Assert
            Assert.Single(new[] { usa });
            Assert.Equal(10, provinces.Count);
            Assert.All(provinces, p => Assert.Equal("nat_usa", p.OwnerId));
        }

        [Fact]
        public async Task Test_SaveGame_SaveData()
        {
            // Arrange
            var saveData = new SaveData
            {
                SaveName = "Phase1Test",
                CurrentTurn = 4,
                CurrentYear = 2011,
                CurrentMonth = 1,
                TurnLengthMonths = 3,
                PlayerNationId = "nat_usa",
                Difficulty = GameDifficulty.Normal
            };
            
            // Add test nation
            saveData.Nations.Add(new NationModel
            {
                Id = "nat_usa",
                Name = "United States",
                Color = "#4169E1",
                IsActive = true
            });
            
            // Add test provinces
            for (int i = 0; i < 10; i++)
            {
                saveData.Provinces.Add(new ProvinceModel
                {
                    Id = $"prov_{i}",
                    Name = $"Province {i}",
                    OwnerId = "nat_usa",
                    Population = 1000000 + (i * 100000)
                });
            }
            
            // Act
            await _saveManager.SaveGameAsync("Phase1Test", saveData);
            
            // Assert
            bool exists = await _saveManager.SaveExistsAsync(saveData.SaveId);
            Assert.True(exists);
        }

        [Fact]
        public async Task Test_LoadGame_RecoverData()
        {
            // Arrange - Create and save data
            var originalSaveData = new SaveData
            {
                SaveName = "Phase1TestLoad",
                CurrentTurn = 4,
                CurrentYear = 2011,
                CurrentMonth = 1,
                TurnLengthMonths = 3,
                PlayerNationId = "nat_usa"
            };
            
            originalSaveData.Nations.Add(new NationModel
            {
                Id = "nat_usa",
                Name = "United States",
                IsActive = true
            });
            
            for (int i = 0; i < 10; i++)
            {
                originalSaveData.Provinces.Add(new ProvinceModel
                {
                    Id = $"prov_test_{i}",
                    Name = $"Test Province {i}",
                    OwnerId = "nat_usa",
                    Population = 1000000 + (i * 100000)
                });
            }
            
            await _saveManager.SaveGameAsync("Phase1TestLoad", originalSaveData);
            
            // Act
            var loadedSaveData = await _saveManager.LoadGameAsync(originalSaveData.SaveId);
            
            // Assert - Verify all data persists
            Assert.Equal(originalSaveData.SaveName, loadedSaveData.SaveName);
            Assert.Equal(originalSaveData.CurrentTurn, loadedSaveData.CurrentTurn);
            Assert.Equal(originalSaveData.CurrentYear, loadedSaveData.CurrentYear);
            Assert.Equal(originalSaveData.CurrentMonth, loadedSaveData.CurrentMonth);
            Assert.Equal(originalSaveData.PlayerNationId, loadedSaveData.PlayerNationId);
            Assert.Single(loadedSaveData.Nations);
            Assert.Equal(10, loadedSaveData.Provinces.Count);
            Assert.All(loadedSaveData.Provinces, p => Assert.Equal("nat_usa", p.OwnerId));
        }

        [Fact]
        public async Task Test_FullGameCycle()
        {
            // Arrange - Initialize game systems
            _turnManager.Initialize(2011, 1, 3);
            
            var usa = new NationModel
            {
                Id = "nat_usa",
                Name = "United States",
                IsActive = true
            };
            
            var provinces = Enumerable.Range(0, 10)
                .Select(i => new ProvinceModel
                {
                    Id = $"prov_{i}",
                    Name = $"Province {i}",
                    OwnerId = "nat_usa",
                    Population = 1000000 + (i * 100000)
                })
                .ToList();
            
            // Act - Advance 4 turns
            for (int i = 0; i < 4; i++)
            {
                _turnManager.AdvanceTurn();
            }
            
            // Create save data
            var saveData = new SaveData
            {
                SaveName = "Phase1Complete",
                CurrentTurn = _turnManager.CurrentTurn,
                CurrentYear = _turnManager.CurrentYear,
                CurrentMonth = _turnManager.CurrentMonth,
                TurnLengthMonths = _turnManager.TurnLengthMonths,
                PlayerNationId = usa.Id
            };
            
            saveData.Nations.Add(usa);
            saveData.Provinces.AddRange(provinces);
            
            // Save
            await _saveManager.SaveGameAsync("Phase1Complete", saveData);
            
            // Load
            var loadedData = await _saveManager.LoadGameAsync(saveData.SaveId);
            
            // Assert - Verify everything persisted correctly
            Assert.Equal(4, loadedData.CurrentTurn);
            Assert.Equal(2012, loadedData.CurrentYear);
            Assert.Equal(1, loadedData.CurrentMonth);
            Assert.Equal(10, loadedData.Provinces.Count);
            Assert.Single(loadedData.Nations);
            Assert.All(loadedData.Provinces, p => Assert.Equal(1000000 + int.Parse(p.Id.Split('_')[1]) * 100000, p.Population));
        }
    }
}
