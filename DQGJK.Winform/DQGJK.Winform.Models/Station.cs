using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XUtils;

namespace DQGJK.Winform.Models
{
    [Table("Station")]
    public class Station
    {
        public Station()
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

        [Display(Name = "环网柜名称"), MaxLength(0x40), Required]
        public string Name { get; set; }

        [Display(Name = "环网柜编号"), MaxLength(100)]
        public string Code { get; set; }

        [Display(Name = "所属单位ID"), MaxLength(100), Required]
        public string DeptID { get; set; }

        [Display(Name = "省"), MaxLength(0x40), Required]
        public string Province { get; set; }

        [Display(Name = "市"), MaxLength(0x40), Required]
        public string City { get; set; }

        [Display(Name = "县"), MaxLength(0x40), Required]
        public string Country { get; set; }

        [Display(Name = "地区编号"), MaxLength(6), Required]
        public string CityCode { get; set; }

        [Display(Name = "环网柜地址"), MaxLength(200), Required]
        public string Address { get; set; }

        [Display(Name = "经度")]
        public string Lng { get; set; }

        [Display(Name = "纬度")]
        public string Lat { get; set; }

        public Status Status { get; set; }
    }
}
