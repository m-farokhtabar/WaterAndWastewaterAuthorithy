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
    public class CustomersTb
    {
        public int Id { set; get; }
        public int Year { set; get; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(10)]
        [Column(TypeName = "Char")]
        public string MeliCode { set; get; }

        [Required]
        [MaxLength(25)]
        [Column(TypeName = "NVarChar")]
        public string Name { set; get; }

        [Required]
        [MaxLength(35)]
        [Column(TypeName = "NVarChar")]
        public string Family { set; get; }

        [Required]
        [MaxLength(25)]
        [Column(TypeName = "NVarChar")]
        public string Father { set; get; }

        [Required]
        [MaxLength(340)]
        [Column(TypeName = "NVarChar")]
        public string Search { set; get; }


        [Required(AllowEmptyStrings = true)]
        [MaxLength(10)]
        [Column(TypeName = "NVarChar")]
        public string IdCard { set; get; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(30)]
        [Column(TypeName = "NVarChar")]
        public string CityCard { set; get; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(11)]
        [Column(TypeName = "VarChar")]
        public string Phone { set; get; }

        [Required]
        [MaxLength(11)]
        [Column(TypeName = "VarChar")]
        public string CellPhone { set; get; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(10)]
        [Column(TypeName = "Char")]
        public string PostalCode { set; get; }

        [Required]
        [MaxLength(250)]
        [Column(TypeName = "NVarChar")]
        public string Address { set; get; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(500)]
        [Column(TypeName = "NVarChar")]
        public string Description { set; get; }

        [Timestamp]
        public byte[] RecordDate { set; get; }

        public IList<SubscriptionsTb> Subscriptions { get; set; }
    }
}
