using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarakhsHC_API.Library.Models
{
    public class TreatmentsMonthlyStatsDto
    {
        public string MonthName { get; set; }
        public int MonthNumber { get; set; }
        public int YearNumber { get; set; }
        public int HearingCount { get; set; }
        public int SpeakingCount { get; set; }
    }
}