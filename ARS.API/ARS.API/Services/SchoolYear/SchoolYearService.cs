using Dapper;
using Microsoft.EntityFrameworkCore;
using Payroll.API.Context;
using Payroll.API.SharedDto;
using System.Data;

namespace ARS.API.Services.SchoolYear
{
    public class SchoolYearService(ParyollContext payrollcontext, IDbConnection connection) : ISchoolYearService
    {
        private readonly ParyollContext _payrollcontext = payrollcontext;
        private readonly IDbConnection _connection = connection;

        public async Task<CallResultDto<List<Models.SchoolYear>>> GetSchoolYear(string? searchQuery, int pageNumber, int pageSize, CancellationToken ct)
        {
            var callResult = new CallResultDto<List<Models.SchoolYear>>();

            try
            {
                var baseSql = @"
        SELECT *
        FROM SchoolYear s";

                var countSql = @"
        SELECT COUNT(*)
        FROM SchoolYear s";

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    var searchCondition = @"
            WHERE (
                s.FromSchoolTerm LIKE @SearchQuery OR
                s.ToSchoolTerm LIKE @SearchQuery OR
                s.IsActive LIKE @SearchQuery
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
                var schools = await _connection.QueryAsync<Models.SchoolYear>(finalSql, new
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
                callResult.Message = "Fetching school year list failed.";
            }

            return callResult;
        }


        public async Task<CallResultDto<List<Models.SchoolYear>>> GetSchoolYearActive( CancellationToken ct)
        {
            var callResult = new CallResultDto<List<Models.SchoolYear>>();

            try
            {
                var baseSql = @"
                SELECT *
                FROM SchoolYear s WHERE s.IsActive = true";


                var schools = await _connection.QueryAsync<Models.SchoolYear>(baseSql);


                callResult.IsSuccess = true;
                callResult.Data = schools.ToList();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Data = null;
                callResult.Message = "Fetching school year list failed.";
            }

            return callResult;
        }

        public async Task<CallResultDto<Models.SchoolYear>> GetSchoolYearById(int id, CancellationToken ct)
        {
            var callResult = new CallResultDto<Models.SchoolYear>();

            try
            {
                var sql = @"
            SELECT *
            FROM SchoolYear s
            WHERE s.Id = @Id";

                // Execute the query using Dapper
                var school = await _connection.QueryFirstOrDefaultAsync<Models.SchoolYear>(sql, new { Id = id });

                // Check if the school with the specified Id was found
                if (school != null)
                {
                    callResult.IsSuccess = true;
                    callResult.Data = school;
                    callResult.Message = "School Year fetched successfully.";
                }
                else
                {
                    callResult.IsSuccess = false;
                    callResult.Message = "School Year not found.";
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Message = "Fetching School Year failed.";
            }

            return callResult;
        }

        public async Task<CallResultDto<object>> AddSchoolYear(Models.SchoolYear schoolyear, CancellationToken ct)
        {
            var callResult = new CallResultDto<object>();

            try
            {
                var newschoolYear = new Models.SchoolYear
                {
                    FromSchoolTerm = schoolyear.FromSchoolTerm,
                    ToSchoolTerm = schoolyear.ToSchoolTerm,
                    IsActive = schoolyear.IsActive,
                 
                };

                _payrollcontext.SchoolYears.Add(newschoolYear);
                await _payrollcontext.SaveChangesAsync(ct);

                callResult.IsSuccess = true;
                callResult.Message = "SchoolYear has been successfully added.";
            }
            catch (Exception ex)
            {
                callResult.IsSuccess = false;
                callResult.Message = "Failed to add school year.";
            }

            return callResult;
        }

        public async Task<CallResultDto<object>> EditSchoolYear(Models.SchoolYear schoolyear, CancellationToken ct)
        {
            var callResult = new CallResultDto<object>();

            try
            {
                var newschoolYear = await _payrollcontext.SchoolYears.FirstOrDefaultAsync(x => x.Id == schoolyear.Id);
                newschoolYear.FromSchoolTerm = schoolyear.FromSchoolTerm;
                newschoolYear.ToSchoolTerm = schoolyear.ToSchoolTerm;
                newschoolYear.IsActive = schoolyear.IsActive;

                
                await _payrollcontext.SaveChangesAsync(ct);

                callResult.IsSuccess = true;
                callResult.Message = "SchoolYear has been successfully edited.";
            }
            catch (Exception ex)
            {
                callResult.IsSuccess = false;
                callResult.Message = "Failed to edit school year.";
            }

            return callResult;
        }

        public async Task<CallResultDto<int>> GetCurrentSchoolTerm(CancellationToken ct)
        {
            var callResult = new CallResultDto<int>();

            try
            {
                var activeSchoolYears = await _payrollcontext.SchoolYears
                    .Where(x => x.IsActive)
                    .ToListAsync(ct);

                if (activeSchoolYears.Count == 0)
                {
                    callResult.IsSuccess = false;
                    callResult.Message = "No active school years found.";
                    return callResult;
                }

                var currentSchoolYear = activeSchoolYears
                    .Select(sy => new
                    {
                        sy.Id,
                        FromSchoolTerm = int.TryParse(sy.FromSchoolTerm, out int fromTerm) ? fromTerm : (int?)null
                    })
                    .Where(sy => sy.FromSchoolTerm.HasValue)
                    .OrderBy(sy => sy.FromSchoolTerm)
                    .FirstOrDefault();

                if (currentSchoolYear == null)
                {
                    callResult.IsSuccess = false;
                    callResult.Message = "No valid school terms found.";
                    return callResult;
                }

                callResult.IsSuccess = true;
                callResult.Message = "Current school term retrieved successfully.";
                callResult.Data = currentSchoolYear.Id;
            }
            catch (Exception ex)
            {
                callResult.IsSuccess = false;
                callResult.Message = $"Failed to retrieve current school term: {ex.Message}";
            }

            return callResult;
        }


    }
}
