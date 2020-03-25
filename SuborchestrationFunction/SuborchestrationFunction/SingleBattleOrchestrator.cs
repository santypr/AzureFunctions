using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

namespace SuborchestrationFunction
{
    public static class SingleBattleOrchestrator
    {
        [FunctionName("SingleBattleOrchestrator")]
        public static async Task<string> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {
            var battleTitle = context.GetInput<string>();
            var result = await context.CallActivityAsync<string>("SingleBattleOrchestrator_Battle", battleTitle);

            if (!context.IsReplaying)
            {
                Logs.LogSaintMessage($"{result}");
            }
            return result;
        }

        [FunctionName("SingleBattleOrchestrator_Battle")]
        public static string Battle([ActivityTrigger] string battleTitle, ILogger log)
        {
            Logs.LogSaintMessage($"{battleTitle} battle has started!");
            return $"{battleTitle} battle ended with Victory!";
        }
    }
}