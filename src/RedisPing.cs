using System.Drawing;
using RedisSample;
using StackExchange.Redis;
using Console = Colorful.Console;

public static class RedisPing
{
    public static async Task Run()
    {
        try
        {
            IDatabase cache = Helper.Connection.GetDatabase();
            var pingResult = await cache.PingAsync();
            Console.WriteLineFormatted("Cache response: {0}", pingResult, Color.Green, Color.Yellow);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}", Color.Red);
            LogAnalyzer.ShowAnalysis();
        }
    }
}