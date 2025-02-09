using BusinessLogic.DtoModels;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Mappers
{
    static class RoleMapper
    {
        public static RoleDto ToDto(Role role)
        {
            return new RoleDto
            {
                Id = role.id,
                RoleName = role.roleName
            };
        }
        public static List<RoleDto> ToDto(List<Role> roles)
        {
            List<RoleDto> dtos = new List<RoleDto>();
            foreach (Role role in roles)
            {
                dtos.Add(ToDto(role));
            }
            return dtos;
        
        }

        public static Role FromDto(RoleDto dto)
        {
            return new Role
            {
                id = dto.Id,
                roleName = dto.RoleName
            };
        }

        public static List<Role> FromDto(List<RoleDto> dtos)
        {
            List<Role> roles = new List<Role>();
            foreach (RoleDto dto in dtos)
            {
                roles.Add(FromDto(dto));
            }
            return roles;
        }
    }
}
