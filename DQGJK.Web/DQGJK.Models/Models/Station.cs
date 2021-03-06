﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DQGJK.Models
{
    [Table("Station")]
    public class Station : BaseEntity
    {
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
