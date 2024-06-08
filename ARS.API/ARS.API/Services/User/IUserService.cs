using ARS.API.Models;
using ARS.API.Services.User.Dto;
using Payroll.API.Services.User.Dto;
using Payroll.API.SharedDto;

namespace Payroll.API.Services.User
{
    public interface IUserService
    {
        Task<CallResultDto<List<UserDto>>> GetUserList(string? searchQuery, int pageNumber, int pageSize, CancellationToken ct);
        Task<CallResultDto<object>> AddUser(AddUserDTO user, CancellationToken ct);
        Task<CallResultDto<object>> AddUserAdmin(AddUserDTO user, int userId, CancellationToken ct);
        Task<CallResultDto<UserDto>> GetUserById (int id, CancellationToken ct);
        Task<CallResultDto<object>> EditUser(EditUserDTO user, CancellationToken ct);
        Task<CallResultDto<List<UserDto>>> GetUserListAdmin(string? searchQuery, int pageNumber, int pageSize, int userId, CancellationToken ct);

        //Temporary Password Updated Method
        Task<CallResultDto<object>> PasswordUpdater(string username, string password, CancellationToken ct);
        Task<CallResultDto<List<UserType>>> GetUserType(CancellationToken ct);
        Task<CallResultDto<List<SchoolDto>>> GetSchool(CancellationToken ct);
    }
}
