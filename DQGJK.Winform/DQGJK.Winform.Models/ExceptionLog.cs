﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XUtils;

namespace DQGJK.Winform.Models
{
    [Table("ExceptionLog")]
    public class ExceptionLog
    {
        public ExceptionLog()
        {
            this.ID = StringUtil.UniqueID();
            this.CreateTime = DateTime.Now;
        }

        public DateTime CreateTime { get; set; }

        [Key, Column(Order = 1), Required, MaxLength(0x40)]
        public string ID { get; set; }

        [Display(Name = "环网柜编号"), MaxLength(100)]
        public string ClientCode { get; set; }

        [Display(Name = "从机柜编号"), MaxLength(100)]
        public string DeviceCode { get; set; }

        public ExceptionType type { get; set; }
    }

    public enum ExceptionType
    {
        offline = 0,
        humidity = 1,
        temperature = 2
    }
}
