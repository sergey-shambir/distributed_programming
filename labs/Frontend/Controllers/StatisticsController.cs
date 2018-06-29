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
using TextLib;

namespace Frontend.Controllers
{
    [Route("[controller]")]
    public class StatisticsController : Controller
    {
        const string urlGetStatistics = "http://127.0.0.1:5000/api/statistics";


        HttpClient _httpClient;

        public StatisticsController()
        {
            this._httpClient = new HttpClient();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                HttpResponseMessage response = await this._httpClient.GetAsync(urlGetStatistics);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string textResponse = await response.Content.ReadAsStringAsync();
                    var report = TextStatsReport.FromJson(textResponse);
                    return View(new TextStatisticsModel{ Succeed = true, Report = report });
                }
                return View(new TextStatisticsModel{ Succeed = false, ErrorText = "HTTP status code " + response.StatusCode});
            }
            catch (Exception ex)
            {
                return View(new TextStatisticsModel{ Succeed = false, ErrorText = ex.ToString() });
            }
        }
    }
}
