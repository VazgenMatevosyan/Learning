using DataAccessLayer;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace TestAPI.ExpectedResults
{
    public class ExpectedGrades
    {
        public List<StudentGrades> ShowStudentGrades(string ssnStudent)
        {
            List<StudentGrades> result = new List<StudentGrades>();
            DbInfo dbInfo = new DbInfo();
            SqlConnection connection = dbInfo.OpenConnection();
            using (connection)
            {
                using (SqlCommand command = new SqlCommand(String.Format("select title_course,grade from Students " +
                   "join CoursesStudentsRelation on Students.id = CoursesStudentsRelation.id_student " +
                   "where SSN = '{0}'", ssnStudent), connection))
                {
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            int course = dataReader.GetOrdinal("title_course");
                            int grade = dataReader.GetOrdinal("grade");
                            while (dataReader.Read())
                            {
                                StudentGrades studentGrades = new StudentGrades();
                                studentGrades.course = dataReader.GetString(course);
                                if (dataReader.IsDBNull(grade))
                                {
                                    studentGrades.grade = null;
                                }
                                else
                                {
                                    studentGrades.grade = dataReader.GetDouble(grade);
                                }
                                result.Add(studentGrades);
                            }
                        }
                    }
                }
            }
            return result;
        }
        public StudentGrades GetGrade(string studentSsn, string courseName)
        {
            StudentGrades studentGrades = new StudentGrades();
            studentGrades.course = courseName;
            DbInfo dbInfo = new DbInfo();
            SqlConnection connection = dbInfo.OpenConnection();
            using (connection)
            {
                using (SqlCommand command = new SqlCommand(String.Format("select id from Students where SSN='{0}'", studentSsn),connection))
                {
                    string studentId = command.ExecuteScalar().ToString();
                    command.CommandText = String.Format("select grade from CoursesStudentsRelation where id_student='{0}' and title_course='{1}'", studentId, courseName);
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        dataReader.Read();
                        if (dataReader.HasRows)
                        {
                            studentGrades.grade = dataReader.GetDouble(dataReader.GetOrdinal("grade"));
                        }
                        else
                        {
                            studentGrades.grade = null;
                        }
                    }
                }
            }
            return studentGrades;
        }
        public List<AllStudentsInSingleCourse> AllStudentsInCourse(string courseName)
        {
            List<AllStudentsInSingleCourse> result = new List<AllStudentsInSingleCourse>();
            DbInfo dbInfo = new DbInfo();
            SqlConnection connection = dbInfo.OpenConnection();
            using (connection)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select name,surname,SSN,grade from Students join " +
                        "CoursesStudentsRelation on Students.id=CoursesStudentsRelation.id_student " +
                        "where title_course='" + courseName + "'";
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            int name = dataReader.GetOrdinal("name");
                            int surname = dataReader.GetOrdinal("surname");
                            int SSN = dataReader.GetOrdinal("SSN");
                            int grade = dataReader.GetOrdinal("grade");
                            while (dataReader.Read())
                            {
                                AllStudentsInSingleCourse temp = new AllStudentsInSingleCourse();
                                temp.name = dataReader.GetString(name);
                                temp.surname = dataReader.GetString(surname);
                                temp.SSN = dataReader.GetString(SSN);
                                temp.grade = dataReader.GetDouble(grade);
                                result.Add(temp);
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
