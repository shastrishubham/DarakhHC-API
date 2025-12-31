using DarakhsHC_API.Library.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarakhsHC_API.Library.Models
{
    public class ItemInfo
    {
        public int Id { get; set; }
        public DateTime FormDate { get; set; }
        public int MS_Comp_Id { get; set; }
        public int MS_ItemComp_Id { get; set; }
        public string CompanyName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OtherInfo { get; set; }
        public double MRP { get; set; }
        public double PRate { get; set; }
        public DateTime PRateEffectiveFrom { get; set; }
        public double SRate { get; set; }
        public int MS_TaxRate_Id { get; set; }
        public string TaxRateName { get; set; }
        public string SerialNo { get; set; }
        public ItemTypes ItemType { get; set; }
        public string HsnCode { get; set; }
        public double MinimumStock { get; set; }
        public int TotalStock { get; set; }
        public int AvailableStock { get; set; }
        public int MS_WH_Id { get; set; }
        public string WarehouseName { get; set; }
    }
}