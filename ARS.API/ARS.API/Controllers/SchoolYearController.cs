using ARS.API.Models;
using ARS.API.Services.SchoolYear;
using Microsoft.AspNetCore.Mvc;

namespace ARS.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class SchoolYearController(ISchoolYearService schoolYearService) : ControllerBase
    {
        private readonly ISchoolYearService _schoolYearService = schoolYearService;

        [HttpGet]
        public async Task<IActionResult> GetSchoolYearList(string? searchQuery, int pageNumber, int pageSize, CancellationToken ct)
        {
            var schoolYears = await _schoolYearService.GetSchoolYear(searchQuery, pageNumber, pageSize, ct);
            return Ok(schoolYears);
        }

        [HttpGet]
        public async Task<IActionResult> GetSchoolYearById(int id, CancellationToken ct)
        {
            var schoolYears = await _schoolYearService.GetSchoolYearById(id, ct);
            return Ok(schoolYears);
        }

        [HttpGet]
        public async Task<IActionResult> GetSchoolYearActive( CancellationToken ct)
        {
            var schoolYears = await _schoolYearService.GetSchoolYearActive( ct);
            return Ok(schoolYears);
        }
            [HttpPost]
        public async Task<IActionResult>AddschoolYear(SchoolYear schoolYear, CancellationToken cancellationToken) 
        {
            var result = await _schoolYearService.AddSchoolYear(schoolYear, cancellationToken);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> EditSchoolYear(SchoolYear schoolYear, CancellationToken cancellationToken)
        {
            var result = await _schoolYearService.EditSchoolYear(schoolYear, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentSchoolTerm( CancellationToken ct)
        {
            var schoolYears = await _schoolYearService.GetCurrentSchoolTerm( ct);
            return Ok(schoolYears);
        }
    }
}
