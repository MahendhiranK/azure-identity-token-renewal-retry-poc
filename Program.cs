using System;
using System.Threading.Tasks;
using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Polly;
using Polly.Retry;

class Program
{
    private static AsyncRetryPolicy retryPolicy;

    static async Task Main(string[] args)
    {
        // Replace with your actual Key Vault URI
        string keyVaultUri = "https://your-keyvault-name.vault.azure.net/";

        var credential = new DefaultAzureCredential();

        var secretClient = new SecretClient(new Uri(keyVaultUri), credential);

        // Define a retry policy that retries on authentication failures and transient errors
        retryPolicy = Policy
            .Handle<AuthenticationFailedException>()
            .Or<RequestFailedException>(ex => ex.Status >= 500)
            .WaitAndRetryAsync(
                retryCount: 5,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    Console.WriteLine($"Retry {retryCount} due to: {exception.Message}. Waiting {timeSpan.TotalSeconds}s before next retry.");
                });

        try
        {
            // Execute the secret retrieval within the retry policy
            await retryPolicy.ExecuteAsync(async () =>
            {
                KeyVaultSecret secret = await secretClient.GetSecretAsync("MySecretName");
                Console.WriteLine($"Secret value: {secret.Value}");
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to retrieve secret after retries: {ex.Message}");
        }
    }
}
