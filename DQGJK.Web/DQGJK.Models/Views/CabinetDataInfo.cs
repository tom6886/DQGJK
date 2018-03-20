using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DQGJK.Models
{
    [Table("CabinetDataInfo")]
    public class CabinetDataInfo
    {
        public string ID { get; set; }

        public DateTime CreateTime { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }

        public string StationName { get; set; }

        public string ClientCode { get; set; }

        public string DeviceCode { get; set; }

        public decimal AverageHumidity { get; set; }

        public decimal MaxHumidity { get; set; }

        public decimal MinHumidity { get; set; }

        public decimal AverageTemperature { get; set; }

        public decimal MaxTemperature { get; set; }

        public decimal MinTemperature { get; set; }

        public int HumidityAlarm { get; set; }

        public int TemperatureAlarm { get; set; }

        public string StationID { get; set; }

        public string DeptID { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string CityCode { get; set; }
    }
}
