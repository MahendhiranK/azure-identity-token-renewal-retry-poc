# Azure.Identity Token Renewal Retry PoC (.NET 8)

This is a minimal .NET 8 console application demonstrating a **robust retry pattern** for handling **Managed Identity token renewal failures** when accessing Azure Key Vault secrets.

## Problem Context

Long-running Azure Container Apps workloads using Managed Identity sometimes face intermittent `AuthenticationFailedException` errors due to token renewal failures in the Azure.Identity SDK.

This sample shows how to use [Polly](https://github.com/App-vNext/Polly) to implement exponential backoff retries for token acquisition and service calls to improve reliability.

## How to run

1. Replace `https://your-keyvault-name.vault.azure.net/` in `Program.cs` with your Azure Key Vault URI.

2. Replace `"MySecretName"` with the actual secret name in your Key Vault.

3. Build and run the project with:

   ```bash
   dotnet run
   ```

## Packages used

- Azure.Identity v1.8.0  
- Azure.Security.KeyVault.Secrets v4.3.0  
- Polly v7.2.3

---

Feel free to reuse and improve this retry pattern to increase robustness of Managed Identity in long-running apps.

---
