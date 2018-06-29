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
            var repo = new TextRepository();
            var messages = new TextMessages();

            Console.WriteLine("Listening for ExchangeText event, press Ctrl+C to stop...");
            messages.ConsumeMessagesInLoop(TextMessages.QueueTextListener, TextMessages.ExchangeText, (model, id) => {
                var text = repo.GetText(id);
                Console.WriteLine(id + ": " + text);
            });
        }
    }
}
