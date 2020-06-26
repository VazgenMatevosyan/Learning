using API.Models;
using AutoMapper;
using DataAccessLayer.Models;

namespace API
{
    public class AutoMapperMapings:Profile
    {
        public AutoMapperMapings()
        {
            CreateMap<Course, CourseAPI>();
            CreateMap<Student, StudentAPI>();
            CreateMap<Teacher, TeacherAPI>();
            CreateMap<Topic, TopicAPI>();
            CreateMap<StudentGrades, StudentGradesAPI>();
        }
    }
}
