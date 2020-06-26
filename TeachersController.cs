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
    public class TeachersController : Controller
    {
        public async Task<IActionResult> AllTeachers()
        {
            List<Teacher> teachers = new List<Teacher>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://vazgen-001-site1.gtempurl.com/");
            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage.Method = HttpMethod.Get;
            requestMessage.RequestUri = new Uri("api/Teachers", UriKind.Relative);
            HttpResponseMessage response = await client.SendAsync(requestMessage);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                teachers = JsonConvert.DeserializeObject<List<Teacher>>(responseBody);
            }
            return View(teachers);
        }
    }
}