using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DQGJK.Models
{
    [Table("LogInfo")]
    public class LogInfo
    {
        public string ID { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string ClientName { get; set; }

        public string ClientCode { get; set; }

        public string DeviceCode { get; set; }

        public ExceptionType Type { get; set; }

        public string DeptID { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string CityCode { get; set; }
    }
}
