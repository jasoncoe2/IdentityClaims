using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityClaims.Models.UsersViewModels
{
    public class ClaimDefinitionViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Name { get; set; }

        public bool Checked { get; set; }
    }
}
