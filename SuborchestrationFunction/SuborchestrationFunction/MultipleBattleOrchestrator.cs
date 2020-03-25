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
    public static class MultipleBattleOrchestrator
    {
        [FunctionName("MultipleBattleOrchestrator")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var multipleBattle = context.GetInput<MultipleBattle>();
            if (!context.IsReplaying) {
                Logs.LogSaintMessage($"{multipleBattle.Title} stage has started!");
            }

            foreach (var battle in multipleBattle.Battles)
            {
                await context.CallSubOrchestratorAsync("SingleBattleOrchestrator", battle);
            }

            if(!context.IsReplaying)
            {
                Logs.LogSaintMessage($"{multipleBattle.Title} stage has ended!");
            }
        }
    }
}