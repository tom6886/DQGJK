using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DQGJK.Winform
{
    [Table("Operate")]
    public class Operate
    {
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

        public int RetryCount { get; set; }

        public OperateState State { get; set; }
    }

    public enum OperateState
    {
        Error = -1,
        Before = 0,
        Sended = 1,
        Done = 2
    }
}
