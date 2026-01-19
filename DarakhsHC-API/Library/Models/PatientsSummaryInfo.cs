using System;

namespace DarakhsHC_API.Library.Models
{
    public class PatientsSummaryInfo
    {
        public int Id { get; set; }
        public int MS_Comp_Id { get; set; }
        public DateTime FormDate { get; set; }
        public string ReceiptNo { get; set; }
        public int MS_Patients_Id { get; set; }
        public string PatientsName { get; set; }
        public string Mobile { get; set; }
        public int MS_Treament_Id { get; set; }
        public string TreamentName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public decimal PostalCode { get; set; }
        public int MS_State_Id { get; set; }
        public string StateName { get; set; }
        public int MS_City_Id { get; set; }
        public string CityName { get; set; }
        public DateTime VisitDate { get; set; }
        public string Remark { get; set; }
        public string Notes { get; set; }
        public DateTime? NextVisitDate { get; set; }
        public bool IsFollowUpReq { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public char? Gender { get; set; }
        public decimal Amount { get; set; }

        public int MS_Reference_Id { get; set; }
        public string Reference { get; set; }
    }
}