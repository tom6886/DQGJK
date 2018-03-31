using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XUtils;

namespace DQGJK.Winform.Models
{
    [Table("Cabinet")]
    public class Cabinet
    {
        public Cabinet()
        {
            this.ID = StringUtil.UniqueID();
            this.CreateTime = DateTime.Now;
            this.ModifyTime = DateTime.Now;
        }

        [MaxLength(50)]
        public string Creator { get; set; }

        public DateTime CreateTime { get; set; }

        [MaxLength(0x40)]
        public string CreatorID { get; set; }

        [Key, Column(Order = 1), Required, MaxLength(0x40)]
        public string ID { get; set; }

        public DateTime ModifyTime { get; set; }

        [MaxLength(1000)]
        public string Remark { get; set; }

        [Display(Name = "电气柜名称"), MaxLength(0x40), Required]
        public string Name { get; set; }

        [Display(Name = "电气柜编号"), MaxLength(100)]
        public string Code { get; set; }

        [Display(Name = "所属环网柜CODE"), MaxLength(100), Required]
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

        public int Sort { get; set; }
    }

    public enum Status
    {
        enable = 1,
        disable = 0
    }
}
