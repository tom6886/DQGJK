using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DQGJK.Models
{
    [Table("Department")]
    public class Department : BaseEntity
    {
        [Display(Name = "名称"), MaxLength(100), Required]
        public string Name { get; set; }

        [Display(Name = "编号"), MaxLength(100), Required]
        public string Code { get; set; }

        [Display(Name = "父级ID"), MaxLength(0x40)]
        public string ParentID { get; set; }

        public Status Status { get; set; }
    }
}
