#region Imports
using AzureKeyVaultLabs.Demo.Models;
using AzureKeyVaultLabs.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
#endregion

namespace AzureKeyVaultLabs.Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        #region Members

        private readonly Settings _settings;
        private readonly IAzureKeyVaultService _azureKeyVaultService;
        private readonly IConfiguration _configuration;

        #endregion

        #region Ctor

        public TestController(IAzureKeyVaultService azureKeyVaultService, IConfiguration configuration, IOptions<Settings> options)
        {
            _azureKeyVaultService = azureKeyVaultService;
            _configuration = configuration;
            _settings = options.Value;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get Settings (From Azure Key Vault - Specific setting)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-settings-from-azure-key-vault")]
        public async Task<IActionResult> GetSettings()
        {
            var appName = await _azureKeyVaultService.GetSecret("Settings__AppName");
            var language = await _azureKeyVaultService.GetSecret("Settings__Language");
            var messages = await _azureKeyVaultService.GetSecret("Settings__Messages");

            var settings = new Settings
            {
                AppName = appName,
                Language = language,
                Messages = messages
            };

            return Ok(settings);
        }

        /// <summary>
        /// Get Settings (From Local appsettings.json)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-settings-from-local-appsettings-json")]
        public IActionResult GetSettingsLocal()
        {
            var settings = _settings;

            // Way-2: Get specific value from appSettings.json
            var settings2 = new Settings
            {
                AppName = _configuration["Settings:AppName"],
                Language = _configuration["Settings:Language"],
                Messages = _configuration["Settings:Messages"]
            };

            return Ok(settings);
        }

        #endregion
    }
}