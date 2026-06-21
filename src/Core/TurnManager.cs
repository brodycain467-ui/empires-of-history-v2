using System;
using System.Collections.Generic;

namespace EmpiresOfHistory.Core
{
    /// <summary>
    /// TurnManager handles turn progression and turn-based mechanics
    /// Coordinates with DateSystem to track game time
    /// </summary>
    public interface ITurnManager
    {
        int CurrentTurn { get; }
        int CurrentYear { get; }
        int CurrentMonth { get; }
        int TurnLengthMonths { get; }
        
        event EventHandler<TurnEventArgs>? TurnAdvanced;
        event EventHandler<YearChangeEventArgs>? YearChanged;
        
        void AdvanceTurn();
        void SetTurn(int turnNumber);
        void Initialize(int startYear, int startMonth, int turnLengthMonths = 3);
    }

    public class TurnEventArgs : EventArgs
    {
        public int PreviousTurn { get; set; }
        public int NewTurn { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }

    public class YearChangeEventArgs : EventArgs
    {
        public int PreviousYear { get; set; }
        public int NewYear { get; set; }
    }

    public class TurnManager : ITurnManager
    {
        private int _currentTurn;
        private int _turnLengthMonths;
        private IDateSystem _dateSystem;
        
        public int CurrentTurn => _currentTurn;
        public int CurrentYear => _dateSystem.Year;
        public int CurrentMonth => _dateSystem.Month;
        public int TurnLengthMonths => _turnLengthMonths;
        
        public event EventHandler<TurnEventArgs>? TurnAdvanced;
        public event EventHandler<YearChangeEventArgs>? YearChanged;

        public TurnManager()
        {
            _dateSystem = new DateSystem();
            _currentTurn = 0;
            _turnLengthMonths = 3;
        }

        public void Initialize(int startYear, int startMonth, int turnLengthMonths = 3)
        {
            _dateSystem.SetDate(startYear, startMonth);
            _turnLengthMonths = turnLengthMonths;
            _currentTurn = 0;
        }

        /// <summary>
        /// Advance to next turn
        /// </summary>
        public void AdvanceTurn()
        {
            int previousTurn = _currentTurn;
            int previousYear = _dateSystem.Year;
            
            // Advance date by turn length
            _dateSystem.AdvanceMonths(_turnLengthMonths);
            _currentTurn++;
            
            // Emit turn advanced event
            TurnAdvanced?.Invoke(this, new TurnEventArgs
            {
                PreviousTurn = previousTurn,
                NewTurn = _currentTurn,
                Year = _dateSystem.Year,
                Month = _dateSystem.Month
            });
            
            // Check for year change
            if (_dateSystem.Year != previousYear)
            {
                YearChanged?.Invoke(this, new YearChangeEventArgs
                {
                    PreviousYear = previousYear,
                    NewYear = _dateSystem.Year
                });
            }
        }

        /// <summary>
        /// Set current turn to specific value
        /// </summary>
        public void SetTurn(int turnNumber)
        {
            if (turnNumber < 0)
                throw new ArgumentException("Turn number cannot be negative");
            
            _currentTurn = turnNumber;
            
            // Recalculate date based on turns
            _dateSystem.SetDate(1, 1);
            if (turnNumber > 0)
            {
                _dateSystem.AdvanceMonths(turnNumber * _turnLengthMonths);
            }
        }

        // Future expansion:
        // - Pause system
        // - Speed controls
        // - Historical event triggers by turn
        // - Multiple calendars
    }
}
