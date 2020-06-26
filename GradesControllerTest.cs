using API.Controllers;
using DataAccessLayer;
using Xunit;
using TestAPI.ExpectedResults;
using DataAccessLayer.Models;
using System.Collections.Generic;
using AutoMapper;
using API;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace TestAPI
{
    public class GradesControllerTest
    {
        private readonly GradesController gradesController;
        public GradesControllerTest()
        {
            DbViewStudent viewStudent = new DbViewStudent();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperMapings>();
                cfg.CreateMap<StudentGrades, StudentGradesAPI>();
            });
            var mapper = config.CreateMapper();
            gradesController = new GradesController(viewStudent, mapper);
        }
        [Fact]
        public void TestGetStudentGrades()
        {
            ExpectedStudents expectedStudents = new ExpectedStudents();
            string studentSsn = expectedStudents.GetStudents()[0].SSN;
            ExpectedGrades expectedGrades = new ExpectedGrades();
            List<StudentGrades> expected = expectedGrades.ShowStudentGrades(studentSsn);       
            ObjectResult result = (ObjectResult) gradesController.GetStudentGrades(studentSsn).Result;
            int? statusCode = result.StatusCode;
            Assert.Equal(200, statusCode);
            List<StudentGrades> actual = (List<StudentGrades>)result.Value;
            Assert.Equal(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; ++i)
            {
                Assert.True(expected[i].course==actual[i].course && expected[i].grade==actual[i].grade);
            }
        }
        [Fact]
        public void TestGetStudentCourseGrade()
        {
            DbCourses dbCourses = new DbCourses();
            dbCourses.Insert("Test Course", "Test Course");
            DbStudents dbStudents = new DbStudents();
            dbStudents.Insert("Test Student", "Test Student", "Test Student");
            DbViewStudent dbViewStudent = new DbViewStudent();
            dbViewStudent.Enrol("Test Student", "Test Course");
            dbViewStudent.UpdateGrade("Test Student", "Test Course", 20);
            ExpectedGrades expectedGrades = new ExpectedGrades();
            ObjectResult result = (ObjectResult)gradesController.GetStudentCourseGrade("Test Student", "Test Course").Result;
            int? statusCode = result.StatusCode;
            Assert.Equal(200, statusCode);
            StudentGradesAPI actual = (StudentGradesAPI)result.Value;
            double? expectedgrade = expectedGrades.GetGrade("Test Student", "Test Course").grade;
            Assert.Equal(expectedgrade, actual.grade);
            dbViewStudent.Unenrol("Test Student", "Test Course");
            dbCourses.Delete("Test Course");
            dbStudents.Delete("Test Student", "Test Student", "Test Student");
        }
        [Fact]
        public void TestUpdateGrade()
        {
            DbCourses dbCourses = new DbCourses();
            dbCourses.Insert("Test Course", "Test Course");
            DbStudents dbStudents = new DbStudents();
            dbStudents.Insert("Test Student", "Test Student", "Test Student");
            DbViewStudent dbViewStudent = new DbViewStudent();
            dbViewStudent.Enrol("Test Student", "Test Course");
            double actual = 5;
            gradesController.Post("Test Student", "Test Course", actual);
            ExpectedGrades expectedGrades = new ExpectedGrades();
            double? expected = expectedGrades.GetGrade("Test Student", "Test Course").grade;
            Assert.Equal(expected, actual);
            dbViewStudent.Unenrol("Test Student", "Test Course");
            dbCourses.Delete("Test Course");
            dbStudents.Delete("Test Student", "Test Student", "Test Student");
        }
        [Fact]
        public void TestGetAllStudentsGrades()
        {
            DbCourses dbCourses = new DbCourses();
            dbCourses.Insert("Test Course", "Test Course");
            ObjectResult result = (ObjectResult)gradesController.GetAllStudentsGrades("Test Course").Result;       
            int? statusCode = result.StatusCode;
            Assert.Equal(200, statusCode);
            if (result.Value.GetType().Name == "String")
            {
                dbCourses.Delete("Test Course");
                Assert.True(1 == 1, "That Course");
            }
            else
            {
                List<AllStudentsInSingleCourse> actual = (List<AllStudentsInSingleCourse>)result.Value;
                ExpectedGrades expectedGrades = new ExpectedGrades();
                List<AllStudentsInSingleCourse> expected = expectedGrades.AllStudentsInCourse("Test Course");
                Assert.Equal(expected.Count, actual.Count);
                for (int i = 0; i < expected.Count; ++i)
                {
                    Assert.Equal(expected[i].grade, actual[i].grade);
                }
                dbCourses.Delete("Test Course");
            }
        }
    }
}