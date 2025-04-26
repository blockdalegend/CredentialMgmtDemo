using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.KeyVault;

public class AzureKeyVaultService
{
    private readonly SecretClient _secretClient;
    private readonly IConfiguration _configuration;

    public AzureKeyVaultService(IConfiguration configuration)
    {
        _configuration = configuration;
        var credential = new DefaultAzureCredential(
            new DefaultAzureCredentialOptions
            {
                ManagedIdentityClientId = builder.Configuration.GetSection("AppSettings").GetSection("AZURE_CLIENT_ID").Value
            });
        string? keyVaultUrl = _configuration.GetSection("AppSettings").GetSection("Keyvault_URL").Value;
        _secretClient = new SecretClient(new Uri(keyVaultUrl!), credential);
    }

    public async Task<string?> GetSecret(string secretName)
    {
        var secret = await _secretClient.GetSecretAsync(String.IsNullOrEmpty(secretName) ? "testsecret": secretName);
        return secret.Value == null ? string.Empty : secret.Value.Value;
    }
}
