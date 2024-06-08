using Payroll.API.Context;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data;
using Payroll.API.SharedDto;
using ARS.API.Models;
using Dapper;
using ARS.API.Services.Section.Dto;
using Microsoft.EntityFrameworkCore;

namespace ARS.API.Services.Section
{
    public class SectionService(ParyollContext payrollcontext, IDbConnection connection): ISectionService
    {
        private readonly ParyollContext _payrollcontext = payrollcontext;
        private readonly IDbConnection _connection = connection;

        public async Task<CallResultDto<List<SchoolSectionDto>>> GetSectionList(string? searchQuery, int pageNumber, int pageSize, int userId, CancellationToken ct)
        {
            var callResult = new CallResultDto<List<SchoolSectionDto>>();
            var user = await _payrollcontext.Employee.FirstOrDefaultAsync(x => x.UserId == userId);
            var schoolId = user.AssignedSchoolId;

            try
            {
                var baseSql = @"
            SELECT ss.*, gl.Level, st.StrandName
            FROM SchoolSection ss
            INNER JOIN GradeLevel gl on gl.Id = ss.GradeLevelID
            LEFT JOIN Strand st on st.Id = ss.StrandId
            WHERE ss.SchoolId = @SchoolId";

                var countSql = @"
            SELECT COUNT(*)
            FROM SchoolSection ss
            INNER JOIN GradeLevel gl on gl.Id = ss.GradeLevelID
            WHERE ss.SchoolId = @SchoolId";

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    var searchCondition = @"
                AND (
                    ss.SectionName LIKE @SearchQuery OR
                    gl.Level LIKE @SearchQuery
                )";

                    baseSql += searchCondition;
                    countSql += searchCondition;
                }

                var paginationSql = "";
                if (pageSize > 0)
                {
                    paginationSql = @"
                ORDER BY gl.Id
                LIMIT @PageSize OFFSET @Offset";
                }
                var finalSql = baseSql + paginationSql;

                var offset = (pageNumber - 1) * pageSize;
                var schoolSections = await _connection.QueryAsync<SchoolSectionDto>(finalSql, new
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
                callResult.Data = schoolSections.ToList();
                callResult.TotalCount = totalCount;  // Add total count to the result
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Data = null;
                callResult.Message = "Fetching School Section List failed.";
            }

            return callResult;

        }

        public async Task<CallResultDto<List<GradeLevel>>> GetGradeLevels(CancellationToken ct)
        {
            var callResult = new CallResultDto<List<GradeLevel>>();

            try
            {
                var sql = @"
                    SELECT * from GradeLevel";

                var userTypes = await _connection.QueryAsync<GradeLevel>(sql);

                callResult.IsSuccess = true;
                callResult.Data = userTypes.ToList();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Data = null;
                callResult.Message = "Fetching Grade Level List failed.";
            }

            return callResult;
        }

        public async Task<CallResultDto<List<SchoolSectionDto>>> GetSchoolGradeLevels(int userId, CancellationToken ct)
        {
            var callResult = new CallResultDto<List<SchoolSectionDto>>();
            var user = await _payrollcontext.Employee.FirstOrDefaultAsync(x => x.UserId == userId);
            var schoolId = user.AssignedSchoolId;
            try
            {
                var sql = @"
                     SELECT ss.Id, ss.gradeLevelId, gl.Level, st.StrandName
                        FROM SchoolSection ss
                        INNER JOIN GradeLevel gl ON gl.Id = ss.GradeLevelID
                        LEFT JOIN Strand st ON st.Id = ss.StrandId
                        WHERE ss.SchoolId = @SchoolId
                        AND ss.Id = (
                            SELECT MIN(ss2.Id)
                            FROM SchoolSection ss2
                            WHERE ss2.gradeLevelId = ss.gradeLevelId
                            AND ss2.SchoolId = ss.SchoolId
                        )
                        ORDER BY ss.gradeLevelId;
                        ";

                var userTypes = await _connection.QueryAsync<SchoolSectionDto>(sql, new { schoolId });

                callResult.IsSuccess = true;
                callResult.Data = userTypes.ToList();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Data = null;
                callResult.Message = "Fetching Grade Level List failed.";
            }

            return callResult;
        }


        public async Task<CallResultDto<List<StrandDto>>> GetStrands(CancellationToken ct)
        {
            var callResult = new CallResultDto<List<StrandDto>>();

            try
            {
                var sql = @"
                    SELECT s.*, t.trackName from Strand s
                    INNER JOIN track t on t.Id = s.trackId
                    Order by s.StrandName";

                var userTypes = await _connection.QueryAsync<StrandDto>(sql);

                callResult.IsSuccess = true;
                callResult.Data = userTypes.ToList();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Data = null;
                callResult.Message = "Fetching Strand List failed.";
            }

            return callResult;
        }
        public async Task<CallResultDto<object>> AddSection(Models.SchoolSection section, int userId, CancellationToken ct)
        {
            var callResult = new CallResultDto<object>();
            var user = await _payrollcontext.Employee.FirstOrDefaultAsync(x => x.UserId == userId);
            var schoolId = user.AssignedSchoolId;
            if (section.StrandId == 0) 
            {
                section.StrandId = null;
            }
            try
            {
                var newSection = new Models.SchoolSection
                {
                    GradeLevelId = section.GradeLevelId,
                    SchoolId = schoolId,
                    StrandId = section.StrandId,
                    SectionName = section.SectionName,
                };

                _payrollcontext.SchoolSections.Add(newSection);
                await _payrollcontext.SaveChangesAsync(ct);

                callResult.IsSuccess = true;
                callResult.Message = "Section has been successfully added.";
            }
            catch (Exception ex)
            {
                callResult.IsSuccess = false;
                callResult.Message = "Failed to add section.";
            }

            return callResult;
        }

        public async Task<CallResultDto<Models.SchoolSection>> GetSectionById(int id, CancellationToken ct)
        {
            var callResult = new CallResultDto<Models.SchoolSection>();

            try
            {
                var sql = @"
            SELECT *
            FROM schoolsection s
            WHERE s.Id = @Id";

                // Execute the query using Dapper
                var school = await _connection.QueryFirstOrDefaultAsync<Models.SchoolSection>(sql, new { Id = id });

                // Check if the school with the specified Id was found
                if (school != null)
                {
                    callResult.IsSuccess = true;
                    callResult.Data = school;
                    callResult.Message = "Section fetched successfully.";
                }
                else
                {
                    callResult.IsSuccess = false;
                    callResult.Message = "Section not found.";
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Message = "Fetching Section failed.";
            }

            return callResult;
        }

        public async Task<CallResultDto<object>> EditSection(SchoolSection section, CancellationToken ct)
        {
            var callResult = new CallResultDto<object>();

            try
            {
                var sect = await _payrollcontext.SchoolSections
                    .FirstOrDefaultAsync(u => u.Id == section.Id, ct);
                if (sect == null)
                {
                    callResult.IsSuccess = false;
                    callResult.Message = "Section Not Found.";
                    return callResult; //messages here and set issucess false;
                }

                sect.GradeLevelId = section.GradeLevelId;
                sect.SectionName = section.SectionName;
                sect.StrandId = section.StrandId;

                await _payrollcontext.SaveChangesAsync(ct);
                callResult.IsSuccess = true;
                callResult.Message = "Section has been successfully edited.";
            }
            catch (Exception ex)
            {
                callResult.IsSuccess = false;
                callResult.Message = "Failed to Edit Section.";
            }

            return callResult;


        }
        public async Task<CallResultDto<List<SchoolSection>>> GetSchoolSectionbyGrade(int id, int? strandId, CancellationToken ct)
        {
            var callResult = new CallResultDto<List<SchoolSection>>();

            try
            {
                var sql = @"
                    SELECT * FROM SchoolSection
                    WHERE GradeLevelId = @id";

                if (strandId.HasValue)
                {
                    sql += " AND StrandId = @strandId";
                }

                sql += " ORDER BY Id";
                var userTypes = await _connection.QueryAsync<SchoolSection>(sql, new { id, strandId });

                callResult.IsSuccess = true;
                callResult.Data = userTypes.ToList();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Data = null;
                callResult.Message = "Fetching School Section List failed.";
            }

            return callResult;
        }

        public async Task<CallResultDto<List<StudentBySection>>> GetTotalBySection(int userId, int syId, CancellationToken ct)
        {
            var callResult = new CallResultDto<List<StudentBySection>>();
            var user = await _payrollcontext.Employee.FirstOrDefaultAsync(x => x.UserId == userId);
            var schoolId = user.AssignedSchoolId;
            try
            {
                var sql = @"
                    SELECT 
                        gl.Level AS GradeLevel,
                        ss.SectionName,
                        SUM(CASE WHEN u.sex = 'Male' THEN 1 ELSE 0 END) AS TotalMales,
                        SUM(CASE WHEN u.sex = 'Female' THEN 1 ELSE 0 END) AS TotalFemales,
                        COUNT(u.id) AS TotalStudents
                    FROM 
                        schoolSection ss
                    LEFT JOIN 
                        enrollstudent es ON es.sectionId = ss.Id AND es.schoolYearId = @syId AND es.schoolId = @schoolId
                    LEFT JOIN 
                        student s ON es.studentId = s.id
                    LEFT JOIN 
                        user u ON s.userId = u.id
                    INNER JOIN 
                        gradeLevel gl ON gl.id = ss.GradeLevelID
                    GROUP BY 
                        gl.Level, 
                        ss.SectionName, 
                        gl.id
                    ORDER BY 
                        gl.id, 
                        ss.SectionName;

                        ";

                var userTypes = await _connection.QueryAsync<StudentBySection>(sql, new { schoolId, syId });

                callResult.IsSuccess = true;
                callResult.Data = userTypes.ToList();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Data = null;
                callResult.Message = "Fetching Grade Level List failed.";
            }

            return callResult;
        }

        public async Task<CallResultDto<StudentCount>> GetTotalBySchool(int userId, int syId, CancellationToken ct)
        {
            var callResult = new CallResultDto<StudentCount>();
            var user = await _payrollcontext.Employee.FirstOrDefaultAsync(x => x.UserId == userId);
            var schoolId = user.AssignedSchoolId;
            try
            {
                var sql = @"
                     SELECT  
                            SUM(CASE WHEN u.sex = 'Male' THEN 1 ELSE 0 END) AS TotalMales,
                            SUM(CASE WHEN u.sex = 'Female' THEN 1 ELSE 0 END) AS TotalFemales,
                            COUNT(u.id) AS TotalStudents
                        FROM 
                            enrollstudent es
                        INNER JOIN 
                            schoolSection ss ON es.sectionId = ss.Id
                        INNER JOIN 
                            gradeLevel gl ON gl.id = ss.GradeLevelID
                        INNER JOIN 
                            student s ON es.studentId = s.id
                        INNER JOIN 
                            user u ON s.userId = u.id
                        INNER JOIN 
                            schoolYear sy ON es.schoolYearId = sy.id
                        INNER JOIN 
                            school sc ON ss.schoolId = sc.id
                        WHERE es.schoolYearId = @syId and es.schoolId = @schoolId
                        ";

                var userTypes = await _connection.QueryFirstOrDefaultAsync<StudentCount>(sql, new { schoolId, syId });

                callResult.IsSuccess = true;
                callResult.Data = userTypes;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Data = null;
                callResult.Message = "Fetching Grade Level List failed.";
            }

            return callResult;
        }

        public async Task<CallResultDto<List<StudentByLevel>>> GetTotalByLevel(int userId, int syId, CancellationToken ct)
        {
            var callResult = new CallResultDto<List<StudentByLevel>>();
            var user = await _payrollcontext.Employee.FirstOrDefaultAsync(x => x.UserId == userId);
            var schoolId = user.AssignedSchoolId;
            try
            {
                var sql = @"
                     SELECT 
                        gl.Level AS GradeLevel,
                        SUM(CASE WHEN u.sex = 'Male' THEN 1 ELSE 0 END) AS TotalMales,
                        SUM(CASE WHEN u.sex = 'Female' THEN 1 ELSE 0 END) AS TotalFemales,
                        COUNT(u.id) AS TotalStudents
                    FROM 
                        enrollstudent es
                    LEFT JOIN 
                        schoolSection ss ON es.sectionId = ss.Id
                    INNER JOIN 
                        gradeLevel gl ON gl.id = ss.GradeLevelID
                    LEFT JOIN 
                        student s ON es.studentId = s.id
                    LEFT JOIN 
                        user u ON s.userId = u.id
                    WHERE 
                        es.schoolYearId = @syID AND es.schoolId = @schoolId
                    GROUP BY 
                        gl.Level, 
                        gl.id
                    ORDER BY 
                        gl.id;
                        ";

                var userTypes = await _connection.QueryAsync<StudentByLevel>(sql, new { schoolId, syId });


                callResult.IsSuccess = true;
                callResult.Data = userTypes.ToList();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Data = null;
                callResult.Message = "Fetching Grade Level List failed.";
            }

            return callResult;
        }


        public async Task<CallResultDto<List<StudentMasterlist>>> GetStudentMasterList(int userId, int syId, int gradeLevelId, int? strandId, string? searchString, CancellationToken ct)
        {
            if (strandId == 0) strandId = null;
            var callResult = new CallResultDto<List<StudentMasterlist>>();
            var user = await _payrollcontext.Employee.FirstOrDefaultAsync(x => x.UserId == userId);
            var schoolId = user.AssignedSchoolId;
            try
            {
                var sql = @"
            SELECT 
                s.StudentIdNumber, u.LastName, u.FirstName, u.MiddleName, u.Sex, s.BirthDate, s.LRN, s.ContactNumber, s.Religion, s.FatherName, s.FatherOccupation, s.MotherName, s.MotherOccupation
            FROM 
                schoolSection ss
            INNER JOIN 
                enrollstudent es ON es.sectionId = ss.Id
            INNER JOIN 
                student s ON es.studentId = s.Id
            INNER JOIN 
                user u ON s.userId = u.Id
            INNER JOIN 
                gradeLevel gl ON gl.Id = ss.GradeLevelID
            WHERE 
                es.schoolYearId = @syId AND es.schoolId = @schoolId AND ss.GradeLevelId = @gradeLevelId AND (@strandId IS NULL OR ss.strandId = @strandId)";

                if (!string.IsNullOrEmpty(searchString))
                {
                    var searchCondition = @"
                        AND (
                            s.StudentIdNumber LIKE @searchString OR
                            u.LastName LIKE @searchString OR
                            u.FirstName  LIKE @searchString OR
                            u.MiddleName  LIKE @searchString OR
                               u.Sex  LIKE @searchString OR
                            s.LRN  LIKE @searchString OR
                            s.BirthDate   LIKE @searchString 
                        )";
                    sql += searchCondition;
                }

                sql += @"
                    ORDER BY 
                        u.LastName, u.FirstName;
                ";

                var userTypes = await _connection.QueryAsync<StudentMasterlist>(sql, new { schoolId, syId, gradeLevelId, strandId, searchString = $"%{searchString}%" });


                callResult.IsSuccess = true;
                callResult.Data = userTypes.ToList();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Data = null;
                callResult.Message = "Fetching Grade Level List failed.";
            }

            return callResult;
        }

    }
}
