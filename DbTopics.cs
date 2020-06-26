using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using DataAccessLayer.Models;
namespace DataAccessLayer
{
    public class DbTopics:IDbTopics
    {
        public List<Topic> GetTopics(string courseName)
        {
            List<Topic> result = new List<Topic>();
            DbInfo dbInfo = new DbInfo();
            SqlConnection connection = dbInfo.OpenConnection();
            using (connection)
            {
                using (SqlCommand command = new SqlCommand(String.Format("select name,course_title from Topics where course_title='{0}'", courseName), connection))
                {
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            int name = dataReader.GetOrdinal("name");
                            int course = dataReader.GetOrdinal("course_title");
                            while (dataReader.Read())
                            {
                                Topic topic = new Topic();
                                topic.name = dataReader.GetString(name);
                                topic.course = dataReader.GetString(course);
                                result.Add(topic);
                            }
                        }
                    }
                }
            }
            return result;
        }
        public bool Insert(string courseName, string topicName)
        {
            bool result;
            DbInfo dbInfo = new DbInfo();
            SqlConnection connection = dbInfo.OpenConnection();
            using (connection)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select count(*) from Courses where title='" + courseName + "'";
                    int countCourses = (int)command.ExecuteScalar();
                    if (countCourses == 0)
                    {
                        return false;
                    }
                    command.CommandText = "select count(*) from Topics join Courses on Topics.course_title=Courses.title where course_title='" + courseName + "' and name='" + topicName + "'";
                    int countTopics = (int)command.ExecuteScalar();
                    if (countTopics == 0)
                    {
                        command.CommandText = "insert into Topics values('" + topicName + "','" + courseName + "')";
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
        public bool Delete(string courseName,string topicName)
        {
            bool result;
            DbInfo dbInfo = new DbInfo();
            SqlConnection connection = dbInfo.OpenConnection();
            using (connection)
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select count(*) from Topics where name='" + topicName + "' and course_title='" + courseName + "' ";
                    int countTopics = (int)command.ExecuteScalar();
                    if (countTopics == 0)
                    {
                        result = false;
                    }
                    else
                    {
                        command.CommandText = "delete from Topics where name='" + topicName + "' and course_title='" + courseName + "'";
                        command.ExecuteNonQuery();
                        result = true;
                    }
                }
            }
            return result;
        }
        public bool IsExistTopic(string courseName, string topicName)
        {
            int count;
            DbInfo dbInfo = new DbInfo();
            SqlConnection connection = dbInfo.OpenConnection();
            using (connection)
            {
                using (SqlCommand command = new SqlCommand(String.Format("select count(*) from Topics where course_title='{0}' and name='{1}'", courseName, topicName), connection))
                {
                    count = (int)command.ExecuteScalar();
                }
            }
            if (count == 1)
            {
                return true;
            }
            return false;
        }
    }
}
