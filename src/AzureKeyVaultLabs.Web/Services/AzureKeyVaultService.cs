#region Imports
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using AzureKeyVaultLabs.Demo.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Threading.Tasks;
#endregion

namespace AzureKeyVaultLabs.Web.Services
{
    public class AzureKeyVaultService : IAzureKeyVaultService
    {
        #region Members

        private readonly SecretClient _client;
        //private readonly Settings _settings;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger _logger;

        #endregion

        #region Ctor

        public AzureKeyVaultService(IConfiguration configuration, IWebHostEnvironment env, ILogger logger)
        {
            //_settings = options.Value;
            _configuration = configuration;
            _env = env;
            _logger = logger;

            var keyVaultUrl = _configuration.GetValue<string>("AzureKeyVaultEndpoint");

            if (!_env.IsDevelopment())
            {
                // Way-2: Secure (For Production scenario, works with Azure Services like App Service, VM, VMSS, Functions)
                // Connect to Azure Key Vault using the Managed Identity.
                _client = new SecretClient(vaultUri: new Uri(keyVaultUrl), credential: new DefaultAzureCredential());
            }
            else
            {
                ////TODO: Imeplement
                //// Way-2: Less Secure (For Developement scenario or Non Azure Service like On-Premise VM/Servers)
                //// Connect to Azure Key Vault using the Client Id and Client Secret (AAD) - Get them from Azure AD Application.
                //var keyVaultClientId = _configuration["AzureKeyVault:ClientId"];
                //var keyVaultClientSecret = _configuration["AzureKeyVault:ClientSecret"];

                ////var credential = new ClientCredential(keyVaultClientId, keyVaultClientSecret);
                ////var authenticationContext = new AuthenticationContext($"https://login.microsoftonline.com/{TenantId}");
                ////var result = authenticationContext.AcquireTokenAsync(uri, credential).Result;
                ////var token = result.AccessToken;

                ////if (!string.IsNullOrEmpty(keyVaultEndpoint) && !string.IsNullOrEmpty(keyVaultClientId) && !string.IsNullOrEmpty(keyVaultClientSecret))
                ////{
                ////    config.AddAzureKeyVault(keyVaultEndpoint, keyVaultClientId, keyVaultClientSecret, new DefaultKeyVaultSecretManager());
                ////}
                //_client = new SecretClient(vaultUri: new Uri(keyVaultUrl), credential: credential);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Retrieve a secret using the secret client.
        /// </summary>
        /// <param name="secretName"></param>
        /// <returns></returns>
        public async Task<string> GetSecret(string secretName)
        {
            var secret = await _client.GetSecretAsync(secretName);

            return secret.Value.Value;
        }

        /// <summary>
        /// Create a new secret using the secret client.
        /// </summary>
        /// <param name="secretName"></param>
        /// <param name="secretValue"></param>
        /// <returns></returns>
        public async Task<string> SetSecret(string secretName, string secretValue)
        {
            var secret = await _client.SetSecretAsync(secretName, secretValue);

            _logger.LogInformation($"Set new value of secret: {secret.Value.Name}");

            return secret.Value.Value;
        }


        /// <summary>
        /// Create a new secret using the secret client.
        /// </summary>
        /// <param name="secretName"></param>
        /// <returns></returns>
        public async Task<string> DeleteSecret(string secretName)
        {
            var operation = await _client.StartDeleteSecretAsync(secretName);
            var secret = await operation.WaitForCompletionAsync();

            _client.PurgeDeletedSecret(operation.Value.Name);

            _logger.LogInformation($"Deleted secret: {secret.Value.Name}");

            return secret.Value.Value;
        }

        #endregion
    }
}



#region @@Reference
/*
https://github.com/Azure/azure-sdk-for-net/tree/Azure.Security.KeyVault.Secrets_4.1.0/sdk/keyvault/Azure.Security.KeyVault.Secrets/ 
*/
#endregion
