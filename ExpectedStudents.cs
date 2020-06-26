using System;
using System.Collections.Generic;
using System.Text;
using DataAccessLayer;
using DataAccessLayer.Models;
using System.Data.SqlClient;

namespace TestAPI.ExpectedResults
{
    public class ExpectedStudents
    {
        public List<Student> GetStudents()
        {
            List<Student> result = new List<Student>();
            DbInfo dbInfo = new DbInfo();
            SqlConnection connection = dbInfo.OpenConnection();
            using (connection)
            {
                using (SqlCommand command = new SqlCommand("select name,surname,SSN from Students", connection))
                {
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            int name = dataReader.GetOrdinal("name");
                            int surname = dataReader.GetOrdinal("surname");
                            int SSN = dataReader.GetOrdinal("SSN");
                            while (dataReader.Read())
                            {
                                Student student = new Student();
                                student.name = dataReader.GetString(name);
                                student.surname = dataReader.GetString(surname);
                                student.SSN = dataReader.GetString(SSN);
                                result.Add(student);
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
