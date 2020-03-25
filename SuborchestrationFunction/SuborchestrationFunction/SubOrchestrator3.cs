using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace SuborchestrationFunction
{
    public static class SubOrchestrator3
    {
        // in orchestrator use 
        //Task provisionTask3 = context.CallSubOrchestratorAsync("SubOrchestrator3", context.InstanceId);




        [FunctionName("SubOrchestrator3")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            // Replace "hello" with the name of your Durable Activity Function.
            outputs.Add(await context.CallActivityAsync<string>("SubOrchestrator3_Hello", "Tokyo"));
            outputs.Add(await context.CallActivityAsync<string>("SubOrchestrator3_Hello", "Seattle"));
            outputs.Add(await context.CallActivityAsync<string>("SubOrchestrator3_Hello", "London"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }

        [FunctionName("SubOrchestrator3_Hello")]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name}.");
            return $"Hello {name}!";
        }

        //[FunctionName("SubOrchestrator3_HttpStart")]
        //public static async Task<HttpResponseMessage> HttpStart(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
        //    [DurableClient] IDurableOrchestrationClient starter,
        //    ILogger log)
        //{
        //    // Function input comes from the request content.
        //    string instanceId = await starter.StartNewAsync("SubOrchestrator", null);

        //    log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

        //    return starter.CreateCheckStatusResponse(req, instanceId);
        //}

        [FunctionName("BudgetApproval")]
        public static async Task RunApproval([OrchestrationTrigger] IDurableOrchestrationContext context)
        {

            bool approved = await context.WaitForExternalEvent<bool>("Approval");
            if (approved)
            {
                // approval granted - do the approved action
            }
            else
            {
                // approval denied - send a notification
            }
        }

        //[FunctionName("Select")]
        //public static async Task RunSelect([OrchestrationTrigger] IDurableOrchestrationContext context)
        //{
        //    var event1 = context.WaitForExternalEvent<float>("Event1");
        //    var event2 = context.WaitForExternalEvent<bool>("Event2");
        //    var event3 = context.WaitForExternalEvent<int>("Event3");
        //    var winner = await Task.WhenAny(event1, event2, event3);
        //    if (winner == event1)
        //    {
        //        // ...
        //    }
        //    else if (winner == event2)
        //    {
        //        // ...
        //    }
        //    else if (winner == event3)
        //    {
        //        // ...
        //    }
        //}

        //[FunctionName("NewBuildingPermit")]
        //public static async Task RunBuildingPermit([OrchestrationTrigger] IDurableOrchestrationContext context)
        //{
        //    string applicationId = context.GetInput<string>();
        //    var gate1 = context.WaitForExternalEvent("CityPlanningApproval");
        //    var gate2 = context.WaitForExternalEvent("FireDeptApproval");
        //    var gate3 = context.WaitForExternalEvent("BuildingDeptApproval");
        //    // all three departments must grant approval before a permit can be issued
        //    await Task.WhenAll(gate1, gate2, gate3);
        //    await context.CallActivityAsync("IssueBuildingPermit", applicationId);
        //}

        //[FunctionName("ApprovalQueueProcessor")]
        //public static async Task RunApprovalProcessed(
        //        [QueueTrigger("approval-queue")] string instanceId,
        //        [DurableClient] IDurableOrchestrationClient client)
        //{
        //    await client.RaiseEventAsync(instanceId, "Approval", true);
        //}

        //[FunctionName("BudgetApproval_HttpStart")]
        //public static async Task<HttpResponseMessage> BudgetApprovalHttpStart(
        //        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
        //        [DurableClient] IDurableOrchestrationClient starter,
        //        ILogger log)
        //{
        //    // Function input comes from the request content.
        //    string instanceId = await starter.StartNewAsync("BudgetApproval", null);

        //    log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

        //    return starter.CreateCheckStatusResponse(req, instanceId);
        //}

    }
}