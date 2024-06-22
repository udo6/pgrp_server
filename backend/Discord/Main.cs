using Discord.Net;
using Discord.WebSocket;

namespace Discord
{
	public class Main
	{
		private static string Token = "MTI1NDExNTk0OTg5MjczMDk1MQ.GasDEW.Z6_0lhDVuuOXUWxtgyvCFy1oQcxXD63UJ8D2Lw";
		private static DiscordSocketClient? Client = null;
		public static void Start()
		{
			ThreadPool.QueueUserWorkItem(async o =>
			{
				try
				{
					if (Client != null) return;

					Client = new DiscordSocketClient(new()
					{
						GatewayIntents = GatewayIntents.GuildMembers
					});
					Client.Log += Log;

					await Client.LoginAsync(TokenType.Bot, Token);
					await Client.StartAsync();
					await Task.Delay(-1);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Disconnected from Discord Bot");
				}
			});
		}

		public static void SendAuthCode(ulong userId, string code)
		{
			ThreadPool.QueueUserWorkItem(async o =>
			{
				try
				{
					if (Client == null) return;

					var guild = Client.Guilds.FirstOrDefault(x => x.Id == 1162876377511505980);
					if (guild == null) return;

					await guild.DownloadUsersAsync();

					var user = guild.GetUser(userId);
					if (user == null) return;

					var dm = await user.CreateDMChannelAsync();
					await dm.SendMessageAsync($"Dein Authentifizierungscode: {code}");
				}
				catch(Exception ex)
				{
					Console.WriteLine($"Cant send discord message to {userId}");
				}
			});
		}

		private static Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}
	}
}
