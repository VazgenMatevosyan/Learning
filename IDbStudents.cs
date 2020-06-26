using DataAccessLayer.Models;
using System.Collections.Generic;

namespace DataAccessLayer
{
    public interface IDbStudents
    {
        List<Student> GetStudents();
        bool Insert(string name, string surname, string ssn);
        bool Delete(string name, string surname, string SSN);
        Student GetOneStudent(string studentSsn);
    }
}
