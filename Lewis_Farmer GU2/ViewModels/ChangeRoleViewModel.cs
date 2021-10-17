using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lewis_Farmer_GU2.ViewModels
{
    public class ChangeRoleViewModel
    {
        public string UserName { get; set; }

        public string OldRole { get; set; }

        [Required, Display(Name = "Role")]
        public string Role { get; set; }

        public ICollection<SelectListItem> Roles { get; set; }
    }
}