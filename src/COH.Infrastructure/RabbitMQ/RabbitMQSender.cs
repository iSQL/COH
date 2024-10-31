using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using RabbitMQ.Client;
namespace COH.Infrastructure.RabbitMQ;
public class RabbitMQSender
{
  ConnectionFactory factory = new();
  IConnection cnn;
  IModel channel;
  string exchangeName;
  string routingKey;
  string queueName;
  public RabbitMQSender()
  {
    factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
    factory.ClientProvidedName = "COH Sender";
    cnn = factory.CreateConnection();
    channel = cnn.CreateModel();

    exchangeName = "COHExchange";
    routingKey = "COHRK";
    queueName = "COHQueue";

    channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
    channel.QueueDeclare(queueName, false, false, false, null);
    channel.QueueBind(queueName, exchangeName, routingKey, null);
  }
 public void SendMessage()
  {
    byte[] messageBody = Encoding.UTF8.GetBytes("Hello from COH");
    channel.BasicPublish(exchangeName, routingKey, null, messageBody);
    channel.Close();
    cnn.Close();
  }
}
