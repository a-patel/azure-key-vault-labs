# Azure Key Vault (with .NET) Labs

> Demo: Azure Key Vault + .NET 5.x





Please refer to below article(s) of my publication [Awesome Azure](https://medium.com/awesome-azure) on Azure Key Vault:

- [Keeping Secrets Safe in .NET/ASP.NET Core Applications with Azure Key Vault](https://medium.com/awesome-azure/keeping-secrets-safe-in-asp-net-core-with-azure-key-vault-228a1409bb3a)



---



## Usage: Web/API Application :page_facing_up:

### Step 1 : Install the package :package:

> To install NuGet, run the following command in the [Package Manager Console](http://docs.nuget.org/docs/start-here/using-the-package-manager-console)

```Powershell
PM> Install-Package Azure.Security.KeyVault.Secrets
PM> Install-Package Microsoft.Extensions.Configuration.AzureKeyVault
PM> Install-Package Azure.Identity
```


### Step 2 : Configuration 🔨

> Here are samples that show you how to config.

##### 2.1 : AppSettings

```js
{
  // Way-1: Connect to Azure App Configuration using the Managed Identity (for Production Scenario)
  "AzureKeyVaultEndpoint": "https://<YourKeyVaultName>.vault.azure.net",

  // Way-2: Connect to Azure App Configuration using the Connection String (for Development Scenario)
  "AzureKeyVault": {
    "Endpoint": "https://<YourKeyVaultName>.vault.azure.net",
    "ClientId": "<YourKeyVaultClientId>",
    "ClientSecret": "<YourKeyVaultClientSecret>"
  },

  "Settings": {
    "AppName": "Azure Key Vault Labs",
    "Version": 1.0,
    "FontSize": 50,
    "RefreshRate": 1000,
    "Language": "English",
    "Messages": "Hello There. Thanks for using Azure Key Vault.",
    "BackgroundColor": "Black"
  }
}

```

##### 2.2 : Configure Program Class

```cs
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
```


### Step 3 : Use in Controller or Business layer :memo:

```cs
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
    public async Task<IActionResult> GetSpecificSettings()
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
    /// Get Settings
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("get-settings")]
    public IActionResult GetSettings()
    {
        var settings = _settings;

        return Ok(settings);
    }

    #endregion
}
```





---





## Give a Star! :star:

Feel free to request an issue on github if you find bugs or request a new feature. Your valuable feedback is much appreciated to better improve this project. If you find this useful, please give it a star to show your support for this project.


## Support :telephone:

> Reach out to me at one of the following places!

- Email :envelope: at <a href="mailto:toaashishpatel@gmail.com" target="_blank">`toaashishpatel@gmail.com`</a>


## Author :boy:

* **Ashish Patel** - [A-Patel](https://github.com/a-patel)


##### Connect with me

| Linkedin | Portfolio | Medium | GitHub | NuGet | Microsoft | Twitter | Facebook | Instagram |
|----------|----------|----------|----------|----------|----------|----------|----------|----------|
| [![linkedin](https://img.icons8.com/ios-filled/96/000000/linkedin.png)](https://www.linkedin.com/in/iamaashishpatel) | [![Portfolio](https://img.icons8.com/wired/96/000000/domain.png)](https://aashishpatel.netlify.app/) | [![medium](https://img.icons8.com/ios-filled/96/000000/medium-monogram.png)](https://iamaashishpatel.medium.com) | [![github](https://img.icons8.com/ios-glyphs/96/000000/github.png)](https://github.com/a-patel) | [![nuget](https://img.icons8.com/windows/96/000000/nuget.png)](https://nuget.org/profiles/iamaashishpatel) | [![microsoft](https://img.icons8.com/ios-filled/90/000000/microsoft.png)](https://docs.microsoft.com/en-us/users/iamaashishpatel) | [![twitter](https://img.icons8.com/ios-filled/96/000000/twitter.png)](https://twitter.com/aashish_mrcool) | [![facebook](https://img.icons8.com/ios-filled/90/000000/facebook.png)](https://www.facebook.com/aashish.mrcool) | [![instagram](https://img.icons8.com/ios-filled/90/000000/instagram-new.png)](https://www.instagram.com/iamaashishpatel/) |


## Donate :dollar:

If you find this project useful — or just feeling generous, consider buying me a beer or a coffee. Cheers! :beers: :coffee:
| PayPal | BMC | Patreon |
| ------------- | ------------- | ------------- |
| [![PayPal](https://www.paypalobjects.com/webstatic/en_US/btn/btn_donate_pp_142x27.png)](https://www.paypal.me/iamaashishpatel) | [![Buy Me A Coffee](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://www.buymeacoffee.com/iamaashishpatel) | [![Patreon](https://c5.patreon.com/external/logo/become_a_patron_button.png)](https://www.patreon.com/iamaashishpatel) |


## License :lock:

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
