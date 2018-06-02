using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {

        public class UploadModel
        {
            public string Data;
        }

        static readonly ConcurrentDictionary<string, string> _data = new ConcurrentDictionary<string, string>();

        // GET api/values/<id>
        [HttpGet("{id}")]
        public string Get(string id)
        {
            string value = null;
            _data.TryGetValue(id, out value);
            return value;
        }

        // POST api/values
        [HttpPost]
        [Consumes("application/json")]
        public string Post([FromBody]UploadModel model)
        {
            var id = Guid.NewGuid().ToString();
            _data[id] = model.Data;
            return id;
        }
    }
}
