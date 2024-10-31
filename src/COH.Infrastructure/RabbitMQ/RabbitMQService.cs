using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace COH.Infrastructure.RabbitMQ;
public class RabbitMQService
{
  private readonly IConnection? _connection;
  private readonly IModel? _channel;
  private string _exchangeName { get; set; }
  private string _routingKey { get; set; }
  private string _queueName { get; set; }


  public RabbitMQService(string exchangeName ="COHExchange", string queueName = "COHQueue", string routingKey = "COHRK")
  {
    _exchangeName = exchangeName;
    _queueName = queueName;
    _routingKey = routingKey;

    var factory = new ConnectionFactory() { Uri = new Uri("amqp://guest:guest@localhost:5672"), ClientProvidedName = "COH Sender" };
    _connection = factory.CreateConnection();
    _channel = _connection.CreateModel();
    _channel.QueueDeclare(queue: _queueName,durable: false, exclusive: false, autoDelete: false, arguments: null);
    _channel.ExchangeDeclare(exchange: _exchangeName,type: ExchangeType.Direct);
    _channel.QueueBind(queue: _queueName, exchange: _exchangeName, routingKey: _routingKey, arguments: null);
  }

  public void SendMessage(string message)
  {
    var body = Encoding.UTF8.GetBytes(message);
    _channel.BasicPublish(exchange: _exchangeName,
                         routingKey: _routingKey,
                         basicProperties: null,
                         body: body);
  }
  public void ReceiveMessage()
  {
    var consumer = new EventingBasicConsumer(_channel);
    consumer.Received += (model, ea) =>
    {
      var body = ea.Body.ToArray();
      var message = Encoding.UTF8.GetString(body);
      Console.WriteLine("Received message: " + message);
    };
    _channel.BasicConsume(queue: _queueName,
                         autoAck: true,
                         consumer: consumer);
  }

  public void Dispose()
  {
    _channel?.Close();
    _connection?.Close();
  }

}

