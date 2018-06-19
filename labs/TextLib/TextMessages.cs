﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TextLib
{
    public class TextMessages
    {
        public static readonly string RabbitMQHost = "localhost";
        public static readonly string QueueTextRancCalc = "text-rank-calc";
        public static readonly string QueueTextListener = "text-listener";
        public static readonly string QueueVowelConsCounter = "vowel-cons-counter";
        public static readonly string QueueVowelConsRater = "vowel-cons-rater";
        public static readonly string QueueTextStatistics = "text-statistics";
        public static readonly string ExchangeText = "text";
        public static readonly string ExchangeTextRankTask = "text-rank-task";
        public static readonly string ExchangeTextScoreTask = "text-score-task";
        public static readonly string ExchangeTextRankCalculated = "text-rank-calculated";

        private ConnectionFactory _factory;

        public TextMessages()
        {
            this._factory = new ConnectionFactory() { HostName = RabbitMQHost };
        }

        public void SendTextCreated(string id)
        {
            using(var connection = this._factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                DeclareExchanges(channel);
                this.SendMessage(channel, ExchangeText, id);
            }
        }
        
        public void SendTextRankTask(string id)
        {
            using(var connection = this._factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                DeclareExchanges(channel);
                this.SendMessage(channel, ExchangeTextRankTask, id);
            }
        }

        public void SendTextScoreTask(string id, int vowelCount, int consCount)
        {
            using(var connection = this._factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                DeclareExchanges(channel);

                var message = new VowelConsCountMessage();
                message.ContextId = id;
                message.VowelCount = vowelCount;
                message.ConsCount = consCount;
                this.SendMessage(channel, ExchangeTextScoreTask, message.ToJson());
            }
        }

        public void SendTextRankCalculated(string id, float score)
        {
            using(var connection = this._factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                DeclareExchanges(channel);

                var message = new TextRankCalculatedMessage();
                message.ContextId = id;
                message.Score = score;
                this.SendMessage(channel, ExchangeTextRankCalculated, message.ToJson());
            }
        }

        public void ConsumeMessagesInLoop(string queueName, string exchange, EventHandler<string> onTextCreated)
        {
            using(var connection = this._factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                BindQueue(queueName, exchange, channel);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) => {
                    var body = ea.Body;
                    string message = Encoding.UTF8.GetString(body);
                    onTextCreated(model, message);
                };
                channel.BasicConsume(queue: queueName,
                                autoAck: true,
                                consumer: consumer);
                                
                while (true)
                {
                    Thread.Sleep(Timeout.Infinite);
                }
            }
        }

        private void DeclareExchanges(IModel channel)
        {
            channel.ExchangeDeclare(exchange: ExchangeText, type: "fanout");
            channel.ExchangeDeclare(exchange: ExchangeTextRankCalculated, type: "fanout");
            channel.ExchangeDeclare(exchange: ExchangeTextRankTask, type: "direct");
            channel.ExchangeDeclare(exchange: ExchangeTextScoreTask, type: "direct");
        }

        private void BindQueue(string queueName, string exchange, IModel channel)
        {
            DeclareExchanges(channel);
            channel.QueueDeclare(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind(
                queue: queueName,
                exchange: exchange,
                routingKey: "");
        }

        private void SendMessage(IModel channel, string exchange, string message)
        {
                channel.ConfirmSelect();
                channel.BasicReturn += (model, args) => {
                    Console.WriteLine("message returned:" + args.ToString() + ", ReplyText=" + args.ReplyText);
                };
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: exchange,
                                    mandatory: true,
                                    routingKey: "",
                                    basicProperties: null,
                                    body: body);
                channel.WaitForConfirms();
                Console.WriteLine("confirmed message with exchange '" + exchange + "'");
        }
    }
}
