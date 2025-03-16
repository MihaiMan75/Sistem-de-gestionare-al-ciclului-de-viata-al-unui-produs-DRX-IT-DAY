using BusinessLogic.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int id);
        Task<int> CreateUserAsync(UserDto user);
        Task<bool> UpdateUserAsync(UserDto user);
        Task<bool> DeleteUserAsync(int id);
        Task<UserDto> GetUserByUserNameAsync(string userName);
        Task<bool> HasUserRelationsAsync(int userId);
    }
}
