using ARS.API.Models;
using ARS.API.Services.Email;
using ARS.API.Services.User.Dto;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Payroll.API.Context;
using Payroll.API.Services.User.Dto;
using Payroll.API.SharedDto;
using System.Data;
using System.Text;

namespace Payroll.API.Services.User
{
    public class UserService(ParyollContext payrollcontext, IDbConnection connection, IEmailService emailService) : IUserService
    {
        private readonly ParyollContext _payrollcontext = payrollcontext;
        private readonly IDbConnection _connection = connection;
        private readonly IEmailService _emailService = emailService;


        public async Task<CallResultDto<List<UserDto>>> GetUserList(string? searchQuery, int pageNumber, int pageSize, CancellationToken ct)
        {
            var callResult = new CallResultDto<List<UserDto>>();

            try
            {
                var baseSql = @"
            SELECT u.Id, u.UserName, u.FirstName, u.LastName, u.Email, u.Active, u.IsLockedOut, u.UserTypeId, ut.TypeName as UserTypeName, e.AssignedSchoolId, s.SchoolName
            FROM User u
            INNER JOIN Employee e on e.UserId = u.Id
            INNER JOIN UserType ut ON u.UserTypeId = ut.Id
            INNER JOIN School s ON e.AssignedSchoolId = s.Id
            WHERE u.Active = true";

                var countSql = @"
            SELECT COUNT(*)
            FROM User u
            INNER JOIN Employee e on e.UserId = u.Id
            INNER JOIN UserType ut ON u.UserTypeId = ut.Id
            INNER JOIN School s ON e.AssignedSchoolId = s.Id
            WHERE u.Active = true";

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    var searchCondition = @"
                AND (
                    u.UserName LIKE @SearchQuery OR
                    u.FirstName LIKE @SearchQuery OR
                    u.LastName LIKE @SearchQuery OR
                    u.Email LIKE @SearchQuery OR
                    s.SchoolName LIKE @SearchQuery OR
                    ut.TypeName LIKE @SearchQuery
                )";

                    baseSql += searchCondition;
                    countSql += searchCondition;
                }

                var paginationSql = "";
                if (pageSize > 0)
                {
                    paginationSql = @"
                ORDER BY u.Id
                LIMIT @PageSize OFFSET @Offset";
                }
                var finalSql = baseSql + paginationSql;

                var offset = (pageNumber - 1) * pageSize;
                var users = await _connection.QueryAsync<UserDto>(finalSql, new
                {
                    SearchQuery = $"%{searchQuery}%",
                    Offset = offset,
                    PageSize = pageSize
                });

                var totalCount = await _connection.ExecuteScalarAsync<int>(countSql, new
                {
                    SearchQuery = $"%{searchQuery}%"
                });

                callResult.IsSuccess = true;
                callResult.Data = users.ToList();
                callResult.TotalCount = totalCount;  // Add total count to the result
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Data = null;
                callResult.Message = "Fetching User List failed.";
            }

            return callResult;
        }


        public async Task<CallResultDto<List<UserType>>> GetUserType(CancellationToken ct)
        {
            var callResult = new CallResultDto<List<UserType>>();

            try
            {
                var sql = @"
                   SELECT * 
                    FROM UserType 
                    WHERE id <> 1;";

                var userTypes = await _connection.QueryAsync<UserType>(sql);

                callResult.IsSuccess = true;
                callResult.Data = userTypes.ToList();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Data = null;
                callResult.Message = "Fetching User Type List failed.";
            }

            return callResult;
        }

        public async Task<CallResultDto<List<SchoolDto>>> GetSchool(CancellationToken ct)
        {
            var callResult = new CallResultDto<List<SchoolDto>>();

            try
            {
                var sql = @"
                    SELECT Id, SchoolName from School WHERE IsActive = 1 ";

                var schools = await _connection.QueryAsync<SchoolDto>(sql);

                callResult.IsSuccess = true;
                callResult.Data = schools.ToList();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Data = null;
                callResult.Message = "Fetching School List failed.";
            }

            return callResult;
        }

        public async Task<CallResultDto<List<UserDto>>> GetUserListAdmin(string? searchQuery, int pageNumber, int pageSize, int userId, CancellationToken ct)
        {
            var callResult = new CallResultDto<List<UserDto>>();
            var user = await _payrollcontext.Employee.FirstOrDefaultAsync(x => x.UserId == userId);
            var schoolId = user.AssignedSchoolId;

            try
            {
                var baseSql = @"
            SELECT u.Id, u.UserName, u.FirstName, u.LastName, u.Email, u.Active, u.IsLockedOut, u.UserTypeId, ut.TypeName as UserTypeName, e.AssignedSchoolId, s.SchoolName
            FROM User u
            INNER JOIN Employee e on e.UserId = u.Id
            INNER JOIN UserType ut ON u.UserTypeId = ut.Id
            INNER JOIN School s ON e.AssignedSchoolId = s.Id
            WHERE u.Active = true AND e.assignedSchoolId = @SchoolId and u.UserTypeId > 2";

                var countSql = @"
            SELECT COUNT(*)
            FROM User u
            INNER JOIN Employee e on e.UserId = u.Id
            INNER JOIN UserType ut ON u.UserTypeId = ut.Id
            INNER JOIN School s ON e.AssignedSchoolId = s.Id
            WHERE u.Active = true AND e.assignedSchoolId = @SchoolId and  u.UserTypeId > 2";

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    var searchCondition = @"
                AND (
                    u.UserName LIKE @SearchQuery OR
                    u.FirstName LIKE @SearchQuery OR
                    u.LastName LIKE @SearchQuery OR
                    u.Email LIKE @SearchQuery OR
                    s.SchoolName LIKE @SearchQuery OR
                    ut.TypeName LIKE @SearchQuery
                )";

                    baseSql += searchCondition;
                    countSql += searchCondition;
                }

                var paginationSql = "";
                if (pageSize > 0)
                {
                    paginationSql = @"
                ORDER BY u.Id
                LIMIT @PageSize OFFSET @Offset";
                }
                var finalSql = baseSql + paginationSql;

                var offset = (pageNumber - 1) * pageSize;
                var users = await _connection.QueryAsync<UserDto>(finalSql, new
                {
                    SearchQuery = $"%{searchQuery}%",
                    Offset = offset,
                    PageSize = pageSize,
                    SchoolId = schoolId
                });

                var totalCount = await _connection.ExecuteScalarAsync<int>(countSql, new
                {
                    SearchQuery = $"%{searchQuery}%",
                    SchoolId = schoolId
                });

                callResult.IsSuccess = true;
                callResult.Data = users.ToList();
                callResult.TotalCount = totalCount;  // Add total count to the result
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Data = null;
                callResult.Message = "Fetching User List failed.";
            }

            return callResult;
        }

        // Command
        public async Task<CallResultDto<object>> AddUser(AddUserDTO user, CancellationToken ct)
        {
            var callResult = new CallResultDto<object>();

            try
            {
                var existStudent = await _payrollcontext.Users.FirstOrDefaultAsync(x => x.Email == user.Email && x.Active == true);
                if (existStudent == null)
                {
                    // Not sure sa algo gamit for password hashing
                    var hashedPassword = Encoding.UTF8.GetBytes(user.Password);

                    // Using Bycrypt for encrpyting password which will be used for password comparison
                    var encryptedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

                    var newUser = new ARS.API.Models.User
                    {
                        UserName = user.UserName,
                        Password = hashedPassword,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        CreatedDate = DateTime.Now,
                        EncryptedPassword = encryptedPassword,
                        Active = true,
                        IsEmployee = true,
                        UserTypeId = user.UserTypeId,
                        IsStudent = false,
                        Sex = ""
                    };

                    _payrollcontext.Users.Add(newUser);
                    await _payrollcontext.SaveChangesAsync(ct);

                    var newUserId = newUser.Id;

                    var newEmployee = new Ars.API.Models.Employee
                    {
                        UserId = newUserId,
                        AssignedSchoolId = user.AssignedSchoolId ?? 0,
                    };

                    _payrollcontext.Employee.Add(newEmployee);
                    await _payrollcontext.SaveChangesAsync(ct);

                    var emailSubject = "Welcome to ARS!";
                    var emailBody = $@"
                <p>Dear {user.FirstName} {user.LastName},</p>
                <p>Congratulations! You have been successfully registered.</p>
                <p>Your User Name is: <strong>{user.Email}</strong></p>
                <p>Your password is: <strong>{user.Password}</strong></p>
                <p>Best regards,</p>
                <p>School Administration</p>";

                    await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody, ct);


                    callResult.IsSuccess = true;
                    callResult.Message = "User has been successfully added.";
                }
                else 
                {
                    callResult.IsSuccess = false;
                    callResult.Message = "Email Address is already being used.";
                }
            }
            catch (Exception ex)
            {
                callResult.IsSuccess = false;
                callResult.Message = "Failed to add user.";
            }

            return callResult;
        }

        public async Task<CallResultDto<object>> AddUserAdmin(AddUserDTO user, int userId, CancellationToken ct)
        {

            var callResult = new CallResultDto<object>();
            var u = await _payrollcontext.Employee.FirstOrDefaultAsync(x => x.UserId == userId);
            var schoolId = u.AssignedSchoolId;

            try
            {
                var existStudent = await _payrollcontext.Users.FirstOrDefaultAsync(x => x.Email == user.Email && x.Active == true);
                if (existStudent == null)
                {
                    // Not sure sa algo gamit for password hashing
                    var hashedPassword = Encoding.UTF8.GetBytes(user.Password);

                    // Using Bycrypt for encrpyting password which will be used for password comparison
                    var encryptedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

                    var newUser = new ARS.API.Models.User
                    {
                        UserName = user.UserName,
                        Password = hashedPassword,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        CreatedDate = DateTime.Now,
                        EncryptedPassword = encryptedPassword,
                        Active = true,
                        IsEmployee = true,
                        UserTypeId = user.UserTypeId,
                        IsStudent = false,
                        Sex = ""
                    };

                    _payrollcontext.Users.Add(newUser);
                    await _payrollcontext.SaveChangesAsync(ct);

                    var newUserId = newUser.Id;

                    var newEmployee = new Ars.API.Models.Employee
                    {
                        UserId = newUserId,
                        AssignedSchoolId = schoolId,
                    };

                    _payrollcontext.Employee.Add(newEmployee);
                    await _payrollcontext.SaveChangesAsync(ct);

                    var emailSubject = "Welcome to ARS!";
                    var emailBody = $@"
                        <p>Dear {user.FirstName} {user.LastName},</p>
                        <p>Congratulations! You have been successfully registered.</p>
                        <p>Your User Name is: <strong>{user.Email}</strong></p>
                        <p>Your password is: <strong>{user.Password}</strong></p>
                        <p>Best regards,</p>
                        <p>School Administration</p>";

                    await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody, ct);


                    callResult.IsSuccess = true;
                    callResult.Message = "User has been successfully added.";
                }
                else 
                {
                    callResult.IsSuccess = false;
                    callResult.Message = "Email Address is already being used.";
                }
            }
            catch (Exception ex)
            {
                callResult.IsSuccess = false;
                callResult.Message = "Failed to add user.";
            }

            return callResult;
        }

        // Command
        public async Task<CallResultDto<object>> EditUser(EditUserDTO userDetail, CancellationToken ct)
        {
            var callResult = new CallResultDto<object>();

            try
            {
                var user = await _payrollcontext.Users
                    .FirstOrDefaultAsync(u => u.Id == userDetail.Id, ct);
                var employee = await _payrollcontext.Employee.FirstOrDefaultAsync(u => u.UserId == userDetail.Id, ct);
                if (user == null)
                {
                    callResult.IsSuccess = false;
                    callResult.Message = "User Not Found.";
                    return callResult; //messages here and set issucess false;
                }

                user.UserName = userDetail.UserName;
                user.FirstName = userDetail.FirstName;
                user.LastName = userDetail.LastName;
                //user.Email = userDetail.Email;
                user.Active = userDetail.Active;
                employee.AssignedSchoolId = userDetail.AssignedSchoolId;
                user.UserTypeId = userDetail.UserTypeId;
                if (user.Email != userDetail.Email) 
                {
                    var emailExists = await _payrollcontext.Users
                    .AnyAsync(u => u.Email == userDetail.Email && u.Id != userDetail.Id && u.Active == true, ct);

                    if (emailExists)
                    {
                        callResult.IsSuccess = false;
                        callResult.Message = "Email already exists.";
                        return callResult;
                    }
                    user.Email = userDetail.Email;
                    var emailSubject = "Changed Email Address";
                    var emailBody = $@"
                        <p>Dear {user.FirstName} {user.LastName},</p>
                        <p>Your email address has been changed to this email address used to receive this message.</p>
                        <p>Your New User Name is: <strong>{user.Email}</strong></p>
                        <p>Your Password is still the same as your current password</strong></p>
                        <p>Best regards,</p>
                        <p>School Administration</p>";

                    await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody, ct);
                }
                await _payrollcontext.SaveChangesAsync(ct);
                callResult.IsSuccess = true;
                callResult.Message = "User has been successfully edited.";
            }
            catch (Exception ex)
            {
                callResult.IsSuccess = false;
                callResult.Message = "Failed to Edit User.";
            }

            return callResult;

        }

        public async Task<CallResultDto<UserDto>> GetUserById(int id, CancellationToken ct)
        {
            var callResult = new CallResultDto<UserDto>();

            try
            {
                var sql = @"
                    SELECT 
                        u.Id,
                        u.UserName,
                        u.FirstName,
                        u.LastName,
                        u.Email,
                        u.Active,
                        u.UserTypeId,
                        e.AssignedSchoolId
                    FROM 
                        User u 
                    INNER JOIN 
                        Employee e ON e.UserId = u.Id
                    WHERE 
                        u.Id = @Id";

                // Execute the query using Dapper
                var user = await _connection.QueryFirstOrDefaultAsync<UserDto>(sql, new { Id = id });

                if (user != null)
                {
                    callResult.IsSuccess = true;
                    callResult.Data = user;
                }
                else
                {
                    callResult.IsSuccess = false;
                    callResult.Data = null;
                    callResult.Message = "No User Found";
                }

            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Data = null;
                callResult.Message = "Fetching User Data failed.";
            }

            return callResult;
        }

        public async Task<CallResultDto<object>> UnlockAccount(int userId, CancellationToken ct)
        {
            var callResult = new CallResultDto<object>();

            try
            {
                var user = await _payrollcontext.Users
                    .FirstOrDefaultAsync(u => u.Id == userId, ct);

                if (user == null)
                {
                    callResult.IsSuccess = false;
                    callResult.Message = "User Not Found.";
                    return callResult; //messages here and set issucess false;
                }

                user.FailedLogins = null;
                user.LoginTimeLockout = null;
                user.IsLockedOut = false;

                await _payrollcontext.SaveChangesAsync(ct);
                callResult.IsSuccess = true;
                callResult.Message = "User Account has been sucessfully unlocked.";
            }
            catch (Exception ex)
            {
                callResult.IsSuccess = false;
                callResult.Message = "Failed to Unlock User.";
            }

            return callResult;

        }

        public async Task<CallResultDto<object>> UnlockStudentAccount(int studentId, CancellationToken ct)
        {
            var callResult = new CallResultDto<object>();

            try
            {
                var student = await _payrollcontext.Students.FirstOrDefaultAsync(x => x.Id == studentId);
                var user = await _payrollcontext.Users
                    .FirstOrDefaultAsync(u => u.Id == student.UserId, ct);

                if (user == null)
                {
                    callResult.IsSuccess = false;
                    callResult.Message = "User Not Found.";
                    return callResult; //messages here and set issucess false;
                }

                user.FailedLogins = null;
                user.LoginTimeLockout = null;
                user.IsLockedOut = false;

                await _payrollcontext.SaveChangesAsync(ct);
                callResult.IsSuccess = true;
                callResult.Message = "User Account has been sucessfully unlocked.";
            }
            catch (Exception ex)
            {
                callResult.IsSuccess = false;
                callResult.Message = "Failed to Unlock User.";
            }

            return callResult;

        }

        // Temporary Method For PasswordUpdater
        public async Task<CallResultDto<object>> PasswordUpdater(string username, string password, CancellationToken ct)
        {
            var callResult = new CallResultDto<object>();

            //find user by username
            var user = await _payrollcontext.Users.Where(u => u.UserName == username).FirstOrDefaultAsync(ct);

            if (user == null)
            {
                callResult.IsSuccess = false;
                callResult.Message = "User not found!";
                return callResult;
            }

            user.EncryptedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            await _payrollcontext.SaveChangesAsync(ct);
            callResult.IsSuccess = true;
            callResult.Message = "Password updated successfully";

            return callResult;
        }
    }
}
