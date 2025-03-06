using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace RedisSample
{
    public static class Helper
    {
        public static Lazy<IConfiguration> Configuration = new Lazy<IConfiguration>(() =>
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
            return builder.Build();
        });

        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            var redisCacheName = Configuration.Value["RedisCacheName"];
            var redisCachePassword = Configuration.Value["RedisCachePassword"];

            return ConnectionMultiplexer.Connect(redisCacheName + ",abortConnect=false,ssl=true,password=" + redisCachePassword, LogAnalyzer.Instance);
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }

    public class BlogPost
    {
        private HashSet<string> tags = new HashSet<string>();

        public BlogPost(int id, string title, int score, IEnumerable<string> tags)
        {
            this.Id = id;
            this.Title = title;
            this.Score = score;
            this.tags = new HashSet<string>(tags);
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int Score { get; set; }
        public ICollection<string> Tags { get { return this.tags; } }
    }
}
