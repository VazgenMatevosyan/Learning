using API.Controllers;
using DataAccessLayer;
using Xunit;
using TestAPI.ExpectedResults;
using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace TestAPI
{
    public class CoursesControllerTest
    {
        [Fact]
        public void TestGetCourses()
        {
            DbCourses courses = new DbCourses();
            CoursesController coursesController = new CoursesController(courses);
            ExpectedCourses expectedCourses=new ExpectedCourses();
            List<Course> expected = expectedCourses.GetCourses();
            ObjectResult result = (ObjectResult)coursesController.Get().Result;
            int? statusCode = result.StatusCode;
            Assert.Equal(200, statusCode);
            List<Course> actual = (List<Course>)result.Value;
            Assert.Equal(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; ++i)
            {
                Assert.Equal(expected[i].title, actual[i].title);
            }
        }
    }
}
