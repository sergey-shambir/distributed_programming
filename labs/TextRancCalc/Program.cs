﻿using System;
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
            messages.ConsumeMessagesInLoop(TextMessages.QueueTextRancCalc, TextMessages.ExchangeText, (model, id) => {
                messages.SendTextRankTask(id);
            });
        }
    }
}
