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
        const string urlDetails = "/details";

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
            string url = getTextResultsUrl(id);

            return Redirect(url);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string getTextResultsUrl(string id)
        {
            var uriBuilder = new UriBuilder(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + urlDetails);
            NameValueCollection query = new NameValueCollection();
            query["id"] = id;
            uriBuilder.Query = String.Join("&", query.AllKeys.Select(a => a + "=" + HttpUtility.UrlEncode(query[a])));
            string url = uriBuilder.ToString();
            Console.WriteLine("redirect url=" + url);

            return url;
        }
    }
}
