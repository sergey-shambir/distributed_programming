
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TextLib
{
    public class VowelConsCount
    {
        public int VowelCount { get; set; }
        public int ConsCount { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(new Dictionary<string, string>() {
                { "vowel", VowelCount.ToString() },
                { "cons", ConsCount.ToString() },
            });
        }
    }
}
