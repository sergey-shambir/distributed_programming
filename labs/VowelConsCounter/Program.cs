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
                string text = repo.GetText(id);
                var count = new VowelConsCount();
                count.ContextId = id;
                count.VowelCount = metrics.GetVowelCount(text);
                count.ConsCount = metrics.GetConsonantsCount(text);
                messages.SendVowelConsCount(count);
                Console.WriteLine(id + " vowel count: " + count.VowelCount + ", cons count: " + count.ConsCount);
            });
        }
    }
}

