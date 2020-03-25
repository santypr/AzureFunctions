using System;
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
    public static class MainOrchestrator
    {

        [FunctionName("SaintSeiyaOrchestrator")]
        public static async Task RunOrchestrator([OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {
            if (!context.IsReplaying)
            {
                Logs.LogSaintMessage($"** COMIENZA LA SAGA DEL SANTUARIO ** : {context.InstanceId}");
            }
            
            // SIMPLE SUBORCHESTRATOR
            await context.CallSubOrchestratorAsync("SingleBattleOrchestrator", "Torneo Galáctico");
            await context.CallSubOrchestratorAsync("SingleBattleOrchestrator", "Fénix: Los caballeros negros");
            await context.CallSubOrchestratorAsync("SingleBattleOrchestrator", "Los Santos de plata: La lucha por la armadura de oro");

            var multipleBattle = new MultipleBattle
            {
                Title = "Los Santos de oro: La batalla de las doce casas",
                Battles = new List<string> { "Aries", "Tauro", "Gemini", "Cáncer", "Leo", "Virgo", "Libra", "Escorpio", "Sagitario", "Capricornio", "Acuario", "Piscis", "Grand Pope" }
            };
            
            // COMPLEX SUBORCHESTRATOR
            await context.CallSubOrchestratorAsync("MultipleBattleOrchestrator", multipleBattle);

            if (!context.IsReplaying)
            {
                Logs.LogSaintMessage($"** FINALIZADA LA SAGA DEL SANTUARIO ** : {context.InstanceId}");
            }
        }

        [FunctionName("SaintSeiyaOrchestrator_HttpStart")]
        public static async Task<HttpResponseMessage> SaintSeiyaHttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("SaintSeiyaOrchestrator", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}