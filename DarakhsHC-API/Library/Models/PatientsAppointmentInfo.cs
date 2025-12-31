using System;

namespace DarakhsHC_API.Library.Models
{
    public class PatientsAppointmentInfo
    {
        public int Id { get; set; }
        public DateTime FormDate { get; set; }
        public int MS_Comp_Id { get; set; }
        public string PatientsName { get; set; }
        public string EnquiryFor { get; set; }
        public decimal Mobile { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Address { get; set; }
        public int MS_Reference_Id { get; set; }
        public string OtherReferenceName { get; set; }
        public string ReferenceName { get; set; }
    }
}