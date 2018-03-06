using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DQGJK.Models
{
    [Table("Cabinet")]
    public class Cabinet : BaseEntity
    {
        [Display(Name = "电气柜名称"), MaxLength(0x40), Required]
        public string Name { get; set; }

        [Display(Name = "电气柜编号"), MaxLength(100)]
        public string Code { get; set; }

        [Display(Name = "所属单位ID"), MaxLength(100), Required]
        public string StationID { get; set; }

        public decimal Humidity { get; set; }

        public decimal Temperature { get; set; }

        public decimal HumidityLimit { get; set; }

        public decimal TemperatureLimit { get; set; }

        public Status Status { get; set; }
    }
}