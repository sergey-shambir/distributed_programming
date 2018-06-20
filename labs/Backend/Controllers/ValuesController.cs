using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using StackExchange.Redis;
using TextLib;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {

        public class UploadModel
        {
            public string Data;
        }
    
        public ValuesController()
        {
        }

        // GET api/values/<id>
        [HttpGet("{id}")]
        public string Get(string id)
        {
            var repo = new TextRepository();
            return repo.GetText(id);
        }

        // POST api/values
        [HttpPost]
        [Consumes("application/json")]
        public string Post([FromBody]UploadModel model)
        {
            var repo = new TextRepository();
            string id = repo.CreateText(model.Data);

            var messages = new TextMessages();
            messages.SendTextCreated(id);

            return id;
        }

        [HttpGet("/api/score/{id}")]
        public async Task<IActionResult> Score(string id)
        {
            Console.WriteLine("score requested for id=" + id);
            var repo = new TextRepository();
            return await RepeatLoad(() => {
                return repo.GetScore(id);
            });
        }

        [HttpGet("/api/statistics")]
        public IActionResult Statistics()
        {
            Console.WriteLine("statistics requested");
            var repo = new TextRepository();
            string json = repo.GetStatsReport();
            return Ok(json);
        }

        private async Task<IActionResult> RepeatLoad(Func<(string, bool)> load)
        {
            int[] repeatIntervalsMsec = {
                100,
                200,
                500,
                1000
            };

            (string result, bool ok) = load();
            if (ok)
            {
                return Ok(result);
            }
            foreach (int delay in repeatIntervalsMsec)
            {
                Console.WriteLine("result isn't ready, waiting for " + delay + " msec");
                await Task.Delay(delay);
                (result, ok) = load();
                if (ok)
                {
                    return Ok(result);
                }
            }
            return NotFound();
        }
    }
}
