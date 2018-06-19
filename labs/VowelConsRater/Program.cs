using System;
using System.Threading;
using TextLib;

namespace VowelConsRater
{
    class Program
    {
        static void Main(string[] args)
        {
            var messages = new TextMessages();
            var repo = new TextRepository();

            Console.WriteLine("Listening for TextScoreTask event, press Ctrl+C to stop...");
            messages.ConsumeMessagesInLoop(TextMessages.QueueVowelConsRater, TextMessages.ExchangeTextScoreTask, (model, message) => {
                VowelConsCountMessage count = VowelConsCountMessage.FromJson(message);
                string id = count.ContextId;
                float score = (float)count.VowelCount / (float)count.ConsCount;
                repo.SetTextScore(id, score);
                Console.WriteLine(id + " score: " + score);
                messages.SendTextRankCalculated(id, score);
            });
        }
    }
}


