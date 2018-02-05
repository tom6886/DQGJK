using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DQGJK.Models
{
    [Table("Guser")]
    public class Guser : BaseEntity
    {
        [Display(Name = "用户编号"), MaxLength(0x40), Required]
        public string Account { get; set; }

        [Display(Name = "口令"), MaxLength(0x40), Required]
        public string PassWord { get; set; }

        [Display(Name = "用户姓名"), MaxLength(100)]
        public string DisplayName { get; set; }

        public Status Status { get; set; }

        [Display(Name = "联系方式"), MaxLength(20)]
        public string Tel { get; set; }

        [Display(Name = "部门ID"), MaxLength(0x40)]
        public string DeptID { get; set; }

        [Display(Name = "所属角色"), MaxLength(200)]
        public string Roles { get; set; }
    }
}
