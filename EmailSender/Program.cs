using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EmailSender
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var signalRConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/messagehub").Build();

            await signalRConnection.StartAsync();
            var factory = new ConnectionFactory
            {
                //Tırnak içine AMQP URL'si gelecek.
                Uri = new Uri("")
            };
            using var factoryConnection = factory.CreateConnection();
            using var channel = factoryConnection.CreateModel();
            channel.QueueDeclare("messagequeue", false, false);

            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume("messagequeue", true, consumer);
            consumer.Received += async (s, e) =>
            {
                //Email Operation 
                //e.Body.Span
                var serializeData = Encoding.UTF8.GetString(e.Body.Span);
                var user = JsonSerializer.Deserialize<User>(serializeData);
                EmailSenderService.Send(user.Email, user.Message);

                Console.WriteLine($"{user.Email} Mail Gönderildi!");
                await signalRConnection.InvokeAsync("SendMessageAsync", $"{user.Email} Mail Gönderildi!");

            };
            Console.Read();
        }
    }
}
