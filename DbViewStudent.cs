using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DataAccessLayer
{
    public class DbViewStudent:IDbViewStudent
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
        public List<Course> FillComboBox(string SSN)
        {
            List<Course> result = new List<Course>();
            DbInfo dbInfo = new DbInfo();
            SqlConnection connection = dbInfo.OpenConnection();
            using (connection)
            {
                using (SqlCommand command = new SqlCommand("select id from Students where SSN='" + SSN + "'", connection))
                {
                    string idStudent = command.ExecuteScalar().ToString();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select title from Courses where title not in " +
                        "(select title_course from " +
                        "CoursesStudentsRelation where id_student='" + idStudent + "')";
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            int title = dataReader.GetOrdinal("title");
                            while (dataReader.Read())
                            {
                                Course course = new Course();
                                course.title = dataReader.GetString(title);
                                result.Add(course);
                            }
                        }
                    }
                }
            }
            return result;
        }
        public int UpdateGrade(string studentSsn, string courseName, double grade)
        {
            DbInfo dbInfo = new DbInfo();
            SqlConnection connection = dbInfo.OpenConnection();
            using (connection)
            {
                using (SqlCommand command = new SqlCommand("select count(*) from Students where SSN='" + studentSsn + "'", connection))
                {
                    int existStudent = (int)command.ExecuteScalar();
                    if (existStudent == 0)
                    {
                        return 1;
                    }
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select count(*) from Courses where title='" + courseName + "'";
                    int existCourse = (int)command.ExecuteScalar();
                    if (existCourse == 0)
                    {
                        return 2;
                    }
                    command.CommandText= "select id from Students where SSN = '" + studentSsn + "'";
                    string idStudent = command.ExecuteScalar().ToString();
                    command.CommandText = String.Format("update CoursesStudentsRelation " +
                        "set grade = {0} where id_student = '{1}' " +
                        "and title_course = '{2}'", grade, idStudent, courseName);
                    int affected =command.ExecuteNonQuery();
                    if (affected == 0)
                    {
                        return 3;
                    }
                    return 4; 
                }
            }
        }
        public int Enrol(string SSN,string course)
        {
            int result;
            DbInfo dbInfo = new DbInfo();
            SqlConnection connection = dbInfo.OpenConnection();
            using (connection)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select count(*) from Courses where title='" + course + "'";
                    int countCourse = (int)command.ExecuteScalar();
                    if (countCourse == 0)
                    {
                        result = 1;
                    }
                    else
                    {
                        command.CommandText = "select  id from Students where SSN='" + SSN + "'";
                        string idStudent = command.ExecuteScalar().ToString();
                        command.CommandText = "select count(*) from CoursesStudentsRelation where title_course='" + course + "' and id_student='" + idStudent + "'";
                        int countAlreadyEnrol = (int)command.ExecuteScalar();
                        if (countAlreadyEnrol != 0)
                        {
                            result = 2;
                        }
                        else
                        {
                            command.CommandText = "insert into CoursesStudentsRelation([id_student],[title_course]) values('" + idStudent + "','" + course + "')";
                            command.ExecuteNonQuery();
                            result = 3;
                        }
                    }
                }
            }
            return result;
        }
        public int Unenrol(string SSN, string course)
        {
            int result;
            DbInfo dbInfo = new DbInfo();
            SqlConnection connection = dbInfo.OpenConnection();
            using (connection)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select count(*) from Courses where title='" + course + "'";
                    int countCourse = (int)command.ExecuteScalar();
                    if (countCourse == 0)
                    {
                        result = 1;
                    }
                    else
                    {
                        command.CommandText = "select  id from Students where SSN='" + SSN + "'";
                        string idStudent = command.ExecuteScalar().ToString();
                        command.CommandText = "select count(*) from CoursesStudentsRelation where title_course='" + course + "' and id_student='" + idStudent + "'";
                        int countNotEnrol = (int)command.ExecuteScalar();
                        if (countNotEnrol == 0)
                        {
                            result = 2;
                        }
                        else
                        {
                            command.CommandText = "delete from CoursesStudentsRelation where id_student='" + idStudent + "' and title_course='" + course + "'";
                            command.ExecuteNonQuery();
                            result = 3;
                        }
                    }
                }
            }
            return result;
        }
        public List<AllStudentsInSingleCourse> AllStudentsInCourse(string courseName)
        {
            List<AllStudentsInSingleCourse> result=new List<AllStudentsInSingleCourse>();
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
                            int surname= dataReader.GetOrdinal("surname");
                            int SSN = dataReader.GetOrdinal("SSN");
                            int grade = dataReader.GetOrdinal("grade");
                            while (dataReader.Read())
                            {
                                AllStudentsInSingleCourse temp=new AllStudentsInSingleCourse();
                                temp.name = dataReader.GetString(name);
                                temp.surname = dataReader.GetString(surname);
                                temp.SSN = dataReader.GetString(SSN);
                                try
                                {
                                    temp.grade = dataReader.GetDouble(grade);
                                }
                                catch(System.Data.SqlTypes.SqlNullValueException)
                                {
                                    temp.grade = null;
                                }
                                result.Add(temp);
                            }
                        }
                    }
                }
            }
            return result;
        }
        public StudentGrades OneStudentGradeOneCourse(string studentSsn, string courseName)
        {
            StudentGrades result = new StudentGrades();
            int count;
            DbInfo dbInfo = new DbInfo();
            SqlConnection connection = dbInfo.OpenConnection();
            using (connection)
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = System.String.Format("select count(*) from Students " +
                   "join CoursesStudentsRelation on Students.id = CoursesStudentsRelation.id_student " +
                   "where SSN = '{0}' and title_course='{1}'",studentSsn,courseName);
                    count = (int)command.ExecuteScalar();
                }
            }
            if (count == 0)
            {
                result.course = "aa";
                result.grade = -100;
            }
            else
            {
                result = ShowStudentGrades(studentSsn).Where(studentGrades => studentGrades.course == courseName).First();
            }
            return result;
        }
    }
}