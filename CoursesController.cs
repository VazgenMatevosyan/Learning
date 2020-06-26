using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class CoursesController : Controller
    {
        public async Task<IActionResult> AllCourses()
        {
            List<Course> courses = new List<Course>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://vazgen-001-site1.gtempurl.com/");
            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage.Method = HttpMethod.Get;
            requestMessage.RequestUri = new Uri("api/Courses", UriKind.Relative);
            HttpResponseMessage response = await client.SendAsync(requestMessage);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                courses = JsonConvert.DeserializeObject<List<Course>>(responseBody);
            }
            return View(courses);
        }
        public async Task<IActionResult> OneCourse(string courseName)
        {
            List<Topic> topics = new List<Topic>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://vazgen-001-site1.gtempurl.com/");
            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage.Method = HttpMethod.Get;
            requestMessage.RequestUri = new Uri(String.Format("api/Topics/Course/{0}",courseName), UriKind.Relative);
            HttpResponseMessage response = await client.SendAsync(requestMessage);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                topics = JsonConvert.DeserializeObject<List<Topic>>(responseBody);
            }
            ViewData["Course Name"] = courseName;
            return View(topics);
        }
    }
}