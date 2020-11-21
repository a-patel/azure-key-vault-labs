#region Imports
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
#endregion

namespace AzureKeyVaultLabs.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var settings = config.Build();

                    if (!context.HostingEnvironment.IsDevelopment())
                    {
                        // Way-1
                        // Connect to Azure Key Vault using the Managed Identity.
                        var keyVaultEndpoint = settings["AzureKeyVaultEndpoint"];

                        if (!string.IsNullOrEmpty(keyVaultEndpoint))
                        {
                            var azureServiceTokenProvider = new AzureServiceTokenProvider();
                            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
                            config.AddAzureKeyVault(keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
                        }
                    }
                    else
                    {
                        // Way-2
                        // Connect to Azure Key Vault using the Client Id and Client Secret (AAD) - Get them from Azure AD Application.
                        var keyVaultEndpoint = settings["AzureKeyVault:Endpoint"];
                        var keyVaultClientId = settings["AzureKeyVault:ClientId"];
                        var keyVaultClientSecret = settings["AzureKeyVault:ClientSecret"];

                        if (!string.IsNullOrEmpty(keyVaultEndpoint) && !string.IsNullOrEmpty(keyVaultClientId) && !string.IsNullOrEmpty(keyVaultClientSecret))
                        {
                            config.AddAzureKeyVault(keyVaultEndpoint, keyVaultClientId, keyVaultClientSecret, new DefaultKeyVaultSecretManager());
                        }
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}



#region Reference

/*
https://github.com/Azure-Samples/key-vault-dotnet-core-quickstart
https://medium.com/@dneimke/add-keyvault-to-an-asp-net-core-application-cab1012d2b60
https://wakeupandcode.com/key-vault-for-asp-net-core-web-apps/
http://anthonygiretti.com/2019/04/29/configuring-azure-keyvault-in-asp-net-core-2-2-and-azure/

// AAD
https://docs.microsoft.com/en-us/azure/media-services/previous/media-services-portal-get-started-with-aad

*/

#endregion
