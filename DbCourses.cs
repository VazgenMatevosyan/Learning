using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DataAccessLayer.Models;
namespace DataAccessLayer
{
    public class DbCourses : IDbCourses
    {
        public List<Course> GetCourses()
        {
            List<Course> result = new List<Course>();
            DbInfo dbInfo = new DbInfo();
            SqlConnection connection = dbInfo.OpenConnection();
            using (connection)
            {
                using (SqlCommand command = new SqlCommand("select title,description from Courses", connection))
                {
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            int title = dataReader.GetOrdinal("title");
                            int description = dataReader.GetOrdinal("description");
                            while (dataReader.Read())
                            {
                                Course course = new Course();
                                course.title = dataReader.GetString(title);
                                course.description = dataReader.GetString(description);
                                result.Add(course);
                            }
                        }
                    }
                }
            }
            return result;
        }
        public bool Insert(string title, string description)
        {
            bool result;
            DbInfo dbInfo = new DbInfo();
            SqlConnection connection = dbInfo.OpenConnection();
            using (connection)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select count(*) from Courses where title='" + title + "'";
                    int countCourse = (int)command.ExecuteScalar();
                    if (countCourse == 0)
                    {
                        command.CommandText = "insert into Courses values('" + title + "','" + description + "')";
                        command.ExecuteNonQuery();
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            return result;
        }
        public bool Delete(string title)
        {
            bool result;
            DbInfo dbInfo = new DbInfo();
            SqlConnection connection = dbInfo.OpenConnection();
            using (connection)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    List<string> courses = new List<string>();
                    command.CommandText = "select count(title) from Courses where title='" + title + "'";
                    int existCourse = (int)command.ExecuteScalar();
                    if (existCourse == 0)
                    {
                        return result = false;
                    }
                    else
                    {
                        command.CommandText = "delete from Topics where course_title='" + title + "'";
                        command.ExecuteNonQuery();
                        command.CommandText = "delete from CoursesStudentsRelation where title_course='" + title + "'";
                        command.ExecuteNonQuery();
                        List<string> idTeachers = new List<string>();
                        command.CommandText = "select id_teacher from CoursesTeachersRelation where title_course='" + title + "'";
                        SqlDataReader sqlDataReader = command.ExecuteReader();
                        while (sqlDataReader.Read())
                        {
                            idTeachers.Add(sqlDataReader["id_teacher"].ToString());
                        }
                        sqlDataReader.Close();
                        command.CommandText = "delete from CoursesTeachersRelation where title_course='" + title + "'";
                        command.ExecuteNonQuery();
                        command.CommandType = CommandType.Text;
                        foreach (string id in idTeachers)
                        {
                            command.CommandText = "delete from Teachers where id='" + id + "'";
                            command.ExecuteNonQuery();
                        }
                        command.CommandText = "delete from Courses where title ='" + title + "'";
                        command.ExecuteNonQuery();
                        result = true;
                    }
                }
            }
            return result;
        }
        public bool IsExistCourse(string course)
        {
            DbInfo dbInfo = new DbInfo();
            SqlConnection connection = dbInfo.OpenConnection();
            using (connection)
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = System.String.Format("select count(*) from Courses where title='{0}'", course);
                    int count = (int)command.ExecuteScalar();
                    if (count == 0)
                    {
                        return false;
                    }
                    return true;
                }
            }
        }
    }
}
