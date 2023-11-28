// See https://aka.ms/new-console-template for more information
using Azure.Messaging.ServiceBus;

const string serviceBusConnectionString = "Endpoint=sb://course.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=UaY1JOxrY9snTL/D0EhScYUMTuizxJ5B7+ASbAFn9JA=";
const string queueName = "azure-course-queue-1";
const int maxNumberMessages = 3;

ServiceBusClient client;
ServiceBusSender sender;

client = new ServiceBusClient(serviceBusConnectionString);
sender = client.CreateSender(queueName);

using ServiceBusMessageBatch batch = await sender.CreateMessageBatchAsync();

for (int i = 1; i <= maxNumberMessages; i++)
{
    if (!batch.TryAddMessage(new ServiceBusMessage($"This is a message - {i}")))
    {
        Console.WriteLine($"Message - {i} was not added to the batch");
    }

  
}

try
{
    await sender.SendMessagesAsync(batch);
    Console.WriteLine("Messages Sent");
}
catch (Exception ex)
{

    Console.WriteLine(ex.ToString());
	throw;
}
finally
{
    await sender.DisposeAsync();
    await client.DisposeAsync();
}

