using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityClaims.Models.UsersViewModels
{
    public class UserClaimsViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public List<ClaimDefinitionViewModel> ClaimsList { get; set; }

        public UserClaimsViewModel()
        {
            ClaimsList = new List<ClaimDefinitionViewModel>();
        }
    }
}
