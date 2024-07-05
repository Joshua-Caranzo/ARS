using Payroll.API.SharedDto;

namespace ARS.API.Services.School
{
    public interface ISchoolService
    {
        Task<CallResultDto<List<Models.School>>> GetSchoolList(string? searchQuery, int pageNumber, int pageSize, CancellationToken ct);
        Task<CallResultDto<object>> AddSchool(Models.School school, CancellationToken ct);
        Task<CallResultDto<Models.School>> GetSchoolById(int id, CancellationToken ct);
        Task<CallResultDto<object>> EditSchool(Models.School school, IFormFile? file, CancellationToken ct);
        Task<CallResultDto<Models.School>> GetSchoolByUserId(int id, CancellationToken ct);
        Task<CallResultDto<List<Models.School>>> GetAllSchools(CancellationToken ct);
    }
}
