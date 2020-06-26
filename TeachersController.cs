using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DataAccessLayer;
using DataAccessLayer.Models;
using AutoMapper;
using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly IDbTeachers teachers;
        private readonly IMapper mapper;
        public TeachersController(IDbTeachers teachers,IMapper mapper)
        {
            this.teachers = teachers;
            this.mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Teacher>> Get()
        {
            try
            {
                return Ok(teachers.GetTeachers());
            }
            catch
            {
                return StatusCode(500, "Server Error.");
            }
        }
        [HttpGet("{teacherSsn}")]
        public ActionResult<TeacherAPI> GetTeacher(string teacherSsn)
        {
            Teacher getTeacher = teachers.GetOneTeacher(teacherSsn);
            try
            {
                if (getTeacher.SSN=="---")
                {
                    return StatusCode(400, System.String.Format("There isn't exist {0} SSN teacher.", teacherSsn));
                }
                else
                {
                    return Ok(getTeacher);
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