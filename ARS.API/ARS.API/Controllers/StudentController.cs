using ARS.API.Services.Student;
using ARS.API.Services.Student.Dto;
using Microsoft.AspNetCore.Mvc;
using Payroll.API.Services.User.Dto;

namespace ARS.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class StudentController(IStudentService studentService) : ControllerBase
    {
        private readonly IStudentService _studentService = studentService;

        [HttpPost]
        public async Task<IActionResult> AddStudent(StudentDto student,  int userId, bool? isStudentRegistered, CancellationToken ct)
        {
            var response = await _studentService.AddStudentRegistrar(student, userId, isStudentRegistered,ct);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentList(string? searchQuery, int pageNumber, int pageSize, int userId, CancellationToken ct)
        {
            var studentList = await _studentService.GetStudentList(searchQuery, pageNumber, pageSize, userId, ct);
            return Ok(studentList);
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentById(int userId, bool fromStudent, CancellationToken ct)
        {
            var student = await _studentService.GetStudentById(userId, fromStudent, ct);
            return Ok(student);
        }

        [HttpPut]
        public async Task<IActionResult> EditStudent(StudentDto student, int userId, CancellationToken ct)
        {
            var response = await _studentService.EditStudentRegistrar(student, userId, ct);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentListForEnrollment(string? searchQuery, int pageNumber, int pageSize, int userId, CancellationToken ct)
        {
            var studentList = await _studentService.GetStudentListForEnrollment(searchQuery, pageNumber, pageSize, userId, ct);
            return Ok(studentList);
        }

        [HttpPost]
        public async Task<IActionResult> EnrollStudent(int userId, int studentId, int sectionId, int syId, int? semId, int? strandId, CancellationToken ct)
        {
            var response = await _studentService.EnrollStudent(userId, studentId, sectionId, syId, semId, strandId, ct);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentListEnrolled(string? searchQuery, int pageNumber, int pageSize, int userId, int syId, CancellationToken ct)
        {
            var studentList = await _studentService.GetStudentListEnrolled(searchQuery, pageNumber, pageSize, userId, syId, ct);
            return Ok(studentList);
        }

        [HttpGet]
        public async Task<IActionResult> GetEnrollmentHistory(string? searchQuery, int pageNumber, int pageSize, int studentId, CancellationToken ct)
        {
            var studentList = await _studentService.GetEnrollmentHistory(searchQuery, pageNumber, pageSize, studentId, ct);
            return Ok(studentList);
        }

        [HttpGet]
        public async Task<IActionResult> GetMoveUpStatus( int studentId, CancellationToken ct)
        {
            var moveUpstatus = await _studentService.GetMoveUpStatus(studentId, ct);
            return Ok(moveUpstatus);
        }

        [HttpPut]
        public async Task<IActionResult> RequestToMoveUp(int studentId, CancellationToken ct)
        {
            await _studentService.RequestToMoveUp(studentId, ct);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentReport(int userId, int gradeLevelId, int syId, CancellationToken ct)
        {
            var moveUpstatus = await _studentService.GetStudentListReport(userId, gradeLevelId, syId, ct);
            return Ok(moveUpstatus);
        }
    }
}
