using System;
using System.Threading;
using TextLib;

namespace TextStatistics
{
    class Program
    {
        static void Main(string[] args)
        {
            var messages = new TextMessages();
            var repo = new TextRepository();
            var stats = new TextStatsReport();

            Console.WriteLine("Listening for TextScoreTask event, press Ctrl+C to stop...");
            messages.ConsumeMessagesInLoop(TextMessages.QueueTextStatistics, TextMessages.ExchangeTextRankCalculated, (model, json) => {
                TextRankCalculatedMessage message = TextRankCalculatedMessage.FromJson(json);
                stats.AddRankResult(message.Score);
                repo.SetStatsReport(stats.ToJson());
            });
        }
    }
}
