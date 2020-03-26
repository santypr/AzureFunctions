using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FunctionsLogging
{
    public static class QueueSampleFx
    {

        //https://github.com/MicrosoftDocs/azure-docs/blob/master/articles/azure-functions/functions-bindings-storage-queue.md

        [FunctionName("QueueFunction")]
        public static void Run([QueueTrigger(
                "myqueue-items", 
                Connection = "LoggingSampleFxQueue")] string myQueueItem,
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
