using DarakhsHC_API.Library.Models;
using System;
using System.Collections.Generic;

namespace DarakhsHC_API.Library.SqlAccess.PatientsInfo
{
    public class PatientsInfoAccessWrapper : IPatientsInfoAccess
    {
        public DashboardResponseDto GetDashboardStats(int compId)
        {
            return PatientsInfoAccess.GetDashboardStats(compId);
        }

        public int UpsertPatientAppointment(PatientsAppointmentInfo appointmentInfo)
        {
            return PatientsInfoAccess.UpsertPatientAppointment(appointmentInfo);
        }

        public bool IsSameDayAppointmentExistsForPatient(PatientsAppointmentInfo patientsAppointment)
        {
            return PatientsInfoAccess.IsSameDayAppointmentExistsForPatient(patientsAppointment);
        }

        public List<PatientsAppointmentInfo> GetPatientAppointments(int CompanyId, DateTime fromDt, DateTime toDt)
        {
            return PatientsInfoAccess.GetPatientAppointments(CompanyId, fromDt, toDt);
        }

        public int UpsertPatientsInfo(Models.PatientsInfo patientsInfo)
        {
            return PatientsInfoAccess.UpsertPatientsInfo(patientsInfo);
        }

        public bool HasPatienAlreadyExists(Models.PatientsInfo patientsInfo)
        {
            return PatientsInfoAccess.HasPatienAlreadyExists(patientsInfo);
        }

        public List<Models.PatientsInfo> GetPatientInfo(int CompanyId, DateTime fromDt, DateTime toDt)
        {
            return PatientsInfoAccess.GetPatientInfo(CompanyId, fromDt, toDt);
        }

        public int UpsertPatientSummary(PatientsSummaryInfo patientsSummary)
        {
            return PatientsInfoAccess.UpsertPatientSummary(patientsSummary);
        }

        public List<PatientsSummaryInfo> GetPatientsSummaries(int compId, DateTime fromDt, DateTime toDt)
        {
            return PatientsInfoAccess.GetPatientsSummaries(compId, fromDt, toDt);
        }



        public List<TreatmentsInfo> GetTreatments(int CompanyId)
        {
            return PatientsInfoAccess.GetTreatments(CompanyId);
        }

        public List<ReferencesInfo> GetReferences(int CompanyId)
        {
            return PatientsInfoAccess.GetReferences(CompanyId);
        }

        public List<StateInfo> GetStates()
        {
            return PatientsInfoAccess.GetStates();
        }

        public List<CityInfo> GetCitiesByStateId(int StateId)
        {
            return PatientsInfoAccess.GetCitiesByStateId(StateId);
        }
        
        public List<Models.PatientsInfo> GetPatientsInfoByDate(int CompanyId, DateTime filterdate)
        {
            return PatientsInfoAccess.GetPatientsInfoByDate(CompanyId, filterdate);
        }

        public List<Models.PatientsInfo> GetExistingPatientsInfoByValue(int CompanyId, string value)
        {
            return PatientsInfoAccess.GetExistingPatientsInfoByValue(CompanyId, value);
        }

        public void CreatePatientHistory(int compId, int patientId, int patientSummaryId)
        {
            PatientsInfoAccess.CreatePatientHistory(compId, patientId, patientSummaryId);
        }

        public List<PatientHistory> GetPatientHistoriesById(int compId, int patientId)
        {
           return PatientsInfoAccess.GetPatientHistoriesById(compId, patientId);
        }

        public List<PatientsSummaryInfo> GetPatientsFollowUpDateByDate(int compId, DateTime date)
        {
            return PatientsInfoAccess.GetPatientsFollowUpDateByDate(compId, date);
        }

        public int UpsertPatientEnquiry(PatientEnquiry patientEnquiry)
        {
            return PatientsInfoAccess.UpsertPatientEnquiry(patientEnquiry);
        }

        public List<PatientEnquiry> GetPatientEnquiriesByDate(int compId, DateTime fromEnquiryDt, DateTime toEnquiryDt)
        {
            return PatientsInfoAccess.GetPatientEnquiriesByDate(compId, fromEnquiryDt, toEnquiryDt);
        }
    }
}