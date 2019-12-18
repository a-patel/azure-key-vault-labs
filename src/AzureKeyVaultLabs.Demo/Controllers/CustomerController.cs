#region Imports
using AzureKeyVaultLabs.Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
#endregion

namespace AzureKeyVaultLabs.Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        #region Members

        private readonly Settings _settings;
        private readonly IConfiguration _configuration;

        #endregion

        #region Ctor

        public CustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
            //_settings = options.Value;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get Settings
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-settings")]
        public IActionResult GetSettings()
        {
            var settings = new Settings
            {
                AppName = _configuration["Settings-AppName"],
                Language = _configuration["Settings-Language"],
                Messages = _configuration["Settings-Messages"]
            };

            return Ok(settings);
        }

        #endregion
    }
}