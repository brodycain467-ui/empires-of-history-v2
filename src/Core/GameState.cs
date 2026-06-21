using System;

namespace EmpiresOfHistoryV2.Core;

public class GameState
{
    public DateTime CurrentDate { get; private set; } = new(2011, 1, 1);
    public int CurrentTurn { get; private set; } = 1;
    public string? SelectedNationId { get; set; }
    public string? SelectedProvinceId { get; set; }

    public void SetState(DateTime currentDate, int currentTurn, string? selectedNationId = null, string? selectedProvinceId = null)
    {
        CurrentDate = currentDate;
        CurrentTurn = currentTurn;
        SelectedNationId = selectedNationId;
        SelectedProvinceId = selectedProvinceId;
    }

    public void AdvanceTurn(int months = 3)
    {
        CurrentTurn++;
        CurrentDate = CurrentDate.AddMonths(months);
    }
}
