using System.Collections.Generic;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using DataAccessLayer;
namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IDbCourses courses;
        public CoursesController(IDbCourses courses)
        {
            this.courses = courses;
        }
        [HttpGet]
        public  ActionResult<IEnumerable<Course>> Get()
        {
            try
            {
                return Ok(courses.GetCourses());
            }
            catch
            {
                return StatusCode(500,"Server Error.");
            }
        }
        [HttpOptions]
        public void Options()
        {
            HttpContext.Response.Headers.Add("Allow", "Get");
        }
    }
}