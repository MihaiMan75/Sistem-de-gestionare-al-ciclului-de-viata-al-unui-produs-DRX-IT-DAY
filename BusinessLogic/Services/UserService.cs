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
using ProductManagementBusinessLogic.AuthUtils;

namespace BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly UserRepository _userRepository;
        private readonly UserRolesRepository _userRoleReposiotory;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            _userRepository = repositoryFactory.CreateUserRepository();
            _userRoleReposiotory = repositoryFactory.CreateUserRolesRepository();
            _passwordHasher = new PasswordHasher();


        }

        public async Task<int> CreateUserAsync(UserDto userDto)
        {
            await Validate(userDto);
           
            var user = UserMaper.FromDto(userDto);
            user.PasswordHashed = _passwordHasher.HashPassword(userDto.PasswordHashed);
            userDto.Id = await _userRepository.AddAsync(user);
            //link the user to the roles
            foreach (RoleDto roleDto in userDto.Roles)
            {
                UserRoles userRoles = new UserRoles();
                userRoles.id_user = userDto.Id;
                userRoles.role_id = roleDto.Id;
                await _userRoleReposiotory.AddAsync(userRoles);
            }
            return userDto.Id;
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
                var roles = await _userRoleReposiotory.GetUserRolesAsync(user.id_user);
               result = result.Append(UserMaper.ToDto(user, roles.ToList()));
            }
            return result;
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            var roles = await _userRoleReposiotory.GetUserRolesAsync(id);
            if(user == null)
            {
                return null;
            }
            return UserMaper.ToDto(user, roles.ToList());
        }

       

        public async Task<bool> UpdateUserAsync(UserDto userDto)
        {
            await Validate(userDto);
            UserRoles userRoles = new UserRoles();
            userRoles.id_user = userDto.Id;
            //userDto.PasswordHashed = _passwordHasher.HashPassword(userDto.PasswordHashed); already hashed in the service

            //search for the roles that are not in the user roles and delete them
            var existingRolesIds = await _userRoleReposiotory.GetUserRolesAsync(userDto.Id);
            foreach (Role role in existingRolesIds)
            {
                if (userDto.Roles.Any(r => r.Id == role.id))
                {
                    continue;
                }
                await _userRoleReposiotory.DeleteAsync(userDto.Id, role.id);
            }
            //search for the roles that are in the user roles and add them
            var existingRoles = await _userRoleReposiotory.GetUserRolesAsync(userDto.Id);
            foreach (RoleDto roleDto in userDto.Roles)
            {
                if (existingRoles.Any(r => r.id== roleDto.Id))
                {
                    continue;
                }
                userRoles.role_id = roleDto.Id;
                await _userRoleReposiotory.AddAsync(userRoles);
            }
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

            if (String.IsNullOrEmpty(user.Name)|| String.IsNullOrEmpty(user.PasswordHashed)|| String.IsNullOrEmpty(user.Email) || String.IsNullOrEmpty(user.PhoneNumber))
                throw new Exception("User name,password, email and phone number are required");

            if (!user.PhoneNumber.All(char.IsDigit))
                throw new Exception("Phone number must contain only digits");

            if (user.Roles.Count == 0)
                throw new Exception("User must have at least one role");

        }

        public async Task<UserDto> GetUserByUserNameAsync(string userName)
        {
            try
            {
                var user = await _userRepository.GetUserByUserNameAsync(userName);
                if (user == null)
                {
                    return null;
                }
                var roles = await _userRoleReposiotory.GetUserRolesAsync(user.id_user);
                return UserMaper.ToDto(user, roles.ToList());
            }
            catch (Exception ex)
            {
                return null;
            }
           
          
        }
    }
}
