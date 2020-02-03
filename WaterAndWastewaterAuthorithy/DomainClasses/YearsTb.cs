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
    public class YearsTb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int  Year { set; get; }
        [Required]
        public bool IsClosed    { set; get; }
        [Timestamp]
        public byte[] RecordDate { set; get; }
    }
}
