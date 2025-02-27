using BusinessLogic.Interfaces;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.DtoModels;
using BusinessLogic.Mappers;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class RoleService: IRoleService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IRepository<Role> _roleRepository;

        public RoleService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            _roleRepository = repositoryFactory.CreateRoleRepository();
        }

        public async Task<int> CreateRoleAsync(RoleDto roleDto)
        {     
            await Validate(roleDto);

            var role = RoleMapper.FromDto(roleDto);
            return await _roleRepository.AddAsync(role);
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            return await _roleRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllAsync();
            return RoleMapper.ToDto(roles.ToList());
        }

        public async Task<RoleDto> GetRoleByIdAsync(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
            {
                return null;
            }
            return RoleMapper.ToDto(role);
        }

        public async Task<bool> UpdateRoleAsync(RoleDto roleDto)
        {
            await Validate(roleDto);

            var role = RoleMapper.FromDto(roleDto);
            return await _roleRepository.UpdateAsync(role);
        }

        private async Task Validate(RoleDto role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            if (string.IsNullOrWhiteSpace(role.RoleName))
                throw new ArgumentException("Role Name cannot be empty");
        }
    }
}
