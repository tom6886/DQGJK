﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utils;

namespace DQGJK.Models
{
    [Table("DeviceOperate")]
    public class DeviceOperate
    {
        public DeviceOperate()
        {
            ID = StringUtil.UniqueID();
            CreateTime = DateTime.Now;
        }

        public DeviceOperate(string parentID, DeviceOperate operate)
        {
            ID = StringUtil.UniqueID();
            CreateTime = DateTime.Now;
            OperateID = parentID;
            DeviceCode = operate.DeviceCode;
            HumidityLimit = operate.HumidityLimit;
            TemperatureLimit = operate.TemperatureLimit;
            RelayOne = operate.RelayOne;
            RelayTwo = operate.RelayTwo;
            Dehumidify = operate.Dehumidify;
        }

        public void Update(DeviceOperate operate)
        {
            HumidityLimit = operate.HumidityLimit;
            TemperatureLimit = operate.TemperatureLimit;
            RelayOne = operate.RelayOne;
            RelayTwo = operate.RelayTwo;
            Dehumidify = operate.Dehumidify;
        }

        [Key, Column(Order = 1), Required, MaxLength(0x40)]
        public string ID { get; set; }

        public DateTime CreateTime { get; set; }

        [Display(Name = "对应操作表ID"), MaxLength(0x40), Required]
        public string OperateID { get; set; }

        [Display(Name = "操作设备CODE"), MaxLength(10), Required]
        public string DeviceCode { get; set; }

        public decimal HumidityLimit { get; set; }

        public decimal TemperatureLimit { get; set; }

        public int RelayOne { get; set; }

        public int RelayTwo { get; set; }

        public int Dehumidify { get; set; }
    }
}
