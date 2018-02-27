using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DQGJK.Models
{
    [Table("StationInfo")]
    public class StationInfo
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string DeptID { get; set; }

        public string DeptName { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string CityCode { get; set; }

        public string Address { get; set; }

        public string Lng { get; set; }

        public string Lat { get; set; }

        public DateTime ModifyTime { get; set; }

        public Status Status { get; set; }
    }
}
