using TextLib;

namespace Frontend.Models
{
    public class TextStatisticsModel
    {
        public TextStatsReport Report { get; set; }
        public bool Succeed { get; set; }
        public string ErrorText { get; set; }
    }
}
