using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarakhsHC_API.Library.Models
{
    public class PatientInventoryInfo
    {
        public int Id { get; set; }
        public int MS_Comp_Id { get; set; }
        public int Patients_Summary_Id { get; set; }
        public int MS_Item_Id { get; set; }
        public string ItemName { get; set; }
        public int MS_Item_SerialNos_Id { get; set; }
        public string SerialNumber { get; set; }
        public double ItemSalePrice { get; set; }
    }
}