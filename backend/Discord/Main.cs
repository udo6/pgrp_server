using Discord.WebSocket;

namespace Discord
{
	public class Main
	{
		private static string Token = "MTE5MjUyMzc3NjM3NDYyNDQxNw.Gt7QA-.WzlugCVoog1UnhqU_mgJUrx1Np7fTpGaL7t9wY";
		private static DiscordSocketClient? Client = null;
		public static async Task Start()
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

		public static async Task<bool> SendAuthCode(ulong userId, string code)
		{
			if (Client == null) return false;

			var guild = Client.Guilds.FirstOrDefault(x => x.Id == 1162876377511505980);
			if (guild == null) return false;

			await guild.DownloadUsersAsync();

			var user = guild.GetUser(userId);
			if (user == null) return false;

			var dm = await user.CreateDMChannelAsync();
			await dm.SendMessageAsync($"Dein Authentifizierungscode: {code}");
			return true;
		}

		private static Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}
	}
}
