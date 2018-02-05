using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DQGJK.Models
{
    [Table("Station")]
    public class Station : BaseEntity
    {
        [Display(Name = "站点名称"), MaxLength(0x40), Required]
        public string Name { get; set; }

        [Display(Name = "站点编号"), MaxLength(100)]
        public string Code { get; set; }

        [Display(Name = "站点地址"), MaxLength(200), Required]
        public string Address { get; set; }

        [Display(Name = "所属单位ID"), MaxLength(100), Required]
        public string DeptID { get; set; }

        public Status Status { get; set; }
    }
}
