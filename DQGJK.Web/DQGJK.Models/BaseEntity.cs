using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utils;

namespace DQGJK.Models
{
    public class BaseEntity
    {
        public BaseEntity()
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
    }
}
