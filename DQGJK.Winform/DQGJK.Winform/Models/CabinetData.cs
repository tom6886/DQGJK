using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XUtils;

namespace DQGJK.Winform
{
    [Table("CabinetData")]
    public class CabinetData
    {
        public CabinetData(DateTime date)
        {
            ID = StringUtil.UniqueID();
            CreateTime = date;
            Year = date.Year;
            Month = date.Month;
            Day = date.Day;
        }

        [Key, Column(Order = 1), Required, MaxLength(0x40)]
        public string ID { get; set; }

        public DateTime CreateTime { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }

        [Display(Name = "环网柜编号"), MaxLength(100)]
        public string ClientCode { get; set; }

        [Display(Name = "从机柜编号"), MaxLength(100)]
        public string DeviceCode { get; set; }

        public decimal AverageHumidity { get; set; }

        public decimal MaxHumidity { get; set; }

        public decimal MinHumidity { get; set; }

        public decimal AverageTemperature { get; set; }

        public decimal MaxTemperature { get; set; }

        public decimal MinTemperature { get; set; }

        public int HumidityAlarm { get; set; }

        public int TemperatureAlarm { get; set; }
    }
}
