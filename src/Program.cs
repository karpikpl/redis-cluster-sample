using Console = Colorful.Console;
using System.Drawing;

Console.WriteAsciiStyled("Redis Sample", new Colorful.StyleSheet(Color.OrangeRed));

//await RedisPing.Run();
await RedisIdentity.Run();

//GettingStarted.Run();
//RedisString.Run();
//GeneralConcepts.Run();
//RedisTags.Run();
//RedisLists.Run();
//RedisSortedSets.Run();
//ConnectionOptions.Run();
//KeySpaceNotifications.Run();
// Clustering.Run();