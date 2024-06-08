using ARS.API.Services.Student.Dto;
using Payroll.API.SharedDto;

namespace ARS.API.Services.Student
{
    public interface IStudentService
    {
        Task<CallResultDto<object>> AddStudentRegistrar(StudentDto student, int userId,bool? isStudentRegistered, CancellationToken ct);
        Task<CallResultDto<List<StudentDto>>> GetStudentList(string? searchQuery, int pageNumber, int pageSize, int userId, CancellationToken ct);
        Task<CallResultDto<StudentDto>> GetStudentById(int studentId, bool fromStudent, CancellationToken ct);
        Task<CallResultDto<object>> EditStudentRegistrar(StudentDto student, int userId, CancellationToken ct);
        Task<CallResultDto<List<StudentForEnroll>>> GetStudentListForEnrollment(string? searchQuery, int pageNumber, int pageSize, int userId, CancellationToken ct);
        Task<CallResultDto<object>> EnrollStudent(int userId, int studentId, int sectionId, int syId, int? semId, int?strandId, CancellationToken ct);
        Task<CallResultDto<List<StudentForEnroll>>> GetStudentListEnrolled(string? searchQuery, int pageNumber, int pageSize, int userId, int syId, CancellationToken ct);
        Task<CallResultDto<List<EnrollStudentDto>>> GetEnrollmentHistory(string? searchQuery, int pageNumber, int pageSize, int studentId, CancellationToken ct);
        Task<CallResultDto<bool>> GetMoveUpStatus(int studentId, CancellationToken ct);
        Task<CallResultDto<object>> RequestToMoveUp(int studentId, CancellationToken ct);
        Task<CallResultDto<List<StudentReport>>> GetStudentListReport(int userId, int gradeLevelId, int syId, CancellationToken ct);
    }
}
