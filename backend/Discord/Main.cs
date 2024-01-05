using Discord.WebSocket;

namespace Discord
{
	public class Main
	{
		private static string Token = "MTE5MjUyMzc3NjM3NDYyNDQxNw.Gt7QA-.WzlugCVoog1UnhqU_mgJUrx1Np7fTpGaL7t9wY";
		private static DiscordSocketClient? Client = null;
		public static void Start()
		{
			Task.Run(async () =>
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
			});
		}

		public static void SendAuthCode(ulong userId, string code)
		{
			Task.Run(async () =>
			{
				if (Client == null) return;

				var guild = Client.Guilds.FirstOrDefault(x => x.Id == 1162876377511505980);
				if (guild == null) return;

				await guild.DownloadUsersAsync();

				var user = guild.GetUser(userId);
				if (user == null) return;

				var dm = await user.CreateDMChannelAsync();
				await dm.SendMessageAsync($"Dein Authentifizierungscode: {code}");
			});
		}

		private static Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}
	}
}
