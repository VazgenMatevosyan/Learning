using DataAccessLayer;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace TestAPI.ExpectedResults
{
    public class ExpectedTopics
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
    }
}