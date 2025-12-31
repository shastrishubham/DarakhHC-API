using System.Collections.Generic;

namespace DarakhsHC_API.Library.Models
{
    public class DashboardResponseDto
    {
        public DashboardKpiDto Kpis { get; set; }
        public List<HourlyAppointmentDto> HourlyAppointments { get; set; }
        public List<MonthlyStatsDto> MonthlyStats { get; set; }
        public List<DonutChartDto> AppointmentVsWalkin { get; set; }
        public List<TreatmentsMonthlyStatsDto> TreatmentsMonthlyStatsDto { get; set; }
        public  List<TreatmentsDonutChartDto> TreatmentsDonutChartDto { get; set; }
    }
}