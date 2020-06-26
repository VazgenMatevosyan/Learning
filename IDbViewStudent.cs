using DataAccessLayer.Models;
using System.Collections.Generic;

namespace DataAccessLayer
{
    public interface IDbViewStudent
    {
        List<StudentGrades> ShowStudentGrades(string ssnStudent);
        List<Course> FillComboBox(string SSN);
        int Enrol(string SSN, string course);
        int Unenrol(string SSN, string course);
        int UpdateGrade(string studentSsn, string courseName, double grade);
        List<AllStudentsInSingleCourse> AllStudentsInCourse(string courseName);
        StudentGrades OneStudentGradeOneCourse(string studentSsn, string courseName);
    }
}
