using DarakhsHC_API.Library.Models;
using System;
using System.Collections.Generic;


namespace DarakhsHC_API.Library.SqlAccess.PatientsInfo
{
    public interface IPatientsInfoAccess
    {
        #region Dashboard

        DashboardResponseDto GetDashboardStats(int compId);

        #endregion

        #region Patient Appointement
        int UpsertPatientAppointment(PatientsAppointmentInfo appointmentInfo);

        bool IsSameDayAppointmentExistsForPatient(PatientsAppointmentInfo patientsAppointmentInfo);

        List<PatientsAppointmentInfo> GetPatientAppointments(int CompanyId, DateTime fromDt, DateTime toDt);
        #endregion

        #region Patient Record

        bool HasPatienAlreadyExists(Library.Models.PatientsInfo patientsInfo);

        int UpsertPatientsInfo(Library.Models.PatientsInfo patientsInfo);

        List<Library.Models.PatientsInfo> GetPatientInfo(int CompanyId, DateTime fromDt, DateTime toDt);

        List<Library.Models.PatientsInfo> GetExistingPatientsInfoByValue(int CompanyId, string value);

        List<TreatmentsInfo> GetTreatments(int CompanyId);

        List<ReferencesInfo> GetReferences(int CompanyId);

        List<StateInfo> GetStates();

        List<CityInfo> GetCitiesByStateId(int StateId);

        List<Library.Models.PatientsInfo> GetPatientsInfoByDate(int CompanyId, DateTime filterdate);
        #endregion

        #region Patient Summary And Inventory
        int UpsertPatientSummary(PatientsSummaryInfo patientsSummary);

        List<PatientsSummaryInfo> GetPatientsSummaries(int compId, DateTime fromDt, DateTime toDt);
        #endregion

        #region PatientHistory

        void CreatePatientHistory(int compId, int patientId, int patientSummaryId);

        List<PatientHistory> GetPatientHistoriesById(int compId, int patientId);
        #endregion

        #region Patient FollowUp Date

        List<PatientsSummaryInfo> GetPatientsFollowUpDateByDate(int compId, DateTime date);

        #endregion
    }
}
