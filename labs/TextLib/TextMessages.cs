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
        private static readonly string RabbitMQHost = "localhost";
        private static readonly string QueueTextCreated = "RPTextCreated";
        
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
                DeclareQueue(channel);
                var body = Encoding.UTF8.GetBytes(id);
                channel.ConfirmSelect();
                channel.BasicReturn += (model, args) => {
                    Console.WriteLine("message returned:" + args.ToString() + ", ReplyText=" + args.ReplyText + ", ReplyCode=" + args.ReplyCode
                    + ", RoutingKey=" + args.RoutingKey + ", Body=" + args.Body);
                };
                channel.BasicPublish(exchange: "",
                                    mandatory: true,
                                    routingKey: QueueTextCreated,
                                    basicProperties: null,
                                    body: body);
                Console.WriteLine("TextCreated published - waiting for confim");
                channel.WaitForConfirms();
                Console.WriteLine("TextCreated confirmed");
            }
        }

        public void ConsumeTextCreatedInLoop(EventHandler<string> onTextCreated)
        {
            using(var connection = this._factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                DeclareQueue(channel);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) => {
                    var body = ea.Body;
                    string message = Encoding.UTF8.GetString(body);
                    onTextCreated(model, message);
                };
                channel.BasicConsume(queue: QueueTextCreated,
                                autoAck: true,
                                consumer: consumer);
                                
                while (true)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private void DeclareQueue(IModel channel)
        {
            channel.QueueDeclare(
                queue: QueueTextCreated,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }
    }
}