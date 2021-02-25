using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQAppBackEnd.Models;

namespace RabbitMQAppBackEnd.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        [HttpPost()]
        public IActionResult Post([FromForm]User model)
        {
            var factory = new ConnectionFactory
            {
                //Tırnak içine AMQP URL'si gelecek.
                Uri = new Uri("")
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare("messagequeue", false, false);

            string serializeData = JsonSerializer.Serialize(model);
            byte[] data = Encoding.UTF8.GetBytes(serializeData);
            channel.BasicPublish("", "messagequeue",body: data);
            return Ok();
        }
    }
}
