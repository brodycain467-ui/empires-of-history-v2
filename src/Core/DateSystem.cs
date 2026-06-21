using System;

namespace EmpiresOfHistory.Core
{
    /// <summary>
    /// DateSystem manages historical dates and calendar conversions
    /// Supports BC/AD dating and month-based calculations
    /// </summary>
    public interface IDateSystem
    {
        int Year { get; }
        int Month { get; }
        string FormattedDate { get; }
        
        void SetDate(int year, int month);
        void AdvanceMonths(int months);
        (int year, int month) GetCurrentDate();
        string GetFormattedDate();
        bool IsLeapYear(int year);
    }

    public class DateSystem : IDateSystem
    {
        private int _year;
        private int _month;
        
        // Months in year
        private const int MONTHS_PER_YEAR = 12;
        private const int MIN_MONTH = 1;
        private const int MAX_MONTH = 12;
        
        // Month names for formatting
        private static readonly string[] MONTH_NAMES = new[]
        {
            "January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        };

        public int Year => _year;
        public int Month => _month;
        public string FormattedDate => GetFormattedDate();

        public DateSystem(int startYear = 1, int startMonth = 1)
        {
            if (startMonth < MIN_MONTH || startMonth > MAX_MONTH)
                throw new ArgumentException($"Month must be between {MIN_MONTH} and {MAX_MONTH}");
            
            _year = startYear;
            _month = startMonth;
        }

        /// <summary>
        /// Set the current date
        /// </summary>
        public void SetDate(int year, int month)
        {
            if (month < MIN_MONTH || month > MAX_MONTH)
                throw new ArgumentException($"Month must be between {MIN_MONTH} and {MAX_MONTH}");
            
            _year = year;
            _month = month;
        }

        /// <summary>
        /// Advance date by specified months
        /// </summary>
        public void AdvanceMonths(int months)
        {
            if (months < 0)
                throw new ArgumentException("Months must be non-negative");
            
            int totalMonths = _month + months;
            
            // Calculate year advances
            int yearAdvance = (totalMonths - 1) / MONTHS_PER_YEAR;
            _year += yearAdvance;
            
            // Calculate new month (1-12)
            _month = ((totalMonths - 1) % MONTHS_PER_YEAR) + 1;
        }

        /// <summary>
        /// Get current date as tuple
        /// </summary>
        public (int year, int month) GetCurrentDate()
        {
            return (_year, _month);
        }

        /// <summary>
        /// Get formatted date string (e.g., "January 2011 AD")
        /// </summary>
        public string GetFormattedDate()
        {
            if (_month < MIN_MONTH || _month > MAX_MONTH)
                throw new InvalidOperationException($"Month must be between {MIN_MONTH} and {MAX_MONTH}");

            string yearStr = _year > 0 
                ? $"{_year} AD" 
                : $"{Math.Abs(_year)} BC";
            
            return $"{MONTH_NAMES[_month - 1]} {yearStr}";
        }

        /// <summary>
        /// Check if year is leap year (Gregorian calendar)
        /// </summary>
        public bool IsLeapYear(int year)
        {
            if (year <= 0) return false; // Simplified for BC dates
            return (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
        }

        // Future expansion:
        // - Different calendar systems (Julian, Islamic, etc.)
        // - Historical date accuracy
        // - Date range validation
        // - Time zones
    }
}
