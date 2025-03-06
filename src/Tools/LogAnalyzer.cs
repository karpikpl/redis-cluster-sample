using System.Drawing;
using System.Text;
using RedisSample;
using Console = Colorful.Console;

public class LogAnalyzer: TextWriter
{
    public static readonly LogAnalyzer Instance = new LogAnalyzer();

    private static readonly StringBuilder sb = new StringBuilder();

    public static void ShowAnalysis()
    {
        if(sb.Length == 0)
        {
            Console.WriteLine("No log analysis available.", Color.Yellow);
            return;
        }
        Console.WriteLine("Log Analysis:", Color.Green);
        Console.WriteLine(sb.ToString(), Color.Red);
        sb.Clear();
    }

    public void Analyze(string message)
    {
        // Analyze the message and log it
        // For example, you can check for specific keywords or patterns
        if (message.Contains("ConnectTimeout") || message.Contains("UnableToConnect"))
        {
            var redisHostWithPort = Helper.Configuration.Value["RedisCacheName"];
            var redisParams = redisHostWithPort!.Split(":");
            var resourceName = redisParams[0].Split(".")[0];

            sb.AppendLine("ConnectionTimout might be caused by Redis Firewal.");
            sb.AppendLine("Execute following troubleshooting steps:");
            sb.AppendFormat("1. Test Network Connectivity in Powershell by running the command: tnc {0} -p {1}\n", redisParams[0], redisParams[1]);
            sb.AppendFormat("2. Test Network Connectivity in Powershell by running the command: psping -q {0}\n", redisHostWithPort);
            sb.AppendFormat("3. Connect with Redis CLI: redis-cli -h {0} -p {1} -a <password> --tls\n", redisParams[0], redisParams[1]);
            sb.AppendLine();
            sb.AppendFormat("Consider enabling public access: az redis update --name {0} --resource-group <resource-group-name> --set publicNetworkAccess=Enabled\n", resourceName);
        }
        else if (message.Contains("NOAUTH Authentication required"))
        {
            sb.AppendLine("NOAUTH Authentication required. Please check your Redis Cache password.");
            sb.AppendLine("When using Identity to Login, please ensure you have the correct permissions.");
            sb.AppendLine("You can grant permissions using Azure Portal:");
            sb.AppendLine("Azure Cache For Redis -> Settings -> Authentication -> Microsoft Entra Authentication -> Select User and add 'Data Owner Access Policy'");
        }


        Console.Out.Write(message);
    }

    public override void Write(char value)
    {
        Analyze(value.ToString());
    }

    public override void Write(char[] buffer, int index, int count)
    {
        Analyze(new string(buffer, index, count));
    }

    public override void Write(string value)
    {
        Analyze(value);
    }

    public override Encoding Encoding => Console.Out.Encoding;
}