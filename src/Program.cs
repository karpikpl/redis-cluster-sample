using Console = Colorful.Console;
using System.Drawing;

Console.WriteAsciiStyled("Redis Sample", new Colorful.StyleSheet(Color.OrangeRed));

// Simple ping test
await RedisPing.Run();

// Test Azure Identity access to Redis
//await RedisIdentity.Run();

//GettingStarted.Run();
//RedisString.Run();
//GeneralConcepts.Run();
//RedisTags.Run();
//RedisLists.Run();
//RedisSortedSets.Run();
//ConnectionOptions.Run();
//KeySpaceNotifications.Run();
// Clustering.Run();