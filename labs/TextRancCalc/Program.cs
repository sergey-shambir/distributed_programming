using System;
using System.Threading;
using StackExchange.Redis;
using TextLib;

namespace TextRancCalc
{
    class Program
    {
        static void Main(string[] args)
        {
            var messages = new TextMessages();
            var repo = new TextRepository();
            var metrics = new TextMetrics();

            Console.WriteLine("Listening for TextCreated event, press Ctrl+C to stop...");
            messages.ConsumeTextCreatedInLoop(TextMessages.QueueTextRancCalc, (model, id) => {
                string text = repo.GetText(id);
                float score = metrics.CalculateScore(text);
                repo.SetTextScore(id, score);

                // TODO: remove echo
                Console.WriteLine(id + " score: " + score + ", text: '" + text + "'");
            });
        }
    }
}
