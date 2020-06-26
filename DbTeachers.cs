using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using DataAccessLayer.Models;
using System.Linq;

namespace DataAccessLayer
{
    public class DbTeachers : IDbTeachers
    {
        public List<Teacher> GetTeachers()
        {
            List<Teacher> result = new List<Teacher>();
            DbInfo dbInfo = new DbInfo();
            SqlConnection connection = dbInfo.OpenConnection();
            using (connection)
            {
                using (SqlCommand command = new SqlCommand("select name,surname,SSN,title_course from " +
                "Teachers left join CoursesTeachersRelation on " +
                "Teachers.id=CoursesTeachersRelation.id_teacher", connection))
                {
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            int name = dataReader.GetOrdinal("name");
                            int surname = dataReader.GetOrdinal("surname");
                            int SSN = dataReader.GetOrdinal("SSN");
                            int titleCourse = dataReader.GetOrdinal("title_course");
                            while (dataReader.Read())
                            {
                                Teacher teacher = new Teacher();
                                teacher.name = dataReader.GetString(name);
                                teacher.surname = dataReader.GetString(surname);
                                teacher.SSN = dataReader.GetString(SSN);
                                teacher.course = dataReader.GetString(titleCourse);
                                result.Add(teacher);
                            }
                        }
                    }
                }
            }
            return result;
        }
        public int Insert(string name, string surname, string ssn,string course)
        {
            int result;
            DbInfo dbInfo = new DbInfo();
            SqlConnection connection = dbInfo.OpenConnection();
            using (connection)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select count(id) from Teachers where exists(select * from " +
                        "CoursesTeachersRelation where title_course= '" + course + "'" +
                        " and name = '" + name + "' and surname = '" + surname + "' and CoursesTeachersRelation.id_teacher=Teachers.id and SSN='" + ssn + "')";
                    int numberRowsCourseTeacher = (int)command.ExecuteScalar();
                    if (numberRowsCourseTeacher != 0)
                    {
                        result = 1;
                    }
                    else
                    {
                        command.CommandText = "select count(id) from Teachers where name = '" + name + "'" +
                            " and surname = '" + surname + "' and SSN='" + ssn + "'";
                        int numberRowsTeacher = (int)command.ExecuteScalar();
                        string idTeacher;
                        if (numberRowsTeacher != 0)
                        {
                            command.CommandText = "select id from Teachers where name='" + name + "' and surname='" + surname + "'" +
                                "and SSN='" + ssn + "'";
                            idTeacher = command.ExecuteScalar().ToString();
                            command.CommandText = "insert into CoursesTeachersRelation values('" + idTeacher + "','" + course + "')";
                            command.ExecuteNonQuery();
                            result = 2;
                        }
                        else
                        {
                            command.CommandText = "select count(*) from Teachers where SSN='" + ssn + "'";
                            int areSSN = (int)command.ExecuteScalar();
                            if (areSSN == 0)
                            {
                                command.CommandText = "insert into Teachers values('" + name + "','" + surname + "','" + ssn + "')";
                                command.ExecuteNonQuery();
                                command.CommandText = "select id from Teachers where name='" + name + "' and surname='" + surname + "'" +
                                    "and SSN='" + ssn + "'";
                                idTeacher = command.ExecuteScalar().ToString();
                                command.CommandText = "insert into CoursesTeachersRelation values('" + idTeacher + "','" + course + "')";
                                command.ExecuteNonQuery();
                                result = 3;
                            }
                            else
                            {
                                result = 4;
                            }
                        }
                    }
                }
            }
            return result;
        }
        public int Delete(string name, string surname, string ssn,string course)
        {
            int result;
            DbInfo dbInfo = new DbInfo();
            SqlConnection connection = dbInfo.OpenConnection();
            using (connection)
            { 
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select count(id) from Teachers where name = '" + name + "'" +
                            " and surname = '" + surname + "' and SSN='" + ssn + "'";
                    int numberRowsTeacher = (int)command.ExecuteScalar();
                    if (numberRowsTeacher == 0)
                    {
                        result = 1;
                    }
                    else
                    {
                        command.CommandText = "select id from Teachers where SSN='" + ssn + "'";
                        string idTeacher = command.ExecuteScalar().ToString();
                        command.CommandText = "delete from CoursesTeachersRelation where id_teacher='" + idTeacher + "' and title_course='" + course + "'";
                        int deletedrows = command.ExecuteNonQuery();
                        if (deletedrows == 0)
                        {
                            result = 2;
                        }
                        else
                        {
                            result = 3;
                        }
                        command.CommandText = "select count(*) from CoursesTeachersRelation where id_teacher='" + idTeacher + "'";
                        int numberRowsTeaching = (int)command.ExecuteScalar();
                        if (numberRowsTeaching == 0)
                        {
                            command.CommandText = "delete from Teachers where SSN='" + ssn + "'";
                            command.ExecuteNonQuery();
                            result = 4;
                        }
                    }
                }
            }
            return result;
        }
        public Teacher GetOneTeacher(string teacherSsn)
        {
            Teacher result = new Teacher();
            int count;
            DbInfo dbInfo = new DbInfo();
            SqlConnection connection = dbInfo.OpenConnection();
            using (connection)
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = System.String.Format("select count(*) from Teachers where SSN='{0}'", teacherSsn);
                    count = (int)command.ExecuteScalar();
                }
            }
            if (count == 0)
            {
                result.SSN = "---";
            }
            else
            {
                result = GetTeachers().Where(teacher => teacher.SSN == teacherSsn).First();
            }
            return result;
        }
    }
}
