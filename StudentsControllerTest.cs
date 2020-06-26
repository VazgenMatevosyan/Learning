using API;
using API.Controllers;
using API.Models;
using AutoMapper;
using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TestAPI.ExpectedResults;
using Xunit;

namespace TestAPI
{
    public class StudentsControllerTest
    {
        private readonly StudentsController studentsController;
        public StudentsControllerTest()
        {
            DbStudents students = new DbStudents();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperMapings>();
                cfg.CreateMap<Student, StudentAPI>();
            });
            var mapper = config.CreateMapper();
            studentsController = new StudentsController(students, mapper);
        }
        [Fact]
        public void TestGetStudents()
        {
            ExpectedStudents expectedStudents = new ExpectedStudents();
            List<Student> expected = expectedStudents.GetStudents();
            ObjectResult result = (ObjectResult)studentsController.Get().Result;
            int? statusCode = result.StatusCode;
            Assert.Equal(200, statusCode);
            List<Student> actual = (List<Student>)result.Value;            
            Assert.Equal(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; ++i)
            {
                Assert.Equal(expected[i].SSN, actual[i].SSN);
            }
        }
        [Fact]
        public void TestGetStudent()
        {
            ExpectedStudents expectedStudents = new ExpectedStudents();
            List<Student> expected = expectedStudents.GetStudents();
            string expectedSsn = expected[0].SSN;
            string expectedName = expected[0].name;
            string expectedSurname = expected[0].surname;
            ObjectResult result = (ObjectResult)studentsController.GetStudent(expectedSsn).Result;
            int? statusCode = result.StatusCode;
            Assert.Equal(200, statusCode);
            StudentAPI student =(StudentAPI) result.Value;
            Assert.True(student.name == expectedName && student.surname == expectedSurname);
        }
    }
}
