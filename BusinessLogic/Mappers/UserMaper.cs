using BusinessLogic.DtoModels;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Mappers
{
    static class UserMaper
    {
        public static UserDto ToDto(User user, List<Role> roles)
        {
            return new UserDto
            {
                Id = user.id_user,
                Name = user.name,
                Email = user.email,
                PhoneNumber = user.phone_number,
                PasswordHashed = user.PasswordHashed,
                Roles = RoleMapper.ToDto(roles)
            };
        }

        public static User FromDto(UserDto dto)
        {
            return new User
            {
                id_user = dto.Id,
                name = dto.Name,
                email = dto.Email,
                PasswordHashed = dto.PasswordHashed,
                phone_number = dto.PhoneNumber.ToString(),
            };
        }

    }
}
