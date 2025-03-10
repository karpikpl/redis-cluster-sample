﻿using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisSample
{
    // Single Instance for app. - Reuse ConnectionMultiplexer
    // Timeout options
    // Key Expiration options
    // How should the multiplexer be configured.
    public static class ConnectionOptions
    {
        public static void Run()
        {
            
            IDatabase cache = Connection.GetDatabase();

            // Demo Setup
            DemoSetup(cache);
            //String
            cache.StringSet("i", 1);
            Console.WriteLine("Current Value=" + cache.StringGet("i"));
            
        }

        private static void DemoSetup(IDatabase cache)
        {
            cache.KeyDelete("i");
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            ConfigurationOptions config = new ConfigurationOptions();
            config.EndPoints.Add(Helper.Configuration.Value["RedisCacheName"]);
            config.Password = Helper.Configuration.Value["RedisCachePassword"];
            config.Ssl = true;
            config.AbortOnConnectFail = false;
            config.ConnectRetry = 5;
            config.ConnectTimeout = 1000;
            return ConnectionMultiplexer.Connect(config, LogAnalyzer.Instance);
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}
