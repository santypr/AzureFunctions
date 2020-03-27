using System;
using System.Collections;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

namespace FunctionsLogging
{
    public static class LoggingSampleFx
    {
        [FunctionName("LoggingSampleFx")]
        public static void Run([ActivityTrigger] int reps,
            [Queue("myqueue-items")]ICollector<MyQueueMessage> myQueueItems,
            ILogger log)
        {
            log.LogTrace($"TRACE - Hello world: {DateTime.Now}");
            log.LogDebug($"DEBUG - Hello world: {DateTime.Now}");
            log.LogInformation($"INFO - Hello world: {DateTime.Now}");
            log.LogWarning($"WARNING - Hello world: {DateTime.Now}");
            log.LogError($"ERROR - Hello world: {DateTime.Now}");
            log.LogCritical($"CRITICAL - Hello world: {DateTime.Now}");
            Console.WriteLine($"CONSOLE MESSAGE - Hello world: {DateTime.Now}");

            for (int i = 0; i < reps; i++)
            {
                myQueueItems.Add(new MyQueueMessage { Title = $"Hello Queue {i}", MessageDate = DateTime.Now });
            }
        }
    }
}
