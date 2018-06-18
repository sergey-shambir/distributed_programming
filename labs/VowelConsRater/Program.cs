using System;
using System.Threading;
using StackExchange.Redis;
using TextLib;

namespace VowelConsRater
{
    class Program
    {
        static void Main(string[] args)
        {
            var messages = new TextMessages();
            var repo = new TextRepository();

            Console.WriteLine("Listening for TextCreated event, press Ctrl+C to stop...");
            messages.ConsumeMessagesInLoop(TextMessages.QueueVowelConsRater, TextMessages.ExchangeTextScoreTask, (model, message) => {
                VowelConsCount count = VowelConsCount.FromJson(message);
                float score = (float)count.VowelCount / (float)count.ConsCount;
                repo.SetTextScore(count.ContextId, score);
                Console.WriteLine(count.ContextId + " score: " + score);
            });
        }
    }
}


