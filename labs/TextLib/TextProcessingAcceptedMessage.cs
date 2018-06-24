using Newtonsoft.Json;
using System.Collections.Generic;

namespace TextLib
{
    public class TextProcessingAcceptedMessage
    {
        public static TextProcessingAcceptedMessage FromJson(string json)
        {
            var obj = new TextProcessingAcceptedMessage();
            JsonConvert.PopulateObject(json, obj);
            return obj;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        [JsonProperty("id")]
        public string ContextId { get; set; }

        [JsonProperty("accepted")]
        public bool Accepted { get; set; }
    }
}
