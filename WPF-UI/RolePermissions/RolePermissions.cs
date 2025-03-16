using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessLogic.Enums;

namespace WPF_UI.RolePermissions
{
   public class RolePermissions
    {
        public int Id { get; set; }
        public Roles Role { get; set; }
        public List<Stages> AllowedStages { get; set; }
    }
}
