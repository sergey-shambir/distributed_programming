using Newtonsoft.Json;
using System.Collections.Generic;

namespace TextLib
{
    public class TextRankCalculatedMessage
    {
        public static TextRankCalculatedMessage FromJson(string json)
        {
            var obj = new TextRankCalculatedMessage();
            JsonConvert.PopulateObject(json, obj);
            return obj;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        [JsonProperty("id")]
        public string ContextId { get; set; }

        [JsonProperty("score")]
        public float Score { get; set; }
    }
}
