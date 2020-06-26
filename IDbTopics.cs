using DataAccessLayer.Models;
using System.Collections.Generic;

namespace DataAccessLayer
{
    public interface IDbTopics
    {
        List<Topic> GetTopics(string courseName);
        bool Insert(string courseName, string topicName);
        bool Delete(string courseName, string topicName);
        bool IsExistTopic(string courseName, string topicName);
    }
}
