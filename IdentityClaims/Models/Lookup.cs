using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityClaims.Models
{
    public class Lookup
    {
        [Key]
        public int LookupID { get; set; }

        [Required]
        [StringLength(48)]
        [Display(Name = "Lookup Code")]
        public string LookupCode { get; set; }

        [Required]
        [StringLength(48)]
        [Display(Name = "Lookup Desc")]
        public string LookupDesc { get; set; }

        [StringLength(128)]
        [Display(Name = "Attribute 1")]
        public string Attribute1 { get; set; }

        [StringLength(128)]
        [Display(Name = "Attribute 2")]
        public string Attribute2 { get; set; }

        [StringLength(128)]
        [Display(Name = "Attribute 3")]
        public string Attribute3 { get; set; }

        [StringLength(128)]
        [Display(Name = "Attribute 4")]
        public string Attribute4 { get; set; }

        [StringLength(128)]
        [Display(Name = "Attribute 5")]
        public string Attribute5 { get; set; }

        [Required]
        [StringLength(1)]
        [Display(Name = "Active")]
        public string ActiveFlag { get; set; }

        public ICollection<LookupValue> LookupValues { get; set; }

    }
}
