using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace WaterAndWastewaterAuthorithy.DomainClasses
{
    public class BillPeriodsTb
    {
        [Key]
        public int Id { set; get; }                      
        public int Year { set; get; }

        [Required]
        public bool IsClosed { set; get; }

        [Required]
        public bool IsSelected { set; get; }
        
        [MaxLength(40)]
        [Column(TypeName = "NChar")]
        public string Name { set; get; }

        [Required]
        [MaxLength(10)]
        [Column(TypeName = "Char")]
        public string DateFrom { set; get; }

        [Required]
        [MaxLength(10)]
        [Column(TypeName = "Char")]
        public string DateTo { set; get; }
        
        [Required]
        public int MonthPeriod { set; get; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(500)]
        [Column(TypeName = "NVarChar")]
        public string Description { set; get; }

        [Timestamp]
        public byte[] RecordDate { set; get; }
    }
}
