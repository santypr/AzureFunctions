using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FunctionsLogging
{
    public static class DurableFunctionSample
    {
        [FunctionName("DurableFunctionSample")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger log)
        {
            var outputs = new List<string>();

            log.LogTrace($"****************** HELLO FROM ORCHESTRATOR '{context.InstanceId}'.");
            outputs.Add(await context.CallActivityAsync<string>("LoggingSampleFx", 2));

            return outputs;
        }

        [FunctionName("DurableFunctionSample_TimerStart")]
        public static async Task TimerStart(
            [TimerTrigger("*/5 * * * * *")]TimerInfo myTimer,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("DurableFunctionSample", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
        }

        [FunctionName("DurableFunctionSample_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("DurableFunctionSample", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }

    }
}