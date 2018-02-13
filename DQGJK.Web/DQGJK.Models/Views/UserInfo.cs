using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DQGJK.Models
{
    [Table("UserInfo")]
    public class UserInfo
    {
        public string ID { get; set; }

        public string Account { get; set; }

        public string DisplayName { get; set; }

        public string Tel { get; set; }

        public string DeptID { get; set; }

        public string DeptName { get; set; }

        public string Roles { get; set; }

        public string DwID { get; set; }

        public string DwName { get; set; }

        public DateTime ModifyTime { get; set; }

        public Status Status { get; set; }
    }
}
