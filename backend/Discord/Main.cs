using Core.Enums;
using Database.Models.Account;
using Database.Models.Inventory;
using Database.Models;
using Database.Services;
using Discord.Net;
using Discord.WebSocket;
using Core;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace Discord
{
	public static class Main
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

					Client.Ready += ClientReady;
					Client.Log += Log;
					Client.SlashCommandExecuted += SlashCommandHandler;
					Client.ModalSubmitted += ModalSubmitted;

                    await Client.LoginAsync(TokenType.Bot, Token);
					await Client.StartAsync();
					await Task.Delay(-1);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Disconnected from Discord Bot! Reconnecting...");
				}
			});
		}

        private static async Task ClientReady()
		{
			await Client!.CreateGlobalApplicationCommandAsync(new SlashCommandBuilder()
				.WithName("register")
				.WithDescription("Erstelle deinen Account!")
				.WithContextTypes(InteractionContextType.BotDm)
				.Build());
        }


        private static async Task SlashCommandHandler(SocketSlashCommand command)
		{
			if (!command.IsDMInteraction)
			{
				await command.DeferAsync();
				return;
			}

			switch (command.Data.Name)
			{
				case "register":
					await command.RespondWithModalAsync(new ModalBuilder()
						.WithTitle("Charakter Erstellung")
						.WithCustomId("RegisterModal")
						.AddTextInput("Name", "RegisterModal::Name", placeholder:"Max_Mustermann")
                        .Build());
					break;
			}
		}

        private static async Task ModalSubmitted(SocketModal modal)
        {
			if (modal.Data.CustomId != "RegisterModal")
			{
				await modal.DeferAsync();
                return;
            }

			var name = modal.Data.Components.FirstOrDefault(x => x.CustomId == "RegisterModal::Name")?.Value;
			if(name == null || name == string.Empty)
			{
                await modal.DeferAsync();
                return;
            }

			if(!Regex.IsMatch(name, "([a-zA-Z]+)_([a-zA-Z]+)$"))
			{
                await modal.RespondAsync("Der Name muss dem Format Vorname_Nachname entsprechen!");
                return;
            }

			if (AccountService.IsNameTaken(name))
            {
                await modal.RespondAsync("Der angegebene Name ist bereits vergeben!");
                return;
			}

			if(AccountService.IsDiscordIdTaken(modal.User.Id))
            {
                await modal.RespondAsync("Du hast bereits einen Account!");
                return;
            }

            var pos = new PositionModel(-1042.4572f, -2745.323f, 21.343628f, 0, 0, -0.49473903f);
            PositionService.Add(pos);

            var custom = new CustomizationModel();
            CustomizationService.Add(custom);

            var clothes = new ClothesModel();
            ClothesService.Add(clothes);

            var inv = new InventoryModel(Config.Inventory.DEFAULT_SLOTS, Config.Inventory.DEFAULT_WEIGHT, InventoryType.PLAYER);
            var labInput = new InventoryModel(8, 30f, InventoryType.LAB_INPUT);
            var labOutput = new InventoryModel(8, 60f, InventoryType.LAB_OUTPUT);
            var locker = new InventoryModel(8, 100f, InventoryType.LOCKER);
            var teamLocker = new InventoryModel(8, 100f, InventoryType.TEAM_LOCKER);
            InventoryService.Add(inv, labInput, labOutput, locker, teamLocker);

            var license = new LicenseModel();
            LicenseService.Add(license);

            AccountService.Add(new(
				name,
				string.Empty,
				0,
				0,
				0,
				modal.User.Id,
				15000,
				35000,
				AccountService.GenerateUniquePhoneNumber(),
				pos.Id,
                custom.Id,
                clothes.Id,
                inv.Id,
                labInput.Id,
                labOutput.Id,
                locker.Id,
                0, // Business
                license.Id,
                false,
                teamLocker.Id));

            await modal.RespondAsync("Dein Charakter wurde erstellt! Du kannst dich nun auf dem Gameserver einloggen.");
        }


        public static void SendAuthCode(ulong userId, string code)
		{
			ThreadPool.QueueUserWorkItem(async o =>
			{
				try
				{
					if (Client == null) return;

                    var guild = Client.Guilds.FirstOrDefault(x => x.Id == 1254115515966689350);
					if (guild == null) return;

					await guild.DownloadUsersAsync();

                    var user = guild.GetUser(userId);
					if (user == null) return;

                    var embed = new EmbedBuilder()
						.WithTitle("Authentifizierung")
						.WithColor(55, 55, 200)
						.WithFields(
							new EmbedFieldBuilder().WithName("Code").WithValue($"```{code}```")
						).WithCurrentTimestamp()
						.WithFooter(new EmbedFooterBuilder().WithText("XXX Roleplay"));

                    var dm = await user.CreateDMChannelAsync();
					await dm.SendMessageAsync("", false, embed.Build());
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
