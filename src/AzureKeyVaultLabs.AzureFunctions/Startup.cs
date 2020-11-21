#region Imports
using AzureKeyVaultLabs.AzureFunctions;
using AzureKeyVaultLabs.AzureFunctions.Models;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
#endregion

[assembly: FunctionsStartup(typeof(Startup))]

namespace AzureKeyVaultLabs.AzureFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //builder.Services.AddOptions<MyConfiguration>()
            //    .Configure<IConfiguration>((settings, configuration) =>
            //    {
            //        configuration.GetSection("MyConfiguration").Bind(settings);
            //    });

            builder.Services.AddOptions<Settings>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("Settings").Bind(settings);
                });

            builder.Services.AddLogging();
            //builder.Services.AddScoped<IService, Service>();
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var builtConfig = builder.ConfigurationBuilder.Build();
            var keyVaultEndpoint = builtConfig["AzureKeyVaultEndpoint"];

            if (!string.IsNullOrEmpty(keyVaultEndpoint))
            {
                //// using Key Vault, either local dev or deployed
                //var azureServiceTokenProvider = new AzureServiceTokenProvider();
                //var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

                builder.ConfigurationBuilder
                        .AddAzureKeyVault(keyVaultEndpoint)
                        .SetBasePath(Environment.CurrentDirectory)
                        .AddJsonFile("local.settings.json", true)
                        .AddEnvironmentVariables()
                    .Build();
            }
            else
            {
                // local dev no Key Vault
                builder.ConfigurationBuilder
                   .SetBasePath(Environment.CurrentDirectory)
                   .AddJsonFile("local.settings.json", true)
                   //.AddUserSecrets(Assembly.GetExecutingAssembly(), true)
                   .AddEnvironmentVariables()
                   .Build();
            }
        }
    }
}



#region Reference

/*
https://damienbod.com/2020/07/20/using-key-vault-and-managed-identities-with-azure-functions/
https://github.com/damienbod/AzureDurableFunctions/tree/master/DurableWait
https://github.com/Azure/azure-sdk-for-net/tree/Azure.Security.KeyVault.Secrets_4.1.0/sdk/keyvault/Azure.Security.KeyVault.Secrets/

*/

#endregion
