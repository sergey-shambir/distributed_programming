
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TextLib
{
    public class VowelConsCount
    {

        public static VowelConsCount FromJson(string json)
        {
            var obj = new VowelConsCount();
            JsonConvert.PopulateObject(json, obj);
            return obj;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        
        [JsonProperty("id")]
        public string ContextId { get; set; }
        public int VowelCount { get; set; }
        public int ConsCount { get; set; }
    }
}
