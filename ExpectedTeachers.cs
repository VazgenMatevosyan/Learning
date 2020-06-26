using DataAccessLayer;
using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace TestAPI.ExpectedResults
{
    public class ExpectedTeachers
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
    }
}
