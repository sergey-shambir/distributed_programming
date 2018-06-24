using System;
using System.Threading;
using TextLib;

namespace TextProcessingLimiter
{
    class Program
    {
        static void Main(string[] args)
        {
            var messages = new TextMessages();
            var repo = new TextRepository();
            var metrics = new TextMetrics();

            Console.WriteLine("Listening for TextCreated event, press Ctrl+C to stop...");
            messages.ConsumeMessagesInLoop(TextMessages.QueueTextProcessingLimiter, TextMessages.ExchangeText, (model, id) => {
                // TODO: <sergey.shambir> implement limiting
                messages.SendProcessingAccepted(id, accepted);
            });
        }
    }
}

