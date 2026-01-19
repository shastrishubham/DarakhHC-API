using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarakhsHC_API.Library.Models
{
    public class PatientHistory
    {
        public int Id { get; set; }
        public int MS_Comp_Id { get; set; }
        public DateTime FormDate { get; set; }
        public string ReceiptNo { get; set; }
        public int MS_Patients_Id { get; set; }
        public string PatientsName { get; set; }
        public decimal Mobile { get; set; }
        public string Address { get; set; }
        public double PostalCode { get; set; }
        public string Remark { get; set; }
        public DateTime VisitDate { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public DateTime NextVisitDate { get; set; }
        public decimal Amount { get; set; }
        public char? Gender { get; set; }
    }
}