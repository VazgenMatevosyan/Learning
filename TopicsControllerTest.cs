using API.Controllers;
using Xunit;
using DataAccessLayer;
using System.Collections.Generic;
using TestAPI.ExpectedResults;
using DataAccessLayer.Models;
using AutoMapper;
using API;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace TestAPI
{
    public class TopicsControllerTest
    {
        private readonly TopicsController topicsController;
        public TopicsControllerTest()
        {
            DbTopics topics = new DbTopics();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperMapings>();
                cfg.CreateMap<Topic, TopicAPI>();
            });
            var mapper = config.CreateMapper();
            topicsController = new TopicsController(topics, mapper);
        }
        [Fact]
        public void TestGetTopics()
        {
            ExpectedCourses expectedCourses = new ExpectedCourses();
            string courseName = expectedCourses.GetCourses()[0].title;
            ObjectResult result = (ObjectResult)topicsController.Get(courseName).Result;
            int? statusCode = result.StatusCode;
            Assert.Equal(200, statusCode);
            List<TopicAPI> actual = (List<TopicAPI>)result.Value;
            ExpectedTopics expectedTopics = new ExpectedTopics();
            List<Topic> expected = expectedTopics.GetTopics(courseName);
            Assert.Equal(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; ++i)
            {
                Assert.True(expected[i].name == actual[i].name && expected[i].course == expected[i].course);
            }
        }
        [Fact]
        public void TestPostTopic()
        {
            ExpectedCourses expectedCourses = new ExpectedCourses();
            string courseName = expectedCourses.GetCourses()[0].title;
            DbTopics dbTopics = new DbTopics();
            string topicName = "Test Topic";
            ObjectResult result = (ObjectResult)topicsController.Post(courseName,topicName);
            int? statusCode = result.StatusCode;
            Assert.Equal(201, statusCode);
            Assert.True(dbTopics.IsExistTopic(courseName,topicName));
            topicsController.Delete(courseName, topicName);
        }
        [Fact]
        public void TestDeleteTopic()
        {
            ExpectedCourses expectedCourses = new ExpectedCourses();
            string courseName = expectedCourses.GetCourses()[0].title;
            DbTopics dbTopics = new DbTopics();
            string topicName = "Test Topic";
            topicsController.Post(courseName,topicName);           
            ObjectResult result = (ObjectResult)topicsController.Delete(courseName, topicName);
            int? statusCode = result.StatusCode;
            Assert.Equal(200, statusCode);
            Assert.True(!dbTopics.IsExistTopic(courseName, topicName));
        }
    }
}
