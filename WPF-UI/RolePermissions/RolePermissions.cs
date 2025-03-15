using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_UI.RolePermissions
{
   public class RolePermissions
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public List<string> AllowedStages { get; set; }
    }
}
