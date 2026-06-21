using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;

namespace EmpiresOfHistoryV2.Core;


public class SaveSystem
{
    private const string SaveDir = "user://saves/";
    private const int MaxSlots = 5;
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    public void Save(int slot, GameState state)
    {
        ValidateSlot(slot);
        EnsureSaveDirectory();

        var nationId = state.SelectedNationId ?? string.Empty;
        var nation = GameManager.Instance.ContentDatabase.GetNation(nationId);
        var nationName = nation?.DisplayName ?? (string.IsNullOrWhiteSpace(nationId) ? "Unknown Nation" : nationId);

        var payload = new SaveData
        {
            Slot = slot,
            SaveName = $"{nationName} — {state.CurrentDate:yyyy} AD",
            DateTime = System.DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture),
            SelectedNationId = nationId,
            CurrentTurn = state.CurrentTurn,
            GameDate = state.CurrentDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
        };

        var json = JsonSerializer.Serialize(payload, _jsonOptions);
        using var file = Godot.FileAccess.Open(GetSlotPath(slot), Godot.FileAccess.ModeFlags.Write);
        file.StoreString(json);

        ApplyToGameState(payload);
    }

    public SaveData? Load(int slot)
    {
        ValidateSlot(slot);
        var path = GetSlotPath(slot);
        if (!Godot.FileAccess.FileExists(path))
        {
            return null;
        }

        using var file = Godot.FileAccess.Open(path, Godot.FileAccess.ModeFlags.Read);
        var json = file.GetAsText();
        var data = JsonSerializer.Deserialize<SaveData>(json);
        if (data == null)
        {
            return null;
        }

        ApplyToGameState(data);
        return data;
    }

    public List<SaveData?> GetAllSaveSlots()
    {
        var result = new List<SaveData?>(MaxSlots);
        for (var slot = 1; slot <= MaxSlots; slot++)
        {
            result.Add(LoadFromDisk(slot));
        }

        return result;
    }

    public void DeleteSave(int slot)
    {
        ValidateSlot(slot);
        var path = GetSlotPath(slot);
        if (Godot.FileAccess.FileExists(path))
        {
            Godot.DirAccess.RemoveAbsolute(path);
        }
    }

    public bool SlotExists(int slot)
    {
        ValidateSlot(slot);
        return Godot.FileAccess.FileExists(GetSlotPath(slot));
    }

    private SaveData? LoadFromDisk(int slot)
    {
        var path = GetSlotPath(slot);
        if (!Godot.FileAccess.FileExists(path))
        {
            return null;
        }

        using var file = Godot.FileAccess.Open(path, Godot.FileAccess.ModeFlags.Read);
        var json = file.GetAsText();
        return JsonSerializer.Deserialize<SaveData>(json);
    }

    private static string GetSlotPath(int slot) => $"{SaveDir}save_{slot}.json";

    private static void EnsureSaveDirectory()
    {
        Godot.DirAccess.MakeDirRecursiveAbsolute(SaveDir);
    }

    private static void ApplyToGameState(SaveData data)
    {
        if (!System.DateTime.TryParseExact(data.GameDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var gameDate))
        {
            gameDate = new System.DateTime(2011, 1, 1);
        }

        GameManager.Instance.GameState.SetState(gameDate, data.CurrentTurn, data.SelectedNationId, GameManager.Instance.GameState.SelectedProvinceId);
    }

    private static void ValidateSlot(int slot)
    {
        if (slot is < 1 or > MaxSlots)
        {
            throw new ArgumentOutOfRangeException(nameof(slot), "Slot must be between 1 and 5.");
        }
    }
}
