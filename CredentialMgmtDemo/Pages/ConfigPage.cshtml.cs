using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CredentialMgmtDemo.Pages
{
    public class ConfigPageModel : PageModel
    {
        private readonly AzureKeyVaultService _azureKeyVaultService;

        public ConfigPageModel(AzureKeyVaultService azureKeyVaultService)
        {
            _azureKeyVaultService = azureKeyVaultService;
        }

        public void OnGet()
        {
            _azureKeyVaultService.GetSecret("testsecret");
        }
    }
}
