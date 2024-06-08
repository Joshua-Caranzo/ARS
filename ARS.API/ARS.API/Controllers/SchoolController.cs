using ARS.API.Models;
using ARS.API.Services.School;
using Microsoft.AspNetCore.Mvc;
using Payroll.API.Services.User.Dto;

namespace ARS.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class SchoolController(ISchoolService schoolService) : ControllerBase
    {
        private readonly ISchoolService _schoolService = schoolService;

        [HttpGet]
        public async Task<IActionResult> GetSchoolList(string? searchQuery, int pageNumber, int pageSize,CancellationToken ct)
        {
            var schoolList = await _schoolService.GetSchoolList(searchQuery,  pageNumber,  pageSize, ct);
            return Ok(schoolList);
        }

        [HttpPost]
        public async Task<IActionResult> AddSchool(School schools, CancellationToken ct)
        {
            var response = await _schoolService.AddSchool(schools, ct);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetSchoolById(int id, CancellationToken ct)
        {
            var school = await _schoolService.GetSchoolById(id,ct);
            return Ok(school);
        }

        [HttpPost]
        public async Task<IActionResult> EditSchool([FromForm] School school, [FromForm] IFormFile? file, CancellationToken ct)
        {
            var response = await _schoolService.EditSchool(school, file, ct);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetSchoolByUserId(int id, CancellationToken ct)
        {
            var school = await _schoolService.GetSchoolByUserId(id, ct);
            return Ok(school);
        }
    }
}
