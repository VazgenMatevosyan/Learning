using DataAccessLayer.Models;
using System.Collections.Generic;

namespace DataAccessLayer
{
    public interface IDbCourses
    {
        List<Course> GetCourses();
        bool Insert(string title, string description);
        bool Delete(string title);
        bool IsExistCourse(string course);
    }
}
