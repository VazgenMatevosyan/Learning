using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DataAccessLayer.Models;
using DataAccessLayer;
using API.Models;
using AutoMapper;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IDbStudents students;
        private readonly IMapper mapper;
        public StudentsController(IDbStudents students,IMapper mapper)
        {
            this.students = students;
            this.mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Student>> Get()
        {
            try
            {
                return Ok(students.GetStudents());
            }
            catch
            {
                return StatusCode(500, "Server Error.");
            }
        }
        [HttpGet("{studentSsn}")]
        public ActionResult<StudentAPI> GetStudent(string studentSsn)
        {
            Student getStudent = students.GetOneStudent(studentSsn);
            try
            {
                if (getStudent.SSN=="---")
                {
                    return StatusCode(400, System.String.Format("There isn't exist {0} SSN student.", studentSsn));
                }
                else
                {
                    return Ok(mapper.Map<StudentAPI>(getStudent));                    
                }
            }
            catch
            {
                return StatusCode(500, "Server Error.");
            }
        }
        [HttpOptions]
        public void Options()
        {
            HttpContext.Response.Headers.Add("Allow", "Get");
        }
    }
}