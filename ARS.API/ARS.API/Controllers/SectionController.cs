using ARS.API.Models;
using ARS.API.Services.School;
using ARS.API.Services.Section;
using Microsoft.AspNetCore.Mvc;

namespace ARS.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class SectionController(ISectionService sectionService): Controller
    {
        private readonly ISectionService _sectionService = sectionService;

        [HttpGet]
        public async Task<IActionResult> GetSectionList(string? searchQuery, int pageNumber, int pageSize, int userId, CancellationToken ct)
        {
            var sectionList = await _sectionService.GetSectionList(searchQuery, pageNumber, pageSize, userId, ct);
            return Ok(sectionList);
        }

        [HttpGet]
        public async Task<IActionResult> GetSchoolGradeLevels( int userId, CancellationToken ct)
        {
            var sectionList = await _sectionService.GetSchoolGradeLevels( userId, ct);
            return Ok(sectionList);
        }

        [HttpGet]
        public async Task<IActionResult> GetGradeLevels(CancellationToken ct)
        {
            var gradelist = await _sectionService.GetGradeLevels(ct);
            return Ok(gradelist);
        }

        [HttpGet]
        public async Task<IActionResult> GetStrands(CancellationToken ct)
        {
            var strandList = await _sectionService.GetStrands(ct);
            return Ok(strandList);
        }

        [HttpGet]
        public async Task<IActionResult> GetSchoolSectionbyGrade(int id, int? strandId, CancellationToken ct)
        {
            var strandList = await _sectionService.GetSchoolSectionbyGrade(id, strandId, ct);
            return Ok(strandList);
        }

        [HttpPost]
        public async Task<IActionResult> AddSection(SchoolSection section, int userId, CancellationToken ct)
        {
            var response = await _sectionService.AddSection(section, userId, ct);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetSectionById(int id, CancellationToken ct)
        {
            var section = await _sectionService.GetSectionById(id, ct);
            return Ok(section);
        }

        [HttpPut]
        public async Task<IActionResult> EditSection(SchoolSection section, CancellationToken ct)
        {
            var response = await _sectionService.EditSection(section, ct);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetTotalBySection(int userId, int syId, CancellationToken ct)
        {
            var section = await _sectionService.GetTotalBySection(userId, syId, ct);
            return Ok(section);
        }

        [HttpGet]
        public async Task<IActionResult> GetTotalBySchool(int userId, int syId, CancellationToken ct)
        {
            var section = await _sectionService.GetTotalBySchool(userId, syId, ct);
            return Ok(section);
        }

        [HttpGet]
        public async Task<IActionResult> GetTotalByLevel(int userId, int syId, CancellationToken ct)
        {
            var section = await _sectionService.GetTotalByLevel(userId, syId, ct);
            return Ok(section);
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentMasterList(int userId, int syId, int gradeLevelId,  int strandId, string? searchString, CancellationToken ct)
        {
            var moveUpstatus = await _sectionService.GetStudentMasterList(userId, syId, gradeLevelId, strandId, searchString, ct);
            return Ok(moveUpstatus);
        }
    }
}
