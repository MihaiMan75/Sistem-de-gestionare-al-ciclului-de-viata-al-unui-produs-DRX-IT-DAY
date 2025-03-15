using BusinessLogic.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_UI.RolePermissions;

namespace WPF_UI.Services
{
    public class PermissionService
    {
        //use also the id
        public static bool HasPermission(UserDto user, string stageName)
        {   

            //if (user.Roles.Any(role => role.RoleName == "Admin")) return true; //admin
            if (user.Roles.Any(role => role.Id == 1)) return true; //admin

            foreach (var role in user.Roles)
            {
                ///var permissions = RolePermissionConfig.Permissions.FirstOrDefault(r => r.Role == role.RoleName);
                //id 
               var permissions = RolePermissionConfig.Permissions.FirstOrDefault(r => r.Id == role.Id);
                if (permissions != null && permissions.AllowedStages.Contains(stageName))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
