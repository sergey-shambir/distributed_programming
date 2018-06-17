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
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = redis.GetDatabase();

            var messages = new TextMessages();

            Console.WriteLine("Listening for TextCreated event, press Ctrl+C to stop...");
            messages.ConsumeTextCreatedInLoop((model, id) => {
                var value = db.StringGet(id);
                Console.WriteLine(id + ": " + value);
            });
        }
    }
}
