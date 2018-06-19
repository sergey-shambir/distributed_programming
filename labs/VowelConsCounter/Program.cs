using System;
using System.Threading;
using StackExchange.Redis;
using TextLib;

namespace VowelConsCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            var messages = new TextMessages();
            var repo = new TextRepository();
            var metrics = new TextMetrics();

            Console.WriteLine("Listening for TextCreated event, press Ctrl+C to stop...");
            messages.ConsumeMessagesInLoop(TextMessages.QueueVowelConsCounter, TextMessages.ExchangeTextRankTask, (model, id) => {
                try
                {
                    string text = repo.GetText(id);
                    int vowelCount = metrics.GetVowelCount(text);
                    int consCount = metrics.GetConsonantsCount(text);
                    messages.SendTextScoreTask(id, vowelCount, consCount);
                    Console.WriteLine(id + " vowel count: " + vowelCount + ", cons count: " + consCount);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("exception occured: " + ex.ToString());
                }
            });
        }
    }
}

