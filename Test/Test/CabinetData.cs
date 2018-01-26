using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test
{
    [Table("CabinetData")]
    public class CabinetData
    {
        [Key, Column(Order = 1), Required, MaxLength(0x40)]
        public string ID { get; set; }

        [Display(Name = "设备编号"), MaxLength(0x40), Required]
        public string Code { get; set; }

        public DateTime CreateTime { get; set; }

        public decimal Humidity { get; set; }

        public decimal Temperature { get; set; }

        public int RelayOne { get; set; }

        public int RelayTwo { get; set; }

        public int HumidityAlarm { get; set; }

        public int TemperatureAlarm { get; set; }

        public int Dehumidify { get; set; }
    }
}
