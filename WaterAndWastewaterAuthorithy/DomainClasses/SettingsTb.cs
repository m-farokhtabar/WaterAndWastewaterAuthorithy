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
    public class SettingsTb
    {
        [Required]
        public int Id { set; get; }

        [Required]
        public int CurrentYear { set; get; }
        
        [Required]
        public int Vat { set; get; }

        [Required]
        public int Subscription { set; get; }

        [Required]
        [MaxLength(50)]
        [Column(TypeName = "NChar")]
        public string CompanyName { set; get; }
        
        [Required]
        [MaxLength(250)]
        [Column(TypeName = "NChar")]
        public string BillMessage { set; get; }
        
        [Required]
        [MaxLength(50)]
        [Column(TypeName = "NChar")]
        public string WaterAndWastewaterAuthorityName { set; get; }
        
        [Required]
        [MaxLength(250)]
        [Column(TypeName = "NChar")]
        public string Address { set; get; }
        
        [Required]
        [MaxLength(11)]
        [Column(TypeName = "Char")]
        public string Tel { set; get; }
        [Timestamp]
        public byte[] RecordDate { set; get; }
    }
}
