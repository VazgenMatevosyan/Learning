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
    public class StudentsController : Controller
    {
        public async Task<IActionResult> AllStudents()
        {
            List<Student> students = new List<Student>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://vazgen-001-site1.gtempurl.com/");
            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage.Method = HttpMethod.Get;
            requestMessage.RequestUri = new Uri("api/Students", UriKind.Relative);
            HttpResponseMessage response = await client.SendAsync(requestMessage);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                students = JsonConvert.DeserializeObject<List<Student>>(responseBody);
            }
            return View(students);
        }
        public IActionResult OneStudent()
        {
            return View();
        }
    }
}