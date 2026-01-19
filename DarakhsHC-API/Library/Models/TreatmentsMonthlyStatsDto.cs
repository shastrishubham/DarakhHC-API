using System.Collections.Generic;

namespace DarakhsHC_API.Library.Models
{
    public class TreatmentsMonthlyStatsDto
    {
        public string MonthName { get; set; }
        public int MonthNumber { get; set; }
        public int YearNumber { get; set; }


        // Key = Treatment Name (Hearing, Speaking, Therapy, etc)
        // Value = Count
        public Dictionary<string, int> TreatmentCounts { get; set; } = new Dictionary<string, int>();
    }
}