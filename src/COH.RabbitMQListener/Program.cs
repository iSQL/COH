using COH.Infrastructure.RabbitMQ;

var rabbitMqService = new RabbitMQService();
rabbitMqService.ReceiveMessage();
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
