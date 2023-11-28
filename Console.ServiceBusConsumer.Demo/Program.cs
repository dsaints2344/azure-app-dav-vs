using Azure.Messaging.ServiceBus;

const string serviceBusConnectionString = "Endpoint=sb://course.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=UaY1JOxrY9snTL/D0EhScYUMTuizxJ5B7+ASbAFn9JA=";
const string queueName = "azure-course-queue-1";

ServiceBusClient client;
ServiceBusProcessor processor = default!;


async Task MessageHandler(ProcessMessageEventArgs processMessageEventArgs)
{
    string body = processMessageEventArgs.Message.Body.ToString();
    Console.WriteLine(body);
    await processMessageEventArgs.CompleteMessageAsync(processMessageEventArgs.Message);
}

Task ErrorHandler(ProcessErrorEventArgs processErrorEventArgs)
{
    string error = processErrorEventArgs.Exception.ToString();
    Console.WriteLine(error);
    return Task.CompletedTask;
}

client = new ServiceBusClient(serviceBusConnectionString);
processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());

try
{
    processor.ProcessMessageAsync += MessageHandler;
    processor.ProcessErrorAsync += ErrorHandler;

    await processor.StartProcessingAsync();
    Console.WriteLine("Press any to stop processing.");
    Console.ReadKey();

    Console.WriteLine("\nStopping the receiver...");
    await processor.StopProcessingAsync();
    Console.WriteLine("Stopped recieving messages");
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
    throw;
}
finally
{
    await processor.DisposeAsync();
    await client.DisposeAsync();
}