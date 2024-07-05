using Dapper;
using Microsoft.EntityFrameworkCore;
using Payroll.API.Context;
using Payroll.API.SharedDto;
using System.Data;

namespace ARS.API.Services.School
{
    public class SchoolService(ParyollContext payrollcontext, IDbConnection connection, IConfiguration configuration) : ISchoolService
    {
        private readonly ParyollContext _payrollcontext = payrollcontext;
        private readonly IDbConnection _connection = connection;
        private readonly IConfiguration _configuration = configuration;

        public async Task<CallResultDto<List<Models.School>>> GetSchoolList(string? searchQuery, int pageNumber, int pageSize, CancellationToken ct)
        {
            var callResult = new CallResultDto<List<Models.School>>();

            try
            {
                var baseSql = @"
        SELECT *
        FROM School s
        WHERE s.IsActive = true";

                var countSql = @"
        SELECT COUNT(*)
        FROM School s
        WHERE s.IsActive = true";

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    var searchCondition = @"
            AND (
                s.SchoolName LIKE @SearchQuery OR
                s.SchoolAcronym LIKE @SearchQuery OR
                s.SchoolAddress LIKE @SearchQuery OR
                s.SchoolEmail LIKE @SearchQuery OR
                s.SchoolContactNum LIKE @SearchQuery
            )";

                    baseSql += searchCondition;
                    countSql += searchCondition;
                }

                var paginationSql = "";
                if (pageSize > 0) // Add this check to prevent fetching 0 rows
                {
                    paginationSql = @"
    ORDER BY s.Id
    LIMIT @PageSize OFFSET @Offset"; // Use LIMIT and OFFSET instead of FETCH NEXT
                }
                var finalSql = baseSql + paginationSql;

                var offset = (pageNumber - 1) * pageSize;
                var schools = await _connection.QueryAsync<Models.School>(finalSql, new
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
                callResult.Data = schools.ToList();
                callResult.TotalCount = totalCount;  // Add total count to the result
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Data = null;
                callResult.Message = "Fetching school list failed.";
            }

            return callResult;
        }

        public async Task<CallResultDto<Models.School>> GetSchoolById(int id, CancellationToken ct)
        {
            var callResult = new CallResultDto<Models.School>();

            try
            {
                var sql = @"
            SELECT *
            FROM School s
            WHERE s.Id = @Id";

                // Execute the query using Dapper
                var school = await _connection.QueryFirstOrDefaultAsync<Models.School>(sql, new { Id = id });

                // Check if the school with the specified Id was found
                if (school != null)
                {
                    callResult.IsSuccess = true;
                    callResult.Data = school;
                    callResult.Message = "School fetched successfully.";
                }
                else
                {
                    callResult.IsSuccess = false;
                    callResult.Message = "School not found.";
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Message = "Fetching School failed.";
            }

            return callResult;
        }

        public async Task<CallResultDto<Models.School>> GetSchoolByUserId(int id, CancellationToken ct)
        {
            var callResult = new CallResultDto<Models.School>();

            try
            {
                var sql = @"
                Select s.* from school s inner join employee e on s.Id = e.AssignedSchoolId inner join user u on u.id = e.userId WHERE u.Id = @Id";

                // Execute the query using Dapper
                var school = await _connection.QueryFirstOrDefaultAsync<Models.School>(sql, new { Id = id });

                // Check if the school with the specified Id was found
                if (school != null)
                {
                    callResult.IsSuccess = true;
                    callResult.Data = school;
                    callResult.Message = "School fetched successfully.";
                }
                else
                {
                    callResult.IsSuccess = false;
                    callResult.Message = "School not found.";
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Message = "Fetching School failed.";
            }

            return callResult;
        }


        //command
        public async Task<CallResultDto<object>> AddSchool(Models.School school, CancellationToken ct)
        {
            var callResult = new CallResultDto<object>();

            try
            {
                var newschools = new Models.School
                {
                    SchoolName = school.SchoolName,
                    SchoolAcronym = school.SchoolAcronym,
                    SchoolAddress = school.SchoolAddress,
                    SchoolContactNum = school.SchoolContactNum,
                    SchoolEmail = school.SchoolEmail,
                    IsActive = true
                };

                _payrollcontext.Schools.Add(newschools);
                await _payrollcontext.SaveChangesAsync(ct);

                callResult.IsSuccess = true;
                callResult.Message = "School has been successfully added.";
            }
            catch (Exception ex)
            {
                callResult.IsSuccess = false;
                callResult.Message = "Failed to add school.";
            }

            return callResult;
        }


        public async Task<CallResultDto<object>> EditSchool(Models.School school, IFormFile? file, CancellationToken ct)
        {
            var callResult = new CallResultDto<object>();

            try
            {

                string fileName = null;
                var sc = await _payrollcontext.Schools
                    .FirstOrDefaultAsync(u => u.Id == school.Id, ct);
                if (sc == null)
                {
                    callResult.IsSuccess = false;
                    callResult.Message = "School Not Found.";
                    return callResult; 
                }
                if (file!=null)
                {
                    // Parse the filename from the URL (assuming it's the last segment)
                    fileName = $"{school.SchoolAcronym}.png"; // Change the extension if necessary

                    // Get the directory path from configuration
                    var directoryPath = _configuration["SchoolImagesDirectory"];

                    // Combine the directory path and filename
                    var filePath = Path.Combine(directoryPath, fileName);

                    // Save the file to the specified directory
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                sc.SchoolName = school.SchoolName;
                sc.SchoolEmail = school.SchoolEmail;
                sc.SchoolAcronym = school.SchoolAcronym;
                sc.SchoolAddress = school.SchoolAddress;
                sc.SchoolContactNum = school.SchoolContactNum;
                sc.IsActive = school.IsActive;
                sc.ImagePath = fileName;


                await _payrollcontext.SaveChangesAsync(ct);
                callResult.IsSuccess = true;
                callResult.Message = "School has been successfully edited.";
            }
            catch (Exception ex)
            {
                callResult.IsSuccess = false;
                callResult.Message = "Failed to Edit School.";
            }

            return callResult;

        }

        public async Task<CallResultDto<List<Models.School>>> GetAllSchools(CancellationToken ct)
        {
            var callResult = new CallResultDto<List<Models.School>>(); // Correctly initialize the callResult with List<Models.School>

            try
            {
                var sql = @"
        SELECT s.* FROM school s WHERE s.IsActive = 1";

                // Execute the query using Dapper
                var schools = await _connection.QueryAsync<Models.School>(sql);

                // Convert to a list and check if any schools were found
                var schoolList = schools.ToList();
                if (schoolList != null && schoolList.Count > 0)
                {
                    callResult.IsSuccess = true;
                    callResult.Data = schoolList;
                    callResult.Message = "Schools fetched successfully.";
                }
                else
                {
                    callResult.IsSuccess = false;
                    callResult.Message = "Schools not found.";
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Message = $"Fetching Schools failed. Error: {ex.Message}";
            }

            return callResult;
        }

    }
}
