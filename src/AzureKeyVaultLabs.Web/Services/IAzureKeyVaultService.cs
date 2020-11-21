#region Imports
using System.Threading.Tasks;
#endregion

namespace AzureKeyVaultLabs.Web.Services
{
    public interface IAzureKeyVaultService
    {
        Task<string> GetSecret(string secretName);

        Task<string> SetSecret(string secretName, string secretValue);

        Task<string> DeleteSecret(string secretName);
    }
}
