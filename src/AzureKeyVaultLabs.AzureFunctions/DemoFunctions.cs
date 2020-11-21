#region Imports
using AzureKeyVaultLabs.AzureFunctions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
#endregion

namespace AzureKeyVaultLabs.AzureFunctions
{
    public class DemoFunctions
    {
        #region Members

        private readonly Settings _settings;

        #endregion

        #region Ctor

        public DemoFunctions(IOptionsSnapshot<Settings> settings)
        {
            _settings = settings.Value;
        }

        #endregion

        #region Functions

        [FunctionName("HTTPTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string appName = _settings.AppName;

            return appName != null
                ? (ActionResult)new OkObjectResult($"Hello, {appName}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }

        #endregion

        #region Utilities

        #endregion
    }
}
