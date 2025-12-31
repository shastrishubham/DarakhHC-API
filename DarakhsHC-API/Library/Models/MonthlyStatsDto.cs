namespace DarakhsHC_API.Library.Models
{
    public class MonthlyStatsDto
    {
        public string MonthName { get; set; }
        public int MonthNumber { get; set; }
        public int YearNumber { get; set; }
        public int PatientCount { get; set; }
        public int AppointmentCount { get; set; }
    }
}