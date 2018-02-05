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

        public decimal Humidity { get; set; }

        public decimal Temperature { get; set; }

        public int RelayOne { get; set; }

        public int RelayTwo { get; set; }

        public int HumidityAlarm { get; set; }

        public int TemperatureAlarm { get; set; }

        public int Dehumidify { get; set; }
    }
}
