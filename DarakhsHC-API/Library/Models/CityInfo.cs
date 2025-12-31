using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarakhsHC_API.Library.Models
{
    public class CityInfo
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public string CityCode { get; set; }
        public int MS_State_Id { get; set; }
        public string StateName { get; set; }
        public int MS_Country_Id { get; set; }

    }
}