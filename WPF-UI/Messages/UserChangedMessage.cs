using BusinessLogic.DtoModels;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_UI.Messages
{
    public class UserChangedMessage
    {
        public UserDto? User { get; }

        public UserChangedMessage(UserDto user)
        {
            User = user;
        }
    }
}
