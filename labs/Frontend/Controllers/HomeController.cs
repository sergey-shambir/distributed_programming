using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Frontend.Models;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Frontend.Controllers
{
    public class HomeController : Controller
    {
        const string urlSetValue = "http://127.0.0.1:5000/api/values";

        HttpClient httpClient;

        public HomeController()
        {
            this.httpClient = new HttpClient();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm]string data)
        {
            string json = JsonConvert.SerializeObject(new Dictionary<string, string>() {
                { "data", data }
            });
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await this.httpClient.PostAsync(urlSetValue, content);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("upload failed: unexpected status code " + response.StatusCode.ToString());
            }
            string id = await response.Content.ReadAsStringAsync();

            return Ok(id);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
