using System;
using System.Collections.Generic;
using System.Linq;
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

        private IDatabase _database;
    
        public ValuesController()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            this._database = redis.GetDatabase();
        }

        // GET api/values/<id>
        [HttpGet("{id}")]
        public string Get(string id)
        {
            return this._database.StringGet(id);
        }

        // POST api/values
        [HttpPost]
        [Consumes("application/json")]
        public string Post([FromBody]UploadModel model)
        {
            var id = Guid.NewGuid().ToString();
            this._database.StringSet(id, model.Data);

            var messages = new TextMessages();
            messages.SendTextCreated(id);

            return id;
        }
    }
}
