using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DQGJK.Models
{
    [Table("Cabinet")]
    public class Cabinet : BaseEntity
    {
        [Display(Name = "电气柜名称"), MaxLength(0x40), Required]
        public string Name { get; set; }

        [Display(Name = "电气柜编号"), MaxLength(100)]
        public string Code { get; set; }

        [Display(Name = "所属环网柜CODE"), MaxLength(10), Required]
        public string StationCode { get; set; }

        public decimal Humidity { get; set; }

        public decimal Temperature { get; set; }

        public decimal HumidityLimit { get; set; }

        public decimal TemperatureLimit { get; set; }

        public int RelayOne { get; set; }

        public int RelayTwo { get; set; }

        public int HumidityAlarm { get; set; }

        public int TemperatureAlarm { get; set; }

        public int Dehumidify { get; set; }

        public Status Status { get; set; }
    }
}