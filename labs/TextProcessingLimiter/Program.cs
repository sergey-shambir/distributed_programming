using System;
using System.Threading;
using TextLib;

namespace TextProcessingLimiter
{
    class Program
    {
        const int processingLimit = 2;

        static void Main(string[] args)
        {
            int remainingAccepts = processingLimit;

            var messages = new TextMessages();
            var repo = new TextRepository();
            var metrics = new TextMetrics();

            Console.WriteLine("Listening for ExchangeText/ExchangeTextSuccessMarked events, press Ctrl+C to stop...");

            using(var connection = messages.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                messages.ListenMessages(channel, TextMessages.QueueTextProcessingLimiter, TextMessages.ExchangeText, (model, id) => {
                    Console.WriteLine("captured text '" + repo.GetText(id) + "' with id=" + id);
                    bool accepted = false;
                    if (remainingAccepts > 0)
                    {
                        --remainingAccepts;
                        accepted = true;
                    }
                    repo.SetTextStatus(id, accepted ? TextStatus.Accepted : TextStatus.Rejected);
                    messages.SendProcessingAccepted(id, accepted);
                });
                messages.ListenMessages(channel, TextMessages.QueueTextProcessingLimiterRevoke, TextMessages.ExchangeTextSuccessMarked, (model, json) => {
                    var message = TextSuccessMarkedMessage.FromJson(json);
                    repo.SetTextStatus(message.ContextId, TextStatus.Ready);
                    Console.WriteLine("revoke transaction for successful text '" + repo.GetText(message.ContextId) + "' with id=" + message.ContextId);
                    if (message.Success)
                    {
                        ++remainingAccepts;
                    }
                });
                while (true)
                {
                    Thread.Sleep(Timeout.Infinite);
                }
            }
        }
    }
}

