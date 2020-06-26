using System.Collections.Generic;
using System.Linq;
using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using API.Models;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        private readonly IDbViewStudent viewStudent;
        private readonly IMapper mapper;
        public GradesController(IDbViewStudent viewStudent, IMapper mapper)
        {
            this.viewStudent = viewStudent;
            this.mapper = mapper;
        }
        [HttpGet("Student/{studentSsn}")]
        public ActionResult<IEnumerable<StudentGrades>> GetStudentGrades([Required] string studentSsn)
        {
            List<StudentGrades> studentGrades = viewStudent.ShowStudentGrades(studentSsn);
            try
            {
                if (studentGrades.Count != 0)
                {
                    return Ok(viewStudent.ShowStudentGrades(studentSsn));
                }
                else
                {
                    return StatusCode(400, "That student isn't enroled any course or that student isn't exist.");
                }
            }
            catch
            {
                return StatusCode(500, "Server Error.");
            }
        }
        [HttpGet("Student/{studentSsn}/Course/{courseName}")]
        public ActionResult<StudentGradesAPI> GetStudentCourseGrade([Required] string studentSsn,[Required] string courseName)
        {
            StudentGrades getStudentCourseGrade = viewStudent.OneStudentGradeOneCourse(studentSsn,courseName);
            try
            {
                if (getStudentCourseGrade.grade == -100)
                {
                    return StatusCode(400, System.String.Format("{0} SSN Student didn't enrol {1} course or {0} SSN student isn't exist or {1} isn't exist.", studentSsn, courseName));
                }
                else
                {
                    return Ok(mapper.Map<StudentGradesAPI>(getStudentCourseGrade));
                }
            }
            catch
            {
                return StatusCode(500, "Server Error.");
            }
        }
        [HttpPost("Student/{studentSsn}/Course/{courseName}/Grade/{grade}")]
        public ActionResult Post( string studentSsn, string courseName, double grade)
        {
            int result = viewStudent.UpdateGrade(studentSsn, courseName, grade);
            try
            {
                switch (result)
                {
                    case 1:
                        return StatusCode(400, System.String.Format("{0} SSN student doesn't exist.", studentSsn));
                    case 2:
                        return StatusCode(400, System.String.Format("{0} course doesn't exist.", courseName));
                    case 3:
                        return StatusCode(400, System.String.Format("{0} SSN student didn't enrol {1} course.", studentSsn, courseName));
                    case 4:
                        return StatusCode(200, System.String.Format("{0} SSN student's grade for {1} course set {2}.", studentSsn, courseName, grade));
                    default:
                        return Ok();
                }
            }
            catch
            {
                return StatusCode(500, "Server Error.");
            }
        }
        [HttpGet("Course/{course}")]
        public ActionResult<List<AllStudentsInSingleCourse>> GetAllStudentsGrades([Required] string course)
        {
            IEnumerable<AllStudentsInSingleCourse> result = viewStudent.AllStudentsInCourse(course);
            try
            {
                DbCourses temp = new DbCourses();
                bool existCourse = temp.IsExistCourse(course);
                if (!existCourse)
                {
                    return StatusCode(400, System.String.Format("{0} course isn't exist.", course));
                }
                if (result.Count() == 0)
                {
                    return StatusCode(200, System.String.Format("{0} course doesn't have any student.",course));
                }
                return Ok(viewStudent.AllStudentsInCourse(course));
            }
            catch
            {
                return StatusCode(500, "Server Error.");
            }
        }
        [HttpOptions]
        public void Options()
        {
            HttpContext.Response.Headers.Add("Allows", "Get, Post");
        }
    }
}