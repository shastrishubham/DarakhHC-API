using System;

namespace DarakhsHC_API.Library.Models
{
    public class PatientEnquiry
    {
        public int Id { get; set; }
        public DateTime FormDate { get; set; }
        public int MS_Comp_Id { get; set; }
        public string Name { get; set; }
        public string EnquiryFor { get; set; }
        public decimal Mobile { get; set; }
        public string Address { get; set; }
        public int MS_Country_Id { get; set; }
        public string CountryName { get; set; }
        public int MS_State_Id { get; set; }
        public string StateName { get; set; }
        public int MS_City_Id { get; set; }
        public string CityName { get; set; }
    }
}