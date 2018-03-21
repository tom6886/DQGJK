using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utils;

namespace DQGJK.Models
{
    [Table("Operate")]
    public class Operate
    {
        public Operate()
        {
            this.ID = StringUtil.UniqueID();
            this.CreateTime = DateTime.Now;
            State = OperateState.Before;
        }

        [Key, Column(Order = 1), Required, MaxLength(0x40)]
        public string ID { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime SendTime { get; set; }

        [Display(Name = "所属环网柜CODE"), MaxLength(15), Required]
        public string ClientCode { get; set; }

        [Display(Name = "操作功能码"), MaxLength(2), Required]
        public string FunctionCode { get; set; }

        [Display(Name = "操作内容"), MaxLength(500)]
        public string Content { get; set; }

        public OperateState State { get; set; }
    }
}
