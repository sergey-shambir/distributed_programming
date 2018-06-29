using System;
using System.Threading;
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

            Console.WriteLine("Listening for ExchangeProcessingAccepted event, press Ctrl+C to stop...");
            messages.ConsumeMessagesInLoop(TextMessages.QueueTextRancCalc, TextMessages.ExchangeProcessingAccepted, (model, json) => {
                var message = TextProcessingAcceptedMessage.FromJson(json);
                if (message.Accepted)
                {
                    messages.SendTextRankTask(message.ContextId);
                }
            });
        }
    }
}
