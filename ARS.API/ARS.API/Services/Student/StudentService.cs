using ARS.API.Models;
using ARS.API.Services.Email;
using ARS.API.Services.Student.Dto;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Payroll.API.Context;
using Payroll.API.SharedDto;
using System.Data;
using System.Text;

namespace ARS.API.Services.Student
{
    public class StudentService(ParyollContext payrollcontext, IDbConnection connection, IEmailService emailService) : IStudentService
    {
        private readonly ParyollContext _payrollcontext = payrollcontext;
        private readonly IDbConnection _connection = connection;
        private readonly IEmailService _emailService = emailService;

        public async Task<CallResultDto<object>> AddStudentRegistrar(StudentDto student, int userId, bool? isStudentRegistered, CancellationToken ct)
        {
            var callResult = new CallResultDto<object>();
            try
            {
                var existStudent = await _payrollcontext.Users.FirstOrDefaultAsync(x => x.Email == student.Email);
                if (existStudent == null)
                {
                    var schoolId = 0;
                    if (isStudentRegistered == true)
                    {
                        schoolId = userId;
                    }
                    else
                    {
                        var u = await _payrollcontext.Employee.FirstOrDefaultAsync(x => x.UserId == userId);
                        schoolId = u.AssignedSchoolId;
                    }
                    var studentsCount = await _payrollcontext.Students.CountAsync(x => x.BirthDate == student.Birthdate);
                    var studentIdNumber = $"{student.Birthdate.Month:00}{student.Birthdate.Day:00}{student.Birthdate.Year % 100:00}{studentsCount + 1:000}";
                    var age = DateTime.UtcNow.Year - student.Birthdate.Year;
                    if (DateTime.UtcNow < student.Birthdate.AddYears(age))
                    {
                        age--; // Decrease age if the birthday hasn't occurred yet this year
                    }

                    string passwordToHash = student.LastName + "123"; // need to change more credible

                    // Convert the concatenated string to bytes
                    var passwordBytes = Encoding.UTF8.GetBytes(passwordToHash);

                    // Hash the password using BCrypt
                    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(passwordToHash);

                    var newUser = new Models.User
                    {
                        UserName = student.Email,
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        MiddleName = student.MiddleName,
                        Password = passwordBytes,
                        EncryptedPassword = hashedPassword,
                        Email = student.Email,
                        Active = true,
                        CreatedBy = userId,
                        CreatedDate = DateTime.UtcNow,
                        IsStudent = true,
                        IsEmployee = false,
                        Sex = student.Sex,
                        Suffix = student.Suffix,
                        UserTypeId = 1
                    };

                    _payrollcontext.Users.Add(newUser);
                    await _payrollcontext.SaveChangesAsync(ct);

                    var newstudent = new Models.Student
                    {
                        StudentIdNumber = studentIdNumber,
                        ContactNumber = student.ContactNumber,
                        LRN = student.LRN,
                        BirthDate = student.Birthdate,
                        Age = age,
                        BirthPlace = student.Birthplace,
                        CivilStatus = student.CivilStatus,
                        Religion = student.Religion,
                        MotherName = student.MothersName,
                        MotherAddress = student.MothersAddress,
                        FatherName = student.FathersName,
                        FatherAddress = student.FathersAddress,
                        GuardianName = student.GuardiansName,
                        GuardianAddress = student.GuardiansAddress,
                        LastSchoolAttended = student.LastSchoolAttended,
                        LastSchoolAttendedYear = student.LastSchoolAttendedYear,
                        IsRegistered = true,
                        IsEnrolled = false,
                        IsForConfirmation = false,
                        IsActive = true,
                        IsGraduated = false,
                        UserId = newUser.Id, // Set this value appropriately
                        SchoolId = schoolId,// Set this value appropriately
                        GradeLevelId = student.GradeLevelId, // Set this value appropriately
                        StrandId = student.StrandId, // Set this value appropriately
                        MotherContactNumber = student.MothersContactNumber,
                        FatherContactNumber = student.FathersContactNumber,
                        GuardianContactNumber = student.GuardiansContactNumber,
                        MotherEmailAddress = student.MothersEmailAddress,
                        FatherEmailAddress = student.FathersEmailAddress,
                        GuardianEmailAddress = student.GuardiansEmailAddress,
                        FatherOccupation = student.FatherOccupation,
                        MotherOccupation = student.MotherOccupation,
                        IsMotherDeceased = student.IsMotherDeceased,
                        IsFatherDeceased = student.IsFatherDeceased,
                        GuardianRelationship = student.GuardianRelationship,
                        DateRegistered = DateTime.UtcNow,
                        StudentAddress = student.StudentAddress
                    };
                    _payrollcontext.Students.Add(newstudent);
                    await _payrollcontext.SaveChangesAsync(ct);

                    var emailSubject = "Welcome to Our School!";
                    var emailBody = $@"
                <p>Dear {student.FirstName} {student.LastName},</p>
                <p>Congratulations! You have been successfully registered.</p>
                <p>Your Student ID Number is: <strong>{studentIdNumber}</strong></p>
                <p>Your User Name is: <strong>{student.Email}</strong></p>
                <p>Your temporary password is: <strong>{passwordToHash}</strong></p>
                <p>Please log in and change your password as soon as possible.</p>
                <p>Best regards,</p>
                <p>School Administration</p>";

                    await _emailService.SendEmailAsync(student.Email, emailSubject, emailBody, ct);

                    callResult.IsSuccess = true;
                    callResult.Message = "Student has been successfully registered.";
                }
                else 
                {
                    callResult.IsSuccess = false;
                    callResult.Message = $"The email address {student.Email} is already registered in our system. If you believe this is an error or if you need assistance, please check your inbox for a confirmation email or contact the school.";
                }
            }
            catch (Exception ex)
            {
                callResult.IsSuccess = false;
                callResult.Message = "Failed to register student.";
            }

            return callResult;
        }

        public async Task<CallResultDto<List<StudentDto>>> GetStudentList(string? searchQuery, int pageNumber, int pageSize, int userId, CancellationToken ct)
        {
            var callResult = new CallResultDto<List<StudentDto>>();
            var u = await _payrollcontext.Employee.FirstOrDefaultAsync(x => x.UserId == userId);
            var schoolId = u.AssignedSchoolId;
            try
            {
                var baseSql = @"
            SELECT s.Id, u.LastName,u.MiddleName, u.FirstName, u.Email, s.ContactNumber, s.LRN, s.BirthDate AS Birthdate, 
                   s.BirthPlace, s.CivilStatus, s.Religion, s.MotherName, s.MotherAddress, s.FatherName, 
                   s.FatherAddress, s.GuardianName, s.GuardianAddress, s.LastSchoolAttended, s.LastSchoolAttendedYear, u.Sex, s.StudentAddress
            FROM Student s
            INNER JOIN User u ON s.UserId = u.Id
            WHERE s.IsActive = true and s.SchoolId = @schoolId and s.IsRegistered = true and s.IsEnrolled = false";

                var countSql = @"
            SELECT COUNT(*)
            FROM Student s
            INNER JOIN User u ON s.UserId = u.Id
            WHERE s.IsActive = true and s.SchoolId = @schoolId  and s.IsRegistered = true  and s.IsEnrolled = false";

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    var searchCondition = @"
                AND (
                    u.LastName LIKE @SearchQuery OR
                    u.FirstName LIKE @SearchQuery OR
                    s.LRN LIKE @SearchQuery OR
                    s.BirthPlace LIKE @SearchQuery OR
                    s.Religion LIKE @SearchQuery OR
                    s.MotherName LIKE @SearchQuery OR
                    s.FatherName LIKE @SearchQuery OR
                    s.GuardianName LIKE @SearchQuery OR
                    s.LastSchoolAttended LIKE @SearchQuery
                )";

                    baseSql += searchCondition;
                    countSql += searchCondition;
                }

                var paginationSql = "";
                if (pageSize > 0) // Add this check to prevent fetching 0 rows
                {
                    paginationSql = @"
                ORDER BY s.Id
                LIMIT @PageSize OFFSET @Offset"; // Use LIMIT and OFFSET
                }
                var finalSql = baseSql + paginationSql;

                var offset = (pageNumber - 1) * pageSize;
                var students = await _connection.QueryAsync<StudentDto>(finalSql, new
                {
                    SearchQuery = $"%{searchQuery}%",
                    Offset = offset,
                    PageSize = pageSize,
                    schoolId
                });

                var totalCount = await _connection.ExecuteScalarAsync<int>(countSql, new
                {
                    SearchQuery = $"%{searchQuery}%",
                    schoolId
                });

                callResult.IsSuccess = true;
                callResult.Data = students.ToList();
                callResult.TotalCount = totalCount;  // Add total count to the result
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Data = null;
                callResult.Message = "Fetching student list failed.";
            }

            return callResult;
        }

        public async Task<CallResultDto<StudentDto>> GetStudentById(int studentId, bool fromStudent, CancellationToken ct)
        {
            var callResult = new CallResultDto<StudentDto>();
            if (fromStudent == false)
            {
                var u = await _payrollcontext.Students.FirstOrDefaultAsync(x => x.Id == studentId);
                studentId = u.UserId;
            }
            try
            {
                var sql = @"
                SELECT s.Id, u.LastName, u.MiddleName, u.FirstName, u.Email, s.ContactNumber, s.LRN, s.BirthDate AS Birthdate, u.Sex,u.Suffix, s.IsMotherDeceased, s.IsFatherDeceased, s.FatherOccupation , s.MotherOccupation, s.GuardianRelationship,
                       s.BirthPlace, s.CivilStatus, s.Religion, s.MotherName as MothersName, s.MotherAddress as MothersAddress, s.FatherName as FathersName, s.Age, s.StudentIdNumber,
                       s.FatherAddress as FathersAddress, s.GuardianName as GuardiansName, s.GuardianAddress as GuardiansAddress, s.LastSchoolAttended, s.LastSchoolAttendedYear,s.StudentAddress,
                       s.GradeLevelId, s.StrandId, s.MotherContactNumber MothersContactNumber, s.FatherContactNumber FathersContactNumber, s.GuardianContactNumber GuardiansContactNumber, s.MotherEmailAddress MothersEmailAddress, s.FatherEmailAddress FathersEmailAddress, s.GuardianEmailAddress GuardiansEmailAddress
                FROM Student s
                INNER JOIN User u ON s.UserId = u.Id
                WHERE u.Id = @studentId";

                var student = await _connection.QuerySingleOrDefaultAsync<StudentDto>(sql, new
                {
                    studentId
                });

                if (student != null)
                {
                    callResult.IsSuccess = true;
                    callResult.Data = student;
                }
                else
                {
                    callResult.IsSuccess = false;
                    callResult.Message = "Student not found.";
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Data = null;
                callResult.Message = "Fetching student details failed.";
            }

            return callResult;
        }

        public async Task<CallResultDto<object>> EditStudentRegistrar(StudentDto student, int userId, CancellationToken ct)
        {
            var callResult = new CallResultDto<object>();
            try
            {
                // First, retrieve the student
                var existingStudent = await _payrollcontext.Students.FirstOrDefaultAsync(x => x.Id == student.Id, ct);


                var emailExists = await _payrollcontext.Users
                        .AnyAsync(u => u.Email == student.Email && u.Id != existingStudent.UserId && u.Active == true, ct);

                if (emailExists)
                {
                    callResult.IsSuccess = false;
                    callResult.Message = "Email already exists.";
                    return callResult;
                }

                if (existingStudent != null)
                {
                    // Update student fields from the StudentDto
                    existingStudent.ContactNumber = student.ContactNumber;
                    existingStudent.LRN = student.LRN;
                    existingStudent.BirthDate = student.Birthdate;
                    existingStudent.BirthPlace = student.Birthplace;
                    existingStudent.CivilStatus = student.CivilStatus;
                    existingStudent.Religion = student.Religion;
                    existingStudent.MotherName = student.MothersName;
                    existingStudent.MotherAddress = student.MothersAddress;
                    existingStudent.FatherName = student.FathersName;
                    existingStudent.FatherAddress = student.FathersAddress;
                    existingStudent.GuardianName = student.GuardiansName;
                    existingStudent.GuardianAddress = student.GuardiansAddress;
                    existingStudent.LastSchoolAttended = student.LastSchoolAttended;
                    existingStudent.LastSchoolAttendedYear = student.LastSchoolAttendedYear;
                    existingStudent.GradeLevelId = student.GradeLevelId;
                    existingStudent.StrandId = student.StrandId;
                    existingStudent.MotherContactNumber = student.MothersContactNumber;
                    existingStudent.FatherContactNumber = student.FathersContactNumber;
                    existingStudent.GuardianContactNumber = student.GuardiansContactNumber;
                    existingStudent.MotherEmailAddress = student.MothersEmailAddress;
                    existingStudent.FatherEmailAddress = student.FathersEmailAddress;
                    existingStudent.GuardianEmailAddress = student.GuardiansEmailAddress;
                    existingStudent.GuardianRelationship = student.GuardianRelationship;
                    existingStudent.MotherOccupation = student.MotherOccupation;
                    existingStudent.FatherOccupation = student.FatherOccupation;
                    existingStudent.IsFatherDeceased = student.IsFatherDeceased;
                    existingStudent.IsMotherDeceased = student.IsMotherDeceased;
                    existingStudent.GradeLevelId = student.GradeLevelId;
                    existingStudent.StudentAddress = student.StudentAddress;

                    // Save changes to the student entity
                    await _payrollcontext.SaveChangesAsync(ct);

                    // Now, update user fields
                    var associatedUser = await _payrollcontext.Users.FirstOrDefaultAsync(u => u.Id == existingStudent.UserId, ct);
                    if (associatedUser != null)
                    {
                        // Update user fields from the StudentDto
                        associatedUser.FirstName = student.FirstName;
                        associatedUser.LastName = student.LastName;
                        associatedUser.MiddleName = student.MiddleName;
                        associatedUser.UpdatedBy = userId;
                        associatedUser.Sex = student.Sex;
                        associatedUser.UpdatedDate = DateTime.UtcNow;
                        associatedUser.Suffix = student.Suffix;

                        if (associatedUser.Email != student.Email)
                        {
                            associatedUser.Email = student.Email;
                            associatedUser.UserName = student.Email;
                            var emailSubject = "Changed Email Address";
                            var emailBody = $@"
                            <p>Dear {associatedUser.FirstName} {associatedUser.LastName},</p>
                            <p>Your email address has been changed to this email address used to receive this message.</p>
                            <p>Your New User Name is: <strong>{associatedUser.Email}</strong></p>
                            <p>Your Password is still the same as your current password</strong></p>
                            <p>Best regards,</p>
                            <p>School Administration</p>";

                            await _emailService.SendEmailAsync(associatedUser.Email, emailSubject, emailBody, ct);
                        }

                        await _payrollcontext.SaveChangesAsync(ct);

                        callResult.IsSuccess = true;
                        callResult.Message = "Student and associated user details have been successfully updated.";
                    }
                    else
                    {
                        callResult.IsSuccess = false;
                        callResult.Message = "Associated user not found.";
                    }
                }
                else
                {
                    callResult.IsSuccess = false;
                    callResult.Message = "Student not found.";
                }
            }
            catch (Exception ex)
            {
                callResult.IsSuccess = false;
                callResult.Message = "Failed to update student and associated user details.";
                // Log the exception here if needed
            }

            return callResult;
        }

        public async Task<CallResultDto<List<StudentForEnroll>>> GetStudentListForEnrollment(string? searchQuery, int pageNumber, int pageSize, int userId, CancellationToken ct)
        {
            var callResult = new CallResultDto<List<StudentForEnroll>>();
            var u = await _payrollcontext.Employee.FirstOrDefaultAsync(x => x.UserId == userId);
            var schoolId = u.AssignedSchoolId;
            try
            {
                var baseSql = @"
                    SELECT *
        FROM (
            SELECT 
                s.Id, 
                u.LastName, 
                u.MiddleName, 
                u.FirstName, 
                u.Email, 
                s.ContactNumber, 
                s.LRN, 
                s.StudentIdNumber, 
                s.GradeLevelId,
                g.Level AS GradeLevel, 
                st.strandName,
                s.strandId,
                s.IsEnrolled,
                CASE 
                    WHEN s.RequestToMoveUp = true AND s.IsEnrolled = true THEN true
                    WHEN s.RequestToMoveUp = false AND s.IsEnrolled = false THEN true
                    WHEN s.RequestToMoveUp = false AND s.IsEnrolled = true THEN false
                    ELSE s.IsEnrolled
                END AS IsForEnrolled
                FROM 
                    Student s
                INNER JOIN 
                    User u ON s.UserId = u.Id
                INNER JOIN 
                    GradeLevel g ON s.GradeLevelId = g.Id
                LEFT JOIN 
                    Strand st ON st.Id = s.StrandId
                WHERE 
                    s.IsActive = true 
                    AND s.SchoolId = @schoolId 
                    AND s.IsRegistered = true
           
            ";

                var countSql = @"
    SELECT COUNT(*)
    FROM (
        SELECT 
            s.Id, 
                u.LastName, 
                u.MiddleName, 
                u.FirstName, 
                u.Email, 
                s.ContactNumber, 
                s.LRN, 
                s.StudentIdNumber, 
                s.GradeLevelId,
                g.Level AS GradeLevel, 
                st.strandName,
                s.strandId,
                s.IsEnrolled,
            CASE 
                WHEN s.RequestToMoveUp = true AND s.IsEnrolled = true THEN true
                WHEN s.RequestToMoveUp = false AND s.IsEnrolled = false THEN true
                WHEN s.RequestToMoveUp = false AND s.IsEnrolled = true THEN false
                ELSE s.IsEnrolled
            END AS IsForEnrolled
        FROM 
            Student s
        INNER JOIN 
            User u ON s.UserId = u.Id
        INNER JOIN 
            GradeLevel g ON s.GradeLevelId = g.Id
        LEFT JOIN 
            Strand st ON st.Id = s.StrandId
        WHERE 
            s.IsActive = true 
            AND s.SchoolId = @schoolId 
            AND s.IsRegistered = true
   ";


                if (!string.IsNullOrEmpty(searchQuery))
                {
                    var searchCondition = @"
                AND (
                    u.LastName LIKE @SearchQuery OR
                    u.FirstName LIKE @SearchQuery OR
                    s.LRN LIKE @SearchQuery OR
                    g.Level LIKE @SearchQuery OR
                    st.strandName LIKE @SearchQuery OR
                    s.StudentIdNumber LIKE @SearchQuery
                )";

                    baseSql += searchCondition;
                    countSql += searchCondition;
                }
                baseSql += @" ) AS subquery
            WHERE 
                IsForEnrolled = 1";
                countSql += @" ) AS subquery
    WHERE 
        IsForEnrolled = 1";
                var paginationSql = "";
                if (pageSize > 0) // Add this check to prevent fetching 0 rows
                {
                    paginationSql = @"
                ORDER BY s.Id
                LIMIT @PageSize OFFSET @Offset"; // Use LIMIT and OFFSET
                }
                var finalSql = baseSql + paginationSql;

                var offset = (pageNumber - 1) * pageSize;
                var students = await _connection.QueryAsync<StudentForEnroll>(finalSql, new
                {
                    SearchQuery = $"%{searchQuery}%",
                    Offset = offset,
                    PageSize = pageSize,
                    schoolId
                });

                var totalCount = await _connection.ExecuteScalarAsync<int>(countSql, new
                {
                    SearchQuery = $"%{searchQuery}%",
                    schoolId
                });

                callResult.IsSuccess = true;
                callResult.Data = students.ToList();
                callResult.TotalCount = totalCount;  // Add total count to the result
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Data = null;
                callResult.Message = "Fetching student list failed.";
            }

            return callResult;
        }


        public async Task<CallResultDto<object>> EnrollStudent(int userId, int studentId, int sectionId, int syId, int? semId, int? strandId, CancellationToken ct)
        {
            var callResult = new CallResultDto<object>();
            try
            {
                var u = await _payrollcontext.Employee.FirstOrDefaultAsync(x => x.UserId == userId);
                var schoolId = u.AssignedSchoolId;
                var newEnrollee = new EnrollStudent
                {
                    StudentId = studentId,
                    SchoolId = schoolId,
                    SectionId = sectionId,
                    SchoolYearId = syId,
                    SemesterId = semId,
                    DateEnrolled = DateTime.Now,
                };
                await _payrollcontext.EnrollStudents.AddAsync(newEnrollee, ct);
                var schoolSection = await _payrollcontext.SchoolSections.FirstOrDefaultAsync(x => x.Id == sectionId);
                var student = await _payrollcontext.Students.FirstOrDefaultAsync(x => x.Id == studentId);
                student.IsEnrolled = true;
                student.GradeLevelId = schoolSection.GradeLevelId;
                if (strandId != null) student.StrandId = strandId;
                await _payrollcontext.SaveChangesAsync(ct);

                callResult.IsSuccess = true;
                callResult.Message = "Student has been successfully enrolled.";
            }
            catch (Exception ex)
            {
                callResult.IsSuccess = false;
                callResult.Message = "Failed to enroll Student.";
            }

            return callResult;
        }

        public async Task<CallResultDto<List<StudentForEnroll>>> GetStudentListEnrolled(string? searchQuery, int pageNumber, int pageSize, int userId, int syId, CancellationToken ct)
        {
            var callResult = new CallResultDto<List<StudentForEnroll>>();
            var u = await _payrollcontext.Employee.FirstOrDefaultAsync(x => x.UserId == userId);
            var schoolId = u.AssignedSchoolId;
            try
            {
                var baseSql = @"
                    SELECT 
                    s.Id, 
                    u.LastName, 
                    u.MiddleName, 
                    u.FirstName, 
                    u.Email, 
                    s.ContactNumber, 
                    s.LRN, 
                    s.StudentIdNumber, 
                    s.GradeLevelId,
                    g.Level AS GradeLevel, 
                    st.strandName,
                    s.BirthDate,
                    s.Age,
                    s.BirthPlace,
                    s.CivilStatus,
                    s.Religion,
                    s.MotherName,
                    s.MotherAddress,
                    s.FatherName,
                    s.FatherAddress,
                    s.GuardianName,
                    s.GuardianAddress,
                    s.LastSchoolAttended,
                    s.LastSchoolAttendedYear,
                    s.MotherContactNumber,
                    s.FatherContactNumber,
                    s.GuardianContactNumber,
                    s.MotherEmailAddress,
                    s.FatherEmailAddress,
                    s.GuardianEmailAddress,
                    u.Sex,
                    s.StudentAddress,
                    u.IsLockedOut,
                    ssGradeLevel.Level AS GradeLevelForSy
                FROM Student s
                INNER JOIN User u ON s.UserId = u.Id
                INNER JOIN GradeLevel g ON s.GradeLevelId = g.Id
                LEFT JOIN Strand st ON st.Id = s.StrandId
                INNER JOIN EnrollStudent es ON s.Id = es.StudentId
                INNER JOIN SchoolSection ss ON ss.id = es.sectionId
                INNER JOIN GradeLevel ssGradeLevel ON ss.gradeLevelId = ssGradeLevel.Id
                WHERE s.IsActive = true 
                  AND s.SchoolId = @schoolId 
                  AND s.IsRegistered = true  
                  AND s.IsEnrolled = true
                  AND es.SchoolYearId = @syId";

                                var countSql = @"
                    SELECT COUNT(*)
                FROM Student s
                INNER JOIN User u ON s.UserId = u.Id
                INNER JOIN GradeLevel g ON s.GradeLevelId = g.Id
                LEFT JOIN Strand st ON st.Id = s.StrandId
                INNER JOIN EnrollStudent es ON s.Id = es.StudentId
                INNER JOIN SchoolSection ss ON ss.id = es.sectionId
                INNER JOIN GradeLevel ssGradeLevel ON ss.gradeLevelId = ssGradeLevel.Id
                    WHERE s.IsActive = true 
                      AND s.SchoolId = @schoolId 
                      AND s.IsRegistered = true  
                      AND s.IsEnrolled = true
                      AND es.SchoolYearId = @syId";

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    var searchCondition = @"
                    AND (
                        u.LastName LIKE @SearchQuery OR
                        u.FirstName LIKE @SearchQuery OR
                        g.Level LIKE @SearchQuery
                    )";

                    baseSql += searchCondition;
                    countSql += searchCondition;
                }

                var paginationSql = "";
                if (pageSize > 0) // Add this check to prevent fetching 0 rows
                {
                    paginationSql = @"
                   ORDER BY s.Id
                LIMIT @PageSize OFFSET @Offset"; // Adjust for SQL Server syntax
                }
                var finalSql = baseSql + paginationSql;

                var offset = (pageNumber - 1) * pageSize;
                var students = await _connection.QueryAsync<StudentForEnroll>(finalSql, new
                {
                    SearchQuery = $"%{searchQuery}%",
                    Offset = offset,
                    PageSize = pageSize,
                    schoolId,
                    syId
                });

                var totalCount = await _connection.ExecuteScalarAsync<int>(countSql, new
                {
                    SearchQuery = $"%{searchQuery}%",
                    schoolId,
                    syId
                });

                callResult.IsSuccess = true;
                callResult.Data = students.ToList();
                callResult.TotalCount = totalCount;  // Add total count to the result
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Data = null;
                callResult.Message = $"Fetching student list failed: {ex.Message}";
            }

            return callResult;
        }



        public async Task<CallResultDto<List<EnrollStudentDto>>> GetEnrollmentHistory(string? searchQuery, int pageNumber, int pageSize, int studentId, CancellationToken ct)
        {
            var callResult = new CallResultDto<List<EnrollStudentDto>>();
            try
            {
                var baseSql = @"
                   Select e.Id, e.DateEnrolled, ss.sectionName, gl.Level from enrollstudent e inner join schoolsection ss on ss.id = e.sectionId inner join gradelevel gl on gl.id = ss.gradelevelId WHERE e.studentId = @studentId";

                var countSql = @"
            SELECT COUNT(*)
                from enrollstudent e inner join schoolsection ss on ss.id = e.sectionId inner join gradelevel gl on gl.id = ss.gradelevelId WHERE e.studentId = @studentId";



                if (!string.IsNullOrEmpty(searchQuery))
                {
                    var searchCondition = @"
                AND (
                   ss.sectionName LIKE @SearchQuery OR
                    e.DateEnrolled LIKE @SearchQuery OR
                    g.Level Like @SearchQuery 
                )";

                    baseSql += searchCondition;
                    countSql += searchCondition;
                }

                var paginationSql = "";
                if (pageSize > 0) // Add this check to prevent fetching 0 rows
                {
                    paginationSql = @"
                ORDER BY e.Id
                LIMIT @PageSize OFFSET @Offset"; // Use LIMIT and OFFSET
                }
                var finalSql = baseSql + paginationSql;

                var offset = (pageNumber - 1) * pageSize;
                var students = await _connection.QueryAsync<EnrollStudentDto>(finalSql, new
                {
                    SearchQuery = $"%{searchQuery}%",
                    Offset = offset,
                    PageSize = pageSize,
                    studentId
                });

                var totalCount = await _connection.ExecuteScalarAsync<int>(countSql, new
                {
                    SearchQuery = $"%{searchQuery}%",
                    studentId
                });

                callResult.IsSuccess = true;
                callResult.Data = students.ToList();
                callResult.TotalCount = totalCount;  // Add total count to the result
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Data = null;
                callResult.Message = "Fetching student list failed.";
            }

            return callResult;
        }

        public async Task<CallResultDto<bool>> GetMoveUpStatus(int studentId, CancellationToken ct)
        {
            var callResult = new CallResultDto<bool>();
            try
            {
                // Get the list of enrolled student records for the given student, ordered by SchoolYearId descending
                var lastEnrollment = await _payrollcontext.EnrollStudents
                    .Where(x => x.StudentId == studentId)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync(ct);

                // Check if the student has any enrolled records
                if (lastEnrollment != null)
                {
                    // Get the details of the last SchoolYear
                    var lastSchoolYear = await _payrollcontext.SchoolYears
                        .FirstOrDefaultAsync(x => x.Id == lastEnrollment.SchoolYearId, ct);

                    // Get the active school years
                    var activeSchoolYears = await _payrollcontext.SchoolYears
                        .Where(x => x.IsActive)
                        .ToListAsync(ct);

                    // Check if any active school year has greater terms than the last enrolled school year
                    var moveUpStatus = activeSchoolYears.Any(sy =>
                        (int.Parse(sy.FromSchoolTerm) > int.Parse(lastSchoolYear.FromSchoolTerm)) &&
                        (int.Parse(sy.ToSchoolTerm) > int.Parse(lastSchoolYear.ToSchoolTerm)));

                    if (moveUpStatus == true)
                    {
                        callResult.IsSuccess = true;
                        callResult.Message = "Move up status determined successfully.";
                        callResult.Data = true;
                    }
                    // Set the result
                    else
                    {
                        callResult.IsSuccess = false;
                        callResult.Message = "You are currently enrolled from the last active school term.";
                        callResult.Data = false;
                    }
                }
                else
                {
                    // No enrollment records found for the student
                    callResult.IsSuccess = false;
                    callResult.Message = "No enrollment records found for the student.";
                    callResult.Data = false;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                callResult.IsSuccess = false;
                callResult.Message = $"Failed to determine move up status. Error: {ex.Message}";
                callResult.Data = false;
            }

            return callResult;
        }

        public async Task<CallResultDto<object>> RequestToMoveUp(int studentId, CancellationToken ct)
        {
            var callResult = new CallResultDto<object>();
            try
            {
                var student = await _payrollcontext.Students.FirstOrDefaultAsync(x => x.Id == studentId);

                student.RequestToMoveUp = true;

                await _payrollcontext.SaveChangesAsync(ct);
                callResult.IsSuccess = true;
                callResult.Message = "Successfully Requested";


            }
            catch (Exception ex)
            {
                // Handle exceptions
                callResult.IsSuccess = false;
                callResult.Message = $"Failed to determine move up status. Error: {ex.Message}";

            }

            return callResult;
        }

        public async Task<CallResultDto<List<StudentReport>>> GetStudentListReport(int userId, int gradeLevelId, int syId, CancellationToken ct)
        {
            var callResult = new CallResultDto<List<StudentReport>>();
            var u = await _payrollcontext.Employee.FirstOrDefaultAsync(x => x.UserId == userId);
            var schoolId = u.AssignedSchoolId;
            try
            {
                var baseSql = @"
                SELECT LastName, FirstName, MiddleName, Sex 
                from User u Inner join student s on u.Id = s.userId 
                inner join enrollstudent e on s.id = e.studentId 
                WHERE s.gradeLevelId = @gradeLevelId and s.schoolId = @schoolId and e.schoolYearId = @syId and s.IsEnrolled = 1";
                var countSql = @"
                SELECT COUNT(*)
                from User u Inner join student s on u.Id = s.userId 
                inner join enrollstudent e on s.id = e.studentId 
                WHERE s.gradeLevelId = @gradeLevelId and s.schoolId = @schoolId and e.schoolYearId = @syId and s.IsEnrolled = 1";


                var finalSql = baseSql;


                var students = await _connection.QueryAsync<StudentReport>(finalSql, new
                {
                    gradeLevelId,
                    syId,
                    schoolId
                });

                var totalCount = await _connection.ExecuteScalarAsync<int>(countSql, new
                {
                    gradeLevelId,
                    syId,
                    schoolId
                });

                callResult.IsSuccess = true;
                callResult.Data = students.ToList();
                callResult.TotalCount = totalCount;  // Add total count to the result
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Data = null;
                callResult.Message = "Fetching student list failed.";
            }

            return callResult;
        }

        public async Task<CallResultDto<List<StudentAdminDto>>> GetStudentListSuperAdmin(string? searchQuery, int pageNumber, int pageSize, CancellationToken ct)
        {
            var callResult = new CallResultDto<List<StudentAdminDto>>();
            try
            {
                var baseSql = @"
             Select student.Id, user.LAstName, user.firstNAme, user.MiddleName, user.suffix, student.StudentIdNumber , s.schoolName from student inner join user on user.id = student.userId
             inner join school s on s.Id = student.SchoolId
             where student.isactive = true and student.IsEnrolled = true and student.IsRegistered = true";

                var countSql = @"
            SELECT COUNT(*)
            from student inner join user on user.id = student.userId
            where student.isactive = true";

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    var searchCondition = @"
                AND (
                    user.LastName LIKE @SearchQuery OR
                    user.FirstName LIKE @SearchQuery OR
                    StudentIdNumber LIKE @SearchQuery
                )";

                    baseSql += searchCondition;
                    countSql += searchCondition;
                }

                var paginationSql = "";
                if (pageSize > 0) // Add this check to prevent fetching 0 rows
                {
                    paginationSql = @"
                ORDER BY student.Id
                LIMIT @PageSize OFFSET @Offset"; // Use LIMIT and OFFSET
                }
                var finalSql = baseSql + paginationSql;

                var offset = (pageNumber - 1) * pageSize;
                var students = await _connection.QueryAsync<StudentAdminDto>(finalSql, new
                {
                    SearchQuery = $"%{searchQuery}%",
                    Offset = offset,
                    PageSize = pageSize,
                });

                var totalCount = await _connection.ExecuteScalarAsync<int>(countSql, new
                {
                    SearchQuery = $"%{searchQuery}%",
                });

                callResult.IsSuccess = true;
                callResult.Data = students.ToList();
                callResult.TotalCount = totalCount;  // Add total count to the result
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Data = null;
                callResult.Message = "Fetching student list failed.";
            }

            return callResult;
        }

        public async Task<CallResultDto<object>> DenyStudent(int studentId, CancellationToken cancellationToken) 
        {
            var callResult = new CallResultDto<object>();
            try
            {
                var student = await _payrollcontext.Students.FirstOrDefaultAsync(x => x.Id == studentId);
                var studentUser = await _payrollcontext.Users.FirstOrDefaultAsync(x => x.Id == student.UserId);
                if (student != null)
                {
                    var emailSubject = "School Registration";
                    var emailBody = $@"
                <p>Dear {studentUser.FirstName} {studentUser.LastName},</p>
                <p>Your Admission to the School is denied for some reasons. For more information please contact the school administrator.</p>
                <p>Best regards,</p>
                <p>School Administration</p>";

                    await _emailService.SendEmailAsync(studentUser.Email, emailSubject, emailBody, cancellationToken);

                    _payrollcontext.Students.Remove(student);
                    await _payrollcontext.SaveChangesAsync(cancellationToken);
                    _payrollcontext.Users.Remove(studentUser);
                    await _payrollcontext.SaveChangesAsync(cancellationToken);


                    callResult.IsSuccess = true;
                    callResult.Message = "Denied student Successfully";
                }
            }
            catch (Exception ex) 
            {
                callResult.IsSuccess = false;
                callResult.Message = "Student Deny Failed.";
            }
            return callResult;
        }


        public async Task<CallResultDto<StudentForEnroll>> GetStudentProfile(int userId, int syId, CancellationToken ct)
        {
            var callResult = new CallResultDto<StudentForEnroll>();
            var student = await _payrollcontext.Students.FirstOrDefaultAsync(x => x.UserId == userId);
            if (student == null)
            {
                callResult.IsSuccess = false;
                callResult.Message = "Student not found";
                return callResult;
            }

            var enrollmentCurrent = await _payrollcontext.EnrollStudents.FirstOrDefaultAsync(x => x.StudentId == student.Id && x.SchoolYearId == syId);
            if (enrollmentCurrent == null)
            {
                callResult.IsSuccess = false;
                callResult.Message = "Enrollment not found for the given school year";
                return callResult;
            }

            var schoolId = enrollmentCurrent.SchoolId;

            try
            {
                var baseSql = @"
            SELECT 
                s.Id, 
                u.LastName, 
                u.MiddleName, 
                u.FirstName, 
                u.Email, 
                s.ContactNumber, 
                s.LRN, 
                s.StudentIdNumber, 
                s.GradeLevelId,
                g.Level AS GradeLevel, 
                st.strandName,
                s.BirthDate,
                s.Age,
                s.BirthPlace,
                s.CivilStatus,
                s.Religion,
                s.MotherName,
                s.MotherAddress,
                s.FatherName,
                s.FatherAddress,
                s.GuardianName,
                s.GuardianAddress,
                s.LastSchoolAttended,
                s.LastSchoolAttendedYear,
                s.MotherContactNumber,
                s.FatherContactNumber,
                s.GuardianContactNumber,
                s.MotherEmailAddress,
                s.FatherEmailAddress,
                s.GuardianEmailAddress,
                u.Sex,
                s.StudentAddress,
                ssGradeLevel.Level AS GradeLevelForSy
            FROM Student s
            INNER JOIN User u ON s.UserId = u.Id
            INNER JOIN GradeLevel g ON s.GradeLevelId = g.Id
            LEFT JOIN Strand st ON st.Id = s.StrandId
            INNER JOIN EnrollStudent es ON s.Id = es.StudentId
            INNER JOIN SchoolSection ss ON ss.id = es.sectionId
            INNER JOIN GradeLevel ssGradeLevel ON ss.gradeLevelId = ssGradeLevel.Id
            WHERE s.IsActive = true 
              AND s.SchoolId = @schoolId 
              AND s.IsRegistered = true  
              AND s.IsEnrolled = true
              AND es.SchoolYearId = @syId
              AND s.Id = @studentId";

                var studentProfile = await _connection.QueryFirstOrDefaultAsync<StudentForEnroll>(baseSql, new
                {
                    schoolId,
                    syId,
                    studentId = student.Id
                });

                if (studentProfile != null)
                {
                    callResult.IsSuccess = true;
                    callResult.Data = studentProfile;
                }
                else
                {
                    callResult.IsSuccess = false;
                    callResult.Message = "Student profile not found";
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Data = null;
                callResult.Message = $"Fetching student profile failed: {ex.Message}";
            }

            return callResult;
        }
        public async Task<CallResultDto<object>> AddNotes(int studentId, string notes, CancellationToken ct)
        {
            var callResult = new CallResultDto<object>();
            try
            {
                var student = await _payrollcontext.Students.FirstOrDefaultAsync(x => x.Id == studentId);
                if (student != null)
                {
                    student.Notes = notes;
                    await _payrollcontext.SaveChangesAsync(ct);
                }
                callResult.IsSuccess = true;
                callResult.Message = "Added notes to student successfully.";
            }
            catch (Exception ex)
            {
                callResult.IsSuccess = false;
                callResult.Message = "Failed to add notes on student.";
            }

            return callResult;
        }

        public async Task<CallResultDto<string>> GetStudentNotes(int studentId, CancellationToken ct)
        {
            var callResult = new CallResultDto<string>();
            try
            {
                var sql = @"
                SELECT notes from student where id = @studentId";

                var notes = await _connection.QuerySingleOrDefaultAsync<string>(sql, new
                {
                    studentId
                });

                if (notes != null)
                {
                    callResult.IsSuccess = true;
                    callResult.Data = notes;
                }
                else
                {
                    callResult.IsSuccess = false;
                    callResult.Message = "Student not found.";
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                callResult.IsSuccess = false;
                callResult.Data = null;
                callResult.Message = "Fetching student details failed.";
            }

            return callResult;
        }
        public async Task<CallResultDto<List<ConsolidatedReports>>> GetSchoolConsolidatedReport(int syId, CancellationToken ct)
        {
            var callResult = new CallResultDto<List<ConsolidatedReports>>(); // Correctly initialize the callResult with List<Models.School>

            try
            {
                var sql = @"
                    SELECT 
                        s.schoolName,
                        SUM(CASE WHEN ss.gradeLevelId = 1 THEN 1 ELSE 0 END) AS Nursery,
                        SUM(CASE WHEN ss.gradeLevelId = 2 THEN 1 ELSE 0 END) AS Kinder,
                        SUM(CASE WHEN ss.gradeLevelId = 3 THEN 1 ELSE 0 END) AS Kinder2,
                        SUM(CASE WHEN ss.gradeLevelId = 4 THEN 1 ELSE 0 END) AS Grade1,
                        SUM(CASE WHEN ss.gradeLevelId = 5 THEN 1 ELSE 0 END) AS Grade2,
                        SUM(CASE WHEN ss.gradeLevelId = 6 THEN 1 ELSE 0 END) AS Grade3,
                        SUM(CASE WHEN ss.gradeLevelId = 7 THEN 1 ELSE 0 END) AS Grade4,
                        SUM(CASE WHEN ss.gradeLevelId = 8 THEN 1 ELSE 0 END) AS Grade5,
                        SUM(CASE WHEN ss.gradeLevelId = 9 THEN 1 ELSE 0 END) AS Grade6,
                        SUM(CASE WHEN ss.gradeLevelId = 10 THEN 1 ELSE 0 END) AS Grade7,
                        SUM(CASE WHEN ss.gradeLevelId = 11 THEN 1 ELSE 0 END) AS Grade8,
                        SUM(CASE WHEN ss.gradeLevelId = 12 THEN 1 ELSE 0 END) AS Grade9,
                        SUM(CASE WHEN ss.gradeLevelId = 13 THEN 1 ELSE 0 END) AS Grade10,
                        SUM(CASE WHEN ss.gradeLevelId = 14 THEN 1 ELSE 0 END) AS Grade11,
                        SUM(CASE WHEN ss.gradeLevelId = 15 THEN 1 ELSE 0 END) AS Grade12
                    FROM 
                        school s
                    LEFT JOIN 
                        enrollstudent e ON s.id = e.schoolId AND e.schoolyearId = @syId
                    LEFT JOIN 
                        schoolsection ss ON ss.id = e.sectionId
                    GROUP BY 
                        s.schoolName
                    ORDER BY 
                        s.schoolName;
                    ";

                var schools = await _connection.QueryAsync<ConsolidatedReports>(sql, new { syId});

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