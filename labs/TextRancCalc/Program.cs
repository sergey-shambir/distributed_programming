using System;
using System.Threading;
using StackExchange.Redis;
using TextLib;

namespace TextListener
{
    class Program
    {
        static void Main(string[] args)
        {
            var messages = new TextMessages();
            var repo = new TextRepository();
            var metrics = new TextMetrics();

            Console.WriteLine("Listening for TextCreated event, press Ctrl+C to stop...");
            messages.ConsumeTextCreatedInLoop((model, id) => {
                string text = repo.GetText(id);
                float score = metrics.CalculateScore(text);
                repo.SetTextScore(id, score);

                // TODO: remove debug code
                // Console.WriteLine(id + " score: " + score + ", text: '" + text);
            });
        }
    }
}
