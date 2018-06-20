using Newtonsoft.Json;

namespace TextLib
{
    public class TextStatsReport
    {
        const float HighRankValue = 0.5f;

        [JsonProperty("rank_sum")]
        private float _rankSum = 0;

        [JsonProperty("text_num")]
        private int _textNum = 0;

        [JsonProperty("high_rank_part")]
        private int _highRankPart = 0;

        public static TextStatsReport FromJson(string json)
        {
            var obj = new TextStatsReport();
            JsonConvert.PopulateObject(json, obj);
            return obj;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    
        public int TextNum
        {
            get
            {
                return this._textNum;
            }
        }

        public int HighRankPart
        {
            get
            {
                return this._highRankPart;
            }
        }

        public float AvgRank
        {
            get
            {
                return this._rankSum / (float)this._textNum;
            }
        }

        public void AddRankResult(float score)
        {
            this._rankSum += score;
            this._textNum += 1;
            this._highRankPart += (score > HighRankValue) ? 1 : 0;
        }
    }
}
