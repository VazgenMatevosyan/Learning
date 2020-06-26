using DataAccessLayer.Models;
using System.Collections.Generic;

namespace DataAccessLayer
{
    public interface IDbTeachers
    {
        List<Teacher> GetTeachers();
        int Insert(string name, string surname, string ssn, string course);
        int Delete(string name, string surname, string ssn, string course);
        Teacher GetOneTeacher(string teacherSsn);
    }
}
