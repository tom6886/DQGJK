using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DQGJK.Models
{
    [Table("CabinetInfo")]
    public class CabinetInfo
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string DeptID { get; set; }

        public string DeptName { get; set; }

        public string StationCode { get; set; }

        public string StationName { get; set; }

        public DateTime ModifyTime { get; set; }

        public Status Status { get; set; }

        public int Sort { get; set; }
    }
}
