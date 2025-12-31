namespace DarakhsHC_API.Library.Models
{
    public class DashboardKpiDto
    {
        public int TodaysAppointmentCount { get; set; }
        public int TodaysPatientCount { get; set; }
        public int ThisMonthPatientCount { get; set; }
        public int TodaysFollowUpCount { get; set; }
        public int TodaysSummaryCount { get; set; }
    }
}