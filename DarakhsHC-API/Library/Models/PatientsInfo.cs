using System;

namespace DarakhsHC_API.Library.Models
{
    public class PatientsInfo
    {
        public int Id { get; set; }
        public DateTime FormDate { get; set; }
        public int MS_Comp_Id { get; set; }
        public int Patients_Appointment_Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string FullName { get; set; }
        public DateTime DOB { get; set; }
        public decimal Mobile1 { get; set; }
        public decimal Mobile2 { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int MS_City_Id { get; set; }
        public string CityName { get; set; }
        public int MS_State_Id { get; set; }
        public string StateName { get; set; }
        public double PostalCode { get; set; }
        public string MaritialStatus { get; set; }
        public int MS_Reference_Id { get; set; }
        public string Reference { get; set; }
        public int MS_Treament_Id { get; set; }
        public string TreamentName { get; set; }
        public int MS_User_Id { get; set; }
        public string UserFullName { get; set; }
        public string IsSummaryGenerated { get; set; }
        public string OtherReferenceName { get; set; }
        public string Remark { get; set; }
        public DateTime VisitDate { get; set; }
    }
}