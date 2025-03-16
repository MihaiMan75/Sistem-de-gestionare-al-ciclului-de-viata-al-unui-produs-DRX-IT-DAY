using System;
using System.Collections.Generic;
using static BusinessLogic.Enums;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_UI.RolePermissions
{
    public static class RolePermissionConfig
    {  /// <summary>
       /// The RolePermissionConfig class defines a static and hard coded configuration for role-based permissions within the application.
       /// </summary>
        public static readonly List<RolePermissions> Permissions = new List<RolePermissions>
            {
                new RolePermissions { Id = 1, Role = Roles.Creator_Concept, AllowedStages = new List<Stages> { Stages.Concept } },
                new RolePermissions { Id = 2, Role = Roles.Engineer, AllowedStages = new List<Stages> { Stages.Fezabilitate } },
                new RolePermissions { Id = 3, Role = Roles.Designer, AllowedStages = new List<Stages> { Stages.Proiectare } },
                new RolePermissions { Id = 4, Role = Roles.Production_Manager, AllowedStages = new List<Stages> { Stages.Productie, Stages.Retragere, Stages.StandBy } },
                new RolePermissions { Id = 5, Role = Roles.Portfolio_Manager, AllowedStages = new List<Stages> { Stages.Retragere, Stages.Cancel, Stages.StandBy, Stages.Productie } },
                new RolePermissions { Id = 6, Role = Roles.Admin, AllowedStages = new List<Stages> { Stages.Concept, Stages.Fezabilitate, Stages.Proiectare, Stages.Productie, Stages.Retragere, Stages.StandBy } }
            };
    }
}
