using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using DataAccessLayer.Models;
using System.Linq;

namespace DataAccessLayer
{
    public class DbStudents:IDbStudents
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
        public bool Insert(string name,string surname,string ssn)
        {
            bool result;
            DbInfo dbInfo = new DbInfo();
            SqlConnection connection = dbInfo.OpenConnection();
            using (connection)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select count(*) from Students where SSN='" + ssn + "'";
                    int countStudents = (int)command.ExecuteScalar();
                    if (countStudents == 0)
                    {
                        command.CommandText = "insert into Students values('" + name + "','" + surname + "','" + ssn + "')";
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
        public bool Delete(string name,string surname,string SSN)
        {
            bool result;
            DbInfo dbInfo = new DbInfo();
            SqlConnection connection = dbInfo.OpenConnection();
            using (connection)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "select count(*) from Students where SSN='" + SSN + "'";
                    int countStudents = (int)command.ExecuteScalar();
                    if (countStudents != 0)
                    {
                        command.CommandText = String.Format("select id from Students where SSN='{0}' and name='{1}' and surname='{2}'", SSN, name, surname);
                        command.ExecuteNonQuery();
                        SqlDataReader sqlDataReader = command.ExecuteReader();
                        sqlDataReader.Read();
                        string idStudent = sqlDataReader.GetInt32(0).ToString();
                        sqlDataReader.Close();
                        command.CommandText = String.Format("delete from CoursesStudentsRelation where id_student='{0}'", idStudent);
                        command.ExecuteNonQuery();
                        command.CommandText = String.Format("delete from Students where id='{0}'", idStudent);
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
        public Student GetOneStudent(string studentSsn)
        {
            Student result = new Student();
            int count;
            DbInfo dbInfo = new DbInfo();
            SqlConnection connection = dbInfo.OpenConnection();
            using (connection)
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = System.String.Format("select count(*) from Students where SSN='{0}'", studentSsn);
                    count = (int)command.ExecuteScalar();
                }
            }
            if (count == 0)
            {
                result.SSN = "---";
            }
            else
            {
                result = GetStudents().Where(student => student.SSN == studentSsn).First();
            }
            return result;
        }
    }
}
