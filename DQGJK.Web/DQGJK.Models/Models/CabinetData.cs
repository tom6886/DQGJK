using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DQGJK.Models
{
    [Table("CabinetData")]
    public class CabinetData
    {
        [Key, Column(Order = 1), Required, MaxLength(0x40)]
        public string ID { get; set; }

        public DateTime CreateTime { get; set; }

        [Display(Name = "电气柜编号"), MaxLength(100)]
        public string Code { get; set; }

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
