using System;
using System.Threading;
using TextLib;

namespace TextProcessingLimiter
{
    class Program
    {
        const float minSuccessfulScore = 0.5f;

        static void Main(string[] args)
        {
            var messages = new TextMessages();
            var repo = new TextRepository();
            var metrics = new TextMetrics();

            Console.WriteLine("Listening for ExchangeTextRankCalculated event, press Ctrl+C to stop...");
            messages.ConsumeMessagesInLoop(TextMessages.QueueTextSuccessMarker, TextMessages.ExchangeTextRankCalculated, (model, json) => {
                var message = TextRankCalculatedMessage.FromJson(json);
                bool isTextSuccessful = (message.Score > minSuccessfulScore);
                messages.SendTextSuccessMarked(message.ContextId, isTextSuccessful);
            });
        }
    }
}

