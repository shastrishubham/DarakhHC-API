using DarakhsHC_API.Library.Models;
using DarakhsHC_API.Library.ServerModel;
using DarakhsHC_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DarakhsHC_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PatientsInfoController : ApiController
    {

        #region Dashboard

        [Route("api/PatientsInfo/GetDashboardStats")]
        [HttpGet]
        public DashboardResponseDto GetDashboardStats(int CompanyId)
        {
            return PatientsInfoServer.GetDashboardStats(CompanyId);
        }

        #endregion

        #region Patient Appointment
        [Route("api/PatientsInfo/UpsertPatientAppointment")]
        [HttpPost]
        public PatientCreationResponse UpsertPatientAppointment(PatientsAppointmentInfo appointmentInfo)
        {
            return PatientsInfoServer.UpsertPatientAppointment(appointmentInfo);
        }

        [Route("api/PatientsInfo/CreateAppointmentForExistingPatient")]
        [HttpPost]
        public PatientCreationResponse CreateAppointmentForExistingPatient(PatientsAppointmentInfo appointmentInfo)
        {
            return PatientsInfoServer.CreateAppointmentForExistingPatient(appointmentInfo);
        }

        [Route("api/PatientsInfo/GetPatientAppointments")]
        [HttpGet]
        public List<PatientsAppointmentInfo> GetPatientAppointments(int CompanyId, DateTime fromDt, DateTime toDt)
        {
            return PatientsInfoServer.GetPatientAppointments(CompanyId, fromDt.Date, toDt.Date);
        }

        // Todo: make provision for delete appointment

        #endregion 

        [Route("api/PatientsInfo/UpsertPatientsInfo")]
        [HttpPost]
        public PatientCreationResponse UpsertPatientsInfo(Library.Models.PatientsInfo patientsInfo, bool isCreateNewRecord = false)
        {
            return PatientsInfoServer.UpsertPatientsInfo(patientsInfo, isCreateNewRecord);
        }

        [Route("api/PatientsInfo/GetPatientInfo")]
        [HttpGet]
        public List<Library.Models.PatientsInfo> GetPatientInfo(int CompanyId, DateTime fromDt, DateTime toDt)
        {
            return PatientsInfoServer.GetPatientInfo(CompanyId, fromDt.Date, toDt.Date);
        }


        [Route("api/PatientsInfo/UpsertPatientSummary")]
        [HttpPost]
        public int UpsertPatientSummary(Library.Models.PatientsSummaryInfo patientsSummaryInfo)
        {
            return PatientsInfoServer.UpsertPatientSummary(patientsSummaryInfo);
        }

        [Route("api/PatientsInfo/GetPatientsSummaries")]
        [HttpGet]
        public List<PatientsSummaryInfo> GetPatientsSummaries(int CompanyId, DateTime fromDt, DateTime toDt)
        {
            return PatientsInfoServer.GetPatientsSummaries(CompanyId, fromDt.Date, toDt.Date);
        }

        [Route("api/PatientsInfo/GetPatientsFollowUpListByDate")]
        [HttpGet]
        public List<PatientsSummaryInfo> GetPatientsFollowUpListByDate(int compId, DateTime followUpDt)
        {
            return PatientsInfoServer.GetPatientsFollowUpDateByDate(compId, followUpDt.Date);
        }

        //[Route("api/PatientsInfo/GetTodaysPatientsInfo")]
        //[HttpGet]
        //public List<Library.Models.PatientsInfo> GetTodaysPatientsInfo(int CompanyId)
        //{
        //    return PatientsInfoServer.GetTodaysPatientsInfo(CompanyId);
        //}

        //[Route("api/PatientsInfo/CreateExistingPatientAppointment")]
        //[HttpPost]
        //public void CreateExistingPatientAppointment(PatientsInfo patientsInfo)
        //{
        //    PatientsInfoServer.CreateExistingPatientAppointment(patientsInfo);
        //}

        [Route("api/PatientsInfo/GetExistingPatientsInfoByValue")]
        [HttpGet]
        public List<PatientsInfo> GetExistingPatientsInfoByValue(int CompanyId, string value)
        {
            return PatientsInfoServer.GetExistingPatientsInfoByValue(CompanyId, value);
        }

        [Route("api/PatientsInfo/GetPatientHistoriesById")]
        [HttpGet]
        public List<PatientHistory> GetPatientHistoriesById(int compId, int patientId)
        {
            return PatientsInfoServer.GetPatientHistoriesById(compId, patientId);
        }

        [Route("api/PatientsInfo/UpsertPatientEnquiry")]
        [HttpPost]
        public int UpsertPatientEnquiry(PatientEnquiry patientEnquiry)
        {
            return PatientsInfoServer.UpsertPatientEnquiry(patientEnquiry);
        }

        [Route("api/PatientsInfo/GetPatientEnquiriesByDate")]
        [HttpGet]
        public List<PatientEnquiry> GetPatientEnquiriesByDate(int compId, DateTime fromEnquiryDt, DateTime toEnquiryDt)
        {
            return PatientsInfoServer.GetPatientEnquiriesByDate(compId, fromEnquiryDt, toEnquiryDt);
        }

        #region Master APIs
        [Route("api/PatientsInfo/GetTreatments")]
        [HttpGet]
        public List<TreatmentsInfo> GetTreatments(int CompanyId)
        {
            return PatientsInfoServer.GetTreatments(CompanyId);
        }

        [Route("api/PatientsInfo/GetReferences")]
        [HttpGet]
        public List<ReferencesInfo> GetReferences(int CompanyId)
        {
            return PatientsInfoServer.GetReferences(CompanyId);
        }

        [Route("api/PatientsInfo/GetStates")]
        [HttpGet]
        public List<StateInfo> GetStates()
        {
            return PatientsInfoServer.GetStates();
        }

        [Route("api/PatientsInfo/GetCitiesByStateId")]
        [HttpGet]
        public List<CityInfo> GetCitiesByStateId(int StateId)
        {
            return PatientsInfoServer.GetCitiesByStateId(StateId);
        }

        #endregion
    }
}
