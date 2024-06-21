using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityApi.Data;
using UniversityApp.Core.Entites;
using UniversityApp.Service.Dtos;
using UniversityApp.Service.Dtos.StudentDtos;
using UniversityApp.Service.Interfaces;

namespace UniversityApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly UniversityDbContext _context;
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost("")]
        public ActionResult Create([FromForm] StudentCreateDto createDto)
        {
            return StatusCode(201, new { Id = _studentService.Create(createDto) });
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            return Ok(_studentService.GetById(id));
        }

        [HttpGet("")]
        public ActionResult<PaginatedList<StudentGetDto>> GetAll(int page=1,int size = 10)
        {
            return Ok(_studentService.GetAllPaginated(page, size));
        }


    }
}
