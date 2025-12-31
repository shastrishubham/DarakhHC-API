using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarakhsHC_API.Library.Models
{
    public class ItemCompanyInfo
    {
        public int Id { get; set; }
        public int MS_Comp_Id { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
    }
}