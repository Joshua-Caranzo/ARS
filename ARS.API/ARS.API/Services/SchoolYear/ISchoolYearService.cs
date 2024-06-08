using Payroll.API.SharedDto;

namespace ARS.API.Services.SchoolYear
{
    public interface ISchoolYearService
    {
        Task<CallResultDto<List<Models.SchoolYear>>> GetSchoolYear(string? searchQuery, int pageNumber, int pageSize, CancellationToken ct);
        Task<CallResultDto<Models.SchoolYear>> GetSchoolYearById(int id, CancellationToken ct);
        Task<CallResultDto<object>> AddSchoolYear(Models.SchoolYear schoolyear, CancellationToken ct);
        Task<CallResultDto<object>> EditSchoolYear(Models.SchoolYear schoolyear, CancellationToken ct);
        Task<CallResultDto<List<Models.SchoolYear>>> GetSchoolYearActive(CancellationToken ct);
        Task<CallResultDto<int>> GetCurrentSchoolTerm(CancellationToken ct);
    }
}
