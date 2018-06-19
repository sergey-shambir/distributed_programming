using Newtonsoft.Json;
using System.Collections.Generic;

namespace TextLib
{
    public class VowelConsCountMessage
    {

        public static VowelConsCountMessage FromJson(string json)
        {
            var obj = new VowelConsCountMessage();
            JsonConvert.PopulateObject(json, obj);
            return obj;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        [JsonProperty("id")]
        public string ContextId { get; set; }

        [JsonProperty("vowel")]
        public int VowelCount { get; set; }

        [JsonProperty("cons")]
        public int ConsCount { get; set; }
    }
}
