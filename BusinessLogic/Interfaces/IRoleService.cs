using BusinessLogic.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<RoleDto> GetRoleByIdAsync(int id);
        Task<int> CreateRoleAsync(RoleDto role);
        Task<bool> UpdateRoleAsync(RoleDto role);
        Task<bool> DeleteRoleAsync(int id);
    }
}
