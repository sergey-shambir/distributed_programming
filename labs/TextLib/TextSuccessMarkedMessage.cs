using Newtonsoft.Json;
using System.Collections.Generic;

namespace TextLib
{
    public class TextSuccessMarkedMessage
    {
        public static TextSuccessMarkedMessage FromJson(string json)
        {
            var obj = new TextSuccessMarkedMessage();
            JsonConvert.PopulateObject(json, obj);
            return obj;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        [JsonProperty("id")]
        public string ContextId { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
