using System.Drawing;
using Azure.Core;
using Azure.Identity;
using RedisSample;
using StackExchange.Redis;
using Console = Colorful.Console;

public static class RedisIdentity
{
    public static async Task Run()
    {
        try
        {
            var redisCacheName = Helper.Configuration.Value["RedisCacheName"];
            var connectionString = $"{redisCacheName},ssl=True,abortConnect=False";
            var configOptions = ConfigurationOptions.Parse(connectionString);

            var credentials = await GetCredential();
            configOptions = await configOptions.ConfigureForAzureWithTokenCredentialAsync(credentials);

            using var connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(configOptions, LogAnalyzer.Instance);
            var db = connectionMultiplexer.GetDatabase();
            var pingResult = await db.PingAsync();
            Console.WriteLineFormatted("Cache response: ", pingResult, Color.Green, Color.Yellow);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}", Color.Red);
            LogAnalyzer.ShowAnalysis();
        }
    }

    private static async Task<TokenCredential> GetCredential()
    {
        try
        {
            // tanant is needed when using Guest Accounts
            var tenantId = Helper.Configuration.Value["TenantId"];

            var options = string.IsNullOrWhiteSpace(tenantId)
             ? new AzureDeveloperCliCredentialOptions()
             : new AzureDeveloperCliCredentialOptions { TenantId = tenantId };
            
            var credentials = new AzureDeveloperCliCredential(options);

            // this is just testing credentials - not needed in production code
            var token = await credentials.GetTokenAsync(new Azure.Core.TokenRequestContext(new[] { "https://redis.azure.com/.default" }));

            return credentials;
        }
        catch (Exception ex)
        {
            Console.WriteLine("--->");
            Console.WriteLine($"Error getting credentials: {ex.Message}", Color.Red);
            Console.WriteLine("Please check your Azure Developer CLI (azd) installation and ensure you are logged in.", Color.Red);
            Console.WriteLineFormatted("You can log in using the command: {0}", "azd auth login", Color.Green, Color.Red);
            Console.WriteLine("<---");
            throw;
        }
    }
}