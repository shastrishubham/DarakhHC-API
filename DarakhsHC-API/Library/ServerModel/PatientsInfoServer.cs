using DarakhsHC_API.Library.Models;
using DarakhsHC_API.Library.SqlAccess.MastersInfo;
using DarakhsHC_API.Library.SqlAccess.PatientsInfo;
using DarakhsHC_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DarakhsHC_API.Library.ServerModel
{
    public class PatientsInfoServer
    {
        #region Properties Interface

        public static IPatientsInfoAccess mPatientsInfoAccessT = new PatientsInfoAccessWrapper();
        public static IMastersInfoAccess mMasterssInfoAccessT = new MastersInfoAccessWrapper();

        #endregion

        public static DashboardResponseDto GetDashboardStats(int compId)
        {
            return mPatientsInfoAccessT.GetDashboardStats(compId);
        }

        public static PatientCreationResponse UpsertPatientAppointment(PatientsAppointmentInfo appointmentInfo)
        {
            if (appointmentInfo.Id == 0)
            {
                bool IsSameDayAppointmentExistsForPatient = mPatientsInfoAccessT.IsSameDayAppointmentExistsForPatient(appointmentInfo);

                if (IsSameDayAppointmentExistsForPatient)
                {
                    return new PatientCreationResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "An appointment for this patient (same name and mobile number) already exists on the selected date."
                    };
                }
            }

            return UpsertAppointment(appointmentInfo);
        }

        public static PatientCreationResponse CreateAppointmentForExistingPatient(PatientsAppointmentInfo appointmentInfo)
        {
            bool IsSameDayAppointmentExistsForPatient = mPatientsInfoAccessT.IsSameDayAppointmentExistsForPatient(appointmentInfo);

            if (IsSameDayAppointmentExistsForPatient)
            {
                return new PatientCreationResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "An appointment for this patient (same name and mobile number) already exists on the selected date."
                };
            }

            return UpsertAppointment(appointmentInfo);
        }

        private static PatientCreationResponse UpsertAppointment(PatientsAppointmentInfo appointmentInfo)
        {
            if (string.IsNullOrEmpty(appointmentInfo.PatientsName) || appointmentInfo.Mobile == 0)
            {
                return new PatientCreationResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Patient Name is empty, Please enter name of the Patient"
                };
            }

            if(appointmentInfo.MS_Reference_Id ==null || appointmentInfo.MS_Reference_Id == 0)
            {
                appointmentInfo.MS_Reference_Id = 0;
            }

            int id = mPatientsInfoAccessT.UpsertPatientAppointment(appointmentInfo);
            if (id == 0)
            {
                return new PatientCreationResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Data not saved, please enter all the mandatory fields"
                };
            }
            else
            {
                return new PatientCreationResponse
                {
                    IsSuccess = true,
                    ErrorMessage = "Appointment created successfully"
                };
            }
        }

        public static List<PatientsAppointmentInfo> GetPatientAppointments(int CompanyId, DateTime fromDt, DateTime toDt)
        {
            return mPatientsInfoAccessT.GetPatientAppointments(CompanyId, fromDt, toDt);
        }

        public static PatientCreationResponse UpsertPatientsInfo(Library.Models.PatientsInfo patientsInfo, bool isCreateNewRecord)
        {
            PatientCreationResponse response = new PatientCreationResponse();

            // check patient record is already exists
            if (!isCreateNewRecord && patientsInfo.Id == 0)
            {
                bool isPatientAlreadyExistsWithNameOrMobile = mPatientsInfoAccessT.HasPatienAlreadyExists(patientsInfo);
                if (isPatientAlreadyExistsWithNameOrMobile)
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "A patient matching this name or mobile number already exists. Would you like to continue creating a new record?";

                    return response;
                }
            }

            int id = mPatientsInfoAccessT.UpsertPatientsInfo(patientsInfo);
            if (id > 0)
            {
                response.IsSuccess = true;
                response.Result = id;
            }
            else if (id <= 0)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Data not saved successfully";
            }
            return response;
        }

        public static int UpsertPatientSummary(PatientsSummaryInfo patientsSummary)
        {
            return mPatientsInfoAccessT.UpsertPatientSummary(patientsSummary);
        }

        public static List<PatientsSummaryInfo> GetPatientsSummaries(int CompanyId, DateTime fromDt, DateTime toDt)
        {
            return mPatientsInfoAccessT.GetPatientsSummaries(CompanyId, fromDt, toDt);
        }

        private static int GetOtherReferenceId(int compId, int userSelectedReferenceId, string otherReferenceName)
        {
            int referenceId = 0;
            List<ReferencesInfo> references = mPatientsInfoAccessT.GetReferences(compId);
            if (references != null)
            {
                ReferencesInfo otherReferenceInfo = references.FirstOrDefault(x => x.Reference.Contains("Other"));
                if (userSelectedReferenceId == otherReferenceInfo.Id)
                {
                    ReferencesInfo referencesInfo = new ReferencesInfo
                    {
                        MS_Comp_Id = compId,
                        Reference = otherReferenceName
                    };

                    referenceId = mMasterssInfoAccessT.UpsertReference(referencesInfo);
                }
            }
            return referenceId;
        }

        public static List<Library.Models.PatientsInfo> GetPatientInfo(int CompanyId, DateTime fromDt, DateTime toDt)
        {
            return mPatientsInfoAccessT.GetPatientInfo(CompanyId, fromDt, toDt);
        }

        public static List<PatientsSummaryInfo> GetPatientsFollowUpDateByDate(int compId, DateTime date)
        {
            return mPatientsInfoAccessT.GetPatientsFollowUpDateByDate(compId, date);
        }






        public static List<TreatmentsInfo> GetTreatments(int CompanyId)
        {
            return mPatientsInfoAccessT.GetTreatments(CompanyId);
        }

        public static List<ReferencesInfo> GetReferences(int CompanyId)
        {
            return mPatientsInfoAccessT.GetReferences(CompanyId);
        }

        public static List<StateInfo> GetStates()
        {
            return mPatientsInfoAccessT.GetStates();
        }

        public static List<CityInfo> GetCitiesByStateId(int StateId)
        {
            return mPatientsInfoAccessT.GetCitiesByStateId(StateId);
        }

        public static List<Library.Models.PatientsInfo> GetTodaysPatientsInfo(int CompanyId)
        {
            DateTime todaysDate = DateTime.Now;
            return mPatientsInfoAccessT.GetPatientsInfoByDate(CompanyId, todaysDate);
        }

        public static List<Models.PatientsInfo> GetExistingPatientsInfoByValue(int CompanyId, string value)
        {
            return mPatientsInfoAccessT.GetExistingPatientsInfoByValue(CompanyId, value);
        }

        public static void CreatePatientHistory(int compId, int patientId, int patientSummaryId)
        {
            mPatientsInfoAccessT.CreatePatientHistory(compId, patientId, patientSummaryId);
        }

        public static List<PatientHistory> GetPatientHistoriesById(int compId, int patientId)
        {
            return mPatientsInfoAccessT.GetPatientHistoriesById(compId, patientId);
        }


    }
}