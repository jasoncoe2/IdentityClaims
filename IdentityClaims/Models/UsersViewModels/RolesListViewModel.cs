using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityClaims.Models.UsersViewModels
{
    public class RolesListViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        public List<RoleViewModel> RolesList { get; set; }

        public RolesListViewModel()
        {
            RolesList = new List<RoleViewModel>();
        }
    }
}
