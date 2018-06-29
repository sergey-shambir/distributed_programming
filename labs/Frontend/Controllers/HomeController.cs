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
using System.Web;
using Newtonsoft.Json;
using System.Collections.Specialized;

namespace Frontend.Controllers
{
    public class HomeController : Controller
    {
        const string urlSetValue = "http://127.0.0.1:5000/api/values";
        const string urlGetScore = "http://127.0.0.1:5000/api/score/";
        const string urlDetails = "/Home/Details/";

        HttpClient _httpClient;

        public HomeController()
        {
            this._httpClient = new HttpClient();
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
            HttpResponseMessage response = await this._httpClient.PostAsync(urlSetValue, content);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("upload failed: unexpected status code " + response.StatusCode.ToString());
            }
            string id = await response.Content.ReadAsStringAsync();
            string url = GetTextResultsUrl(id);

            return Redirect(url);
        }

        [HttpGet("/Home/Details/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            Console.WriteLine("requested details for id=" + id);
            string url = urlGetScore + id;
            HttpResponseMessage response = await this._httpClient.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string score = await response.Content.ReadAsStringAsync();
                return View(new ScoreViewModel { Succeed = true, Score = score } );
            }
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return View(new ScoreViewModel{ Succeed = false, ErrorText = "text processing limit exceed"});
            }
            return View(new ScoreViewModel{ Succeed = false, ErrorText = "HTTP status code " + response.StatusCode});

        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string GetTextResultsUrl(string id)
        {
            string url = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + urlDetails + id;
            return url;
        }

        private string AddUrlQuery(string url, NameValueCollection query)
        {
            var uriBuilder = new UriBuilder(url);
            uriBuilder.Query = String.Join("&", query.AllKeys.Select(a => a + "=" + HttpUtility.UrlEncode(query[a])));
            return uriBuilder.ToString();
        }
    }
}
