using DataAccessLayer;
using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace TestAPI.ExpectedResults
{
    public class ExpectedCourses
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
    }
}
