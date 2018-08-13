using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityClaims.Models
{
    public class LookupValue
    {
        [Key]
        public int LookupValueID { get; set; }

        [Required]
        [Display(Name = "Lookup")]
        public int LookupID { get; set; }

        [Required]
        [StringLength(48)]
        [Display(Name = "Lookup Value Code")]
        public string LookupValueCode { get; set; }

        [Required]
        [StringLength(48)]
        [Display(Name = "Lookup Value Desc")]
        public string LookupValueDesc { get; set; }

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

        public Lookup Lookup { get; set; }

    }
}
