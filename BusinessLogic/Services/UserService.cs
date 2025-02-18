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
    public class UserService : IUserService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserRoles> _userRoleReposiotory;

        public UserService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            _userRepository = repositoryFactory.CreateUserRepository();
            _userRoleReposiotory = repositoryFactory.CreateUserRolesRepository();

        }

        public async Task<int> CreateUserAsync(UserDto userDto)
        {
            await Validate(userDto);
            //link the user to the roles
            foreach (RoleDto roleDto in userDto.Roles)
            {
                UserRoles userRoles = new UserRoles();
                userRoles.id_user = userDto.Id;
                userRoles.role_id = roleDto.Id;
                await _userRoleReposiotory.AddAsync(userRoles);
            }
            var user = UserMaper.FromDto(userDto);
            return await _userRepository.AddAsync(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            IEnumerable<UserDto> result = new List<UserDto>();
            foreach (User user in users)
            {
                var roles = await _userRoleReposiotory.GetAllRolesByUserId(user.id_user);
                result.Append(UserMaper.ToDto(user, roles));
            }
            return result;
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            var roles = await _userRoleReposiotory.GetAllRolesByUserId(id);
            return UserMaper.ToDto(user, roles);
        }

        public async Task<bool> UpdateUserAsync(UserDto userDto)
        {
            await Validate(userDto);
            UserRoles userRoles = new UserRoles();
            userRoles.id_user = userDto.Id;

            //update aslo user roles
            foreach (RoleDto roleDto in userDto.Roles)
            {
               
                userRoles.role_id = roleDto.Id;
                await _userRoleReposiotory.UpdateAsync(userRoles);
            }
            var user = UserMaper.FromDto(userDto);
            return await _userRepository.UpdateAsync(user);
        }

        private async Task Validate(UserDto user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (String.IsNullOrEmpty(user.Name) || String.IsNullOrEmpty(user.Email) || String.IsNullOrEmpty(user.PhoneNumber))
                throw new Exception("User name, email and phone number are required");
            
            if (user.Roles.Count == 0)
                throw new Exception("User must have at least one role");

            bool result = int.TryParse(user.PhoneNumber,out int i);
            if (!result)
                throw new Exception("Phone number must be a number");
        }
    }
}
