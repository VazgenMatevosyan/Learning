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
    public class TeachersControllerTest
    {
        private readonly TeachersController teachersController;
        public TeachersControllerTest()
        {
            DbTeachers teachers = new DbTeachers();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperMapings>();
                cfg.CreateMap<Teacher, TeacherAPI>();
            });
            var mapper = config.CreateMapper();
            teachersController = new TeachersController(teachers, mapper);
        }
        [Fact]
        public void TestGetTeachers()
        {
            ExpectedTeachers expectedTeachers = new ExpectedTeachers();
            List<Teacher> expected = expectedTeachers.GetTeachers();
            ObjectResult result = (ObjectResult)teachersController.Get().Result;
            int? statusCode = result.StatusCode;
            Assert.Equal(200, statusCode);
            List<Teacher> actual = (List<Teacher>)result.Value;            
            Assert.Equal(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; ++i)
            {
                Assert.Equal(expected[i].SSN, actual[i].SSN);
            }
        }
        [Fact]
        public void TestGetTeacher()
        {
            ExpectedTeachers expectedTeachers = new ExpectedTeachers();
            List<Teacher> expected = expectedTeachers.GetTeachers();
            string expectedSsn = expected[0].SSN;
            string expectedName = expected[0].name;
            string expectedSurname = expected[0].surname;
            ObjectResult result = (ObjectResult)teachersController.GetTeacher(expectedSsn).Result;
            int? statusCode = result.StatusCode;
            Assert.Equal(200, statusCode);
            Teacher teacher = (Teacher)result.Value;
            Assert.True(teacher.name == expectedName && teacher.surname == expectedSurname);
        }
    }
}
