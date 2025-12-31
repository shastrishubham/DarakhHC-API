using System;

namespace DarakhsHC_API.Library.Models
{
    public class ItemSerialNosInfo
    {
        public int Id { get; set; }
        public DateTime FormDate { get; set; }
        public int MS_Comp_Id { get; set; }
        public int MS_Item_Id { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string Description { get; set; }
    }
}