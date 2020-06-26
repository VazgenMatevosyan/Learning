using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DataAccessLayer.Models;
using DataAccessLayer;
using AutoMapper;
using API.Models;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [Route("api/[controller]/Course/{course}")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly IDbTopics topics;
        private readonly IMapper mapper;
        public TopicsController(IDbTopics topics, IMapper mapper)
        {
            this.topics = topics;
            this.mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<TopicAPI>> Get(string course)
        {
            List<Topic> getTopics = topics.GetTopics(course);
            try
            {
                if (getTopics.Count != 0)
                {
                    return Ok(mapper.Map<IEnumerable<TopicAPI>>(getTopics));
                }
                else
                {
                    return StatusCode(400, System.String.Format("{0} course isn't exist or {0} course doesn't have any topic.", course));
                }
            }
            catch
            {
                return StatusCode(500, "Server Error.");
            }
        }
        [HttpPost]
        public ActionResult Post([FromRoute] string course, [Required] string name)
        {
            try
            {
                if (topics.Insert(course, name))
                {
                    return Created("", System.String.Format("{0} topic successfully added for {1} course.", name, course));
                }
                else
                {
                    return StatusCode(400, System.String.Format("{0} course isn't exist or {1} topic already in {0} course.", course, name));
                }
            }
            catch
            {
                return StatusCode(500, "Server Error.");
            }
        }
        [HttpDelete]
        public ActionResult Delete([FromRoute] string course, [Required] string name)
        {
            try
            {
                if (topics.Delete(course, name))
                {
                    return Ok("");
                }
                else
                {
                    return StatusCode(400, System.String.Format("{0} course doesn't have {1} topic or {0} course isn't exist.", course, name));
                }
            }
            catch
            {
                return StatusCode(500, "Server Error.");
            }
        }
        [HttpOptions("/api/Topics")]
        public void Options()
        {
            HttpContext.Response.Headers.Add("Allows", "Get, Post, Delete");
        }
    }
}