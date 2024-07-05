using Microsoft.AspNetCore.Mvc;
using Payroll.API.Models;
using Payroll.API.Services.User;
using Payroll.API.Services.User.Dto;

namespace Payroll.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpGet]
        public async Task<IActionResult> GetUserById(int id, CancellationToken ct)
        {
            var userList = await _userService.GetUserById(id, ct);
            return Ok(userList);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserList(string? searchQuery, int pageNumber, int pageSize, CancellationToken ct)
        {
            var userList = await _userService.GetUserList(searchQuery, pageNumber, pageSize,ct);
            return Ok(userList);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserListAdmin(string? searchQuery, int pageNumber, int pageSize, int userId, CancellationToken ct)
        {
            var userList = await _userService.GetUserListAdmin(searchQuery, pageNumber, pageSize, userId,ct);
            return Ok(userList);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserType(CancellationToken ct)
        {
            var userList = await _userService.GetUserType(ct);
            return Ok(userList);
        }

        [HttpGet]
        public async Task<IActionResult> GetSchool(CancellationToken ct)
        {
            var userList = await _userService.GetSchool(ct);
            return Ok(userList);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserDTO user, CancellationToken ct)
        {
            var response = await _userService.AddUser(user, ct);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAdmin(int userId, AddUserDTO user, CancellationToken ct)
        {
            var response = await _userService.AddUserAdmin(user, userId, ct);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> EditUser(EditUserDTO user, CancellationToken ct)
        {
            var response = await _userService.EditUser(user, ct);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> TempPasswordUpdater(string username,string password,CancellationToken ct)
        {
            // temporary controller method to update encrypted password
            var response = await _userService.PasswordUpdater(username, password, ct);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UnlockAccount(int userId, CancellationToken ct)
        {
            var response = await _userService.UnlockAccount(userId, ct);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UnlockStudentAccount(int studentId, CancellationToken ct)
        {
            var response = await _userService.UnlockStudentAccount(studentId, ct);
            return Ok(response);
        }
    }
}
