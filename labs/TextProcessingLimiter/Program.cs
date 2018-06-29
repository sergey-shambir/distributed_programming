using System;
using System.Threading;
using TextLib;

namespace TextProcessingLimiter
{
    class Program
    {
        const int processingLimit = 100;

        static void Main(string[] args)
        {
            int remainingAccepts = processingLimit;

            var messages = new TextMessages();
            var repo = new TextRepository();
            var metrics = new TextMetrics();

            Console.WriteLine("Listening for ExchangeText event, press Ctrl+C to stop...");
            messages.ConsumeMessagesInLoop(TextMessages.QueueTextProcessingLimiter, TextMessages.ExchangeText, (model, id) => {
                Console.WriteLine("captured text '" + repo.GetText(id) + "' with id=" + id);
                bool accepted = false;
                if (remainingAccepts > 0)
                {
                    --remainingAccepts;
                    accepted = true;
                }
                messages.SendProcessingAccepted(id, accepted);
            });
            messages.ConsumeMessagesInLoop(TextMessages.QueueTextProcessingLimiter, TextMessages.ExchangeTextSuccessMarked, (model, json) => {
                var message = TextSuccessMarkedMessage.FromJson(json);
                Console.WriteLine("revoke transaction for successful text '" + repo.GetText(message.ContextId) + "' with id=" + message.ContextId);
                if (message.Success)
                {
                    ++remainingAccepts;
                }
            });
        }
    }
}

