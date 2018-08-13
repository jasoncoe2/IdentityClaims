using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityClaims.Models.UsersViewModels
{
    public class UserRolesViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Roles List")]
        public List<RoleDefinitionViewModel> RolesList { get; set; }

        public UserRolesViewModel()
        {
            RolesList = new List<RoleDefinitionViewModel>();
        }
    }
}
