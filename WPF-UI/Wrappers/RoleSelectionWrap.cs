using BusinessLogic.DtoModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_UI.Wrappers
{
    public partial class RoleSelectionWrap : ObservableObject
    {
        public RoleDto Role { get; }

        [ObservableProperty]
        private bool isSelected;

        public RoleSelectionWrap(RoleDto role)
        {
            Role = role;
        }
    }
}
