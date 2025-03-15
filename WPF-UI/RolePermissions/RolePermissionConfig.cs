using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_UI.RolePermissions
{
    public static class RolePermissionConfig
    {
        public static readonly List<RolePermissions> Permissions = new List<RolePermissions>
    {
        new RolePermissions { Id=1, Role = "Concept Creator", AllowedStages = new List<string> { "Concept", "Fezabilitate" } },
        new RolePermissions { Id=2, Role = "Engineer", AllowedStages = new List<string> { "Fezabilitate", "Proiectare" } },
        new RolePermissions { Id=3, Role = "Designer", AllowedStages = new List<string> { "Proiectare", "Productie" } },
        new RolePermissions { Id=4, Role = "Production Manager", AllowedStages = new List<string> { "Productie", "Retragere", "Stand-by" } },
        new RolePermissions { Id=5, Role = "Portfolio Manager", AllowedStages = new List<string> { "Retragere", "Cancel", "Stand-by", "Productie" } },
        new RolePermissions { Id=6, Role = "Admin", AllowedStages = new List<string> { "Concept", "Fezabilitate", "Proiectare", "Productie", "Retragere", "Stand-by", "Cancel" } }
    };
    }
}
