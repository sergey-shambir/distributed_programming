
namespace TextLib
{
    class TextStatsReport
    {
        const float HighRankValue = 0.5f;

        private float _rankSum = 0;
        private int _textNum = 0;
        private int _highRankPart = 0;

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
