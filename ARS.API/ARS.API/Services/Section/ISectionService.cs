using ARS.API.Models;
using ARS.API.Services.Section.Dto;
using Payroll.API.SharedDto;

namespace ARS.API.Services.Section
{
    public interface ISectionService
    {
        Task<CallResultDto<List<SchoolSectionDto>>> GetSectionList(string? searchQuery, int pageNumber, int pageSize, int userId, CancellationToken ct);
        Task<CallResultDto<List<StrandDto>>> GetStrands(CancellationToken ct);
        Task<CallResultDto<List<GradeLevel>>> GetGradeLevels(CancellationToken ct);
        Task<CallResultDto<object>> AddSection(Models.SchoolSection section, int userId, CancellationToken ct);
        Task<CallResultDto<Models.SchoolSection>> GetSectionById(int id, CancellationToken ct);
        Task<CallResultDto<object>> EditSection(SchoolSection section, CancellationToken ct);
        Task<CallResultDto<List<SchoolSection>>> GetSchoolSectionbyGrade(int userId, int id, int? strandId, CancellationToken ct);
        Task<CallResultDto<List<SchoolSectionDto>>> GetSchoolGradeLevels(int SchoolId, CancellationToken ct);
        Task<CallResultDto<List<StudentBySection>>> GetTotalBySection(int userId, int syId, CancellationToken ct);
        Task<CallResultDto<StudentCount>> GetTotalBySchool(int userId, int syId, CancellationToken ct);
        Task<CallResultDto<List<StudentByLevel>>> GetTotalByLevel(int userId, int syId, CancellationToken ct);
        Task<CallResultDto<List<StudentMasterlist>>> GetStudentMasterList(int userId, int syId, int gradeLevelId, int? strandId, string? searchString, CancellationToken ct);
        Task<CallResultDto<List<StudentByLevel>>> GetTotalByLevelSuperAdmin(int schoolId, int syId, CancellationToken ct);
        Task<CallResultDto<StudentCount>> GetTotalBySchoolSuperAdmin(int schoolId, int syId, CancellationToken ct);
        Task<CallResultDto<CurrentSection>> GetCurrentSection(int studentId, int syId, CancellationToken ct);
        Task<CallResultDto<object>> ChangeSection(int studentId, int syId, int sectionId, CancellationToken ct);
        Task<CallResultDto<CurrentStrandDto>> GetCurrentStrand(int studentId, int syId, CancellationToken ct);
        Task<CallResultDto<object>> ChangeStrand(int studentId, int sectionId, int strandId, int syId, CancellationToken ct);
    }
}
