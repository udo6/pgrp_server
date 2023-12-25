using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Models.Bank;
using Database.Models.Phone;
using Database.Services;
using Newtonsoft.Json;

namespace Game.Modules
{
    public static class PhoneModule
	{
		[Initialize]
		public static void Initialize()
		{
			// Team
			Alt.OnClient<RPPlayer>("Server:Phone:Team:RequestData", RequestTeamData);
			Alt.OnClient<RPPlayer, string>("Server:Phone:Team:Invite", TeamInvitePlayer);

			// Lifeinvader
			Alt.OnClient<RPPlayer>("Server:Phone:Lifeinvader:RequestData", RequestLifeinvaderData);

			// Contacts
			Alt.OnClient<RPPlayer>("Server:Phone:Contacts:RequestData", RequestContacts);
			Alt.OnClient<RPPlayer, int, string, int, bool>("Server:Phone:Contacts:Update", UpdateContact);
			Alt.OnClient<RPPlayer, string, int>("Server:Phone:Contacts:Add", AddContact);
			Alt.OnClient<RPPlayer, int>("Server:Phone:Contacts:Remove", RemoveContact);

			// SMS
			Alt.OnClient<RPPlayer>("Server:Phone:SMS:RequestChats", RequestChats);
			Alt.OnClient<RPPlayer, int>("Server:Phone:SMS:CreateChat", CreateChat);
			Alt.OnClient<RPPlayer, int>("Server:Phone:SMS:DeleteChat", DeleteChat);
			Alt.OnClient<RPPlayer, int>("Server:Phone:SMS:RequestChatHistory", RequestChatHistory);
			Alt.OnClient<RPPlayer, int, string>("Server:Phone:SMS:SendMessage", SendChatMessage);

			// Banking
			Alt.OnClient<RPPlayer>("Server:Phone:Banking:RequestData", RequestBankingData);

			// Dispatch
			Alt.OnClient<RPPlayer, string, int>("Server:Phone:Dispatch:Create", CreateDispatch);
			Alt.OnClient<RPPlayer>("Server:Phone:Dispatch:Close", CloseDispatch);

			// Settings
			Alt.OnClient<RPPlayer>("Server:Phone:Settings:RequestData", RequestSettings);

			// Profile
			Alt.OnClient<RPPlayer>("Server:Phone:Profile:RequestData", RequestProfileData);

			Alt.OnClient<RPPlayer, int>("Server:Phone:StartCall", StartCall);
			Alt.OnClient<RPPlayer>("Server:Phone:AcceptCall", AcceptCall);
			Alt.OnClient<RPPlayer>("Server:Phone:EndCall", EndCall);
			Alt.OnClient<RPPlayer>("Server:Phone:MuteCall", MuteCall);
			Alt.OnClient<RPPlayer>("Server:Phone:Open", Open);
		}

		#region Team

		private static void RequestTeamData(RPPlayer player)
		{
			var data = new List<object>();
			foreach (var user in RPPlayer.All.ToList())
			{
				if (user.TeamId != player.TeamId) continue;

				var userAccount = AccountService.Get(user.DbId);
				if (userAccount == null) continue;

				data.Add(new
				{
					Id = user.DbId,
					Name = user.Name,
					Phone = userAccount.PhoneNumber,
					Rank = userAccount.TeamRank,
					Leader = userAccount.TeamAdmin
				});
			}

			player.EmitBrowser("Phone:Team:SetData", JsonConvert.SerializeObject(data));
		}

		private static void TeamInvitePlayer(RPPlayer player, string targetName)
		{
			var account = AccountService.Get(player.DbId);
			if (account == null || !account.TeamAdmin) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.Name.ToLower() == targetName.ToLower());
			if (target == null)
			{
				player.Notify("Information", $"Der Spieler {targetName} konnte nicht gefunden werden!", NotificationType.ERROR);
				return;
			}

			/*if(target.Level < 8)
			{
				player.Notify("Information", $"Die Person muss mind. Visumstufe 8 erreicht haben!", NotificationType.ERROR);
				return;
			}*/

			if(player.Position.Distance(target.Position) > 15f)
			{
				player.Notify("Information", $"Der Spieler muss in deiner nähe sein!", NotificationType.ERROR);
				return;
			}

			if(target.TeamId > 0)
			{
				player.Notify("Information", $"Der Spieler ist bereits Mitglied einer Fraktion!", NotificationType.ERROR);
				return;
			}

			var team = TeamService.Get(player.TeamId);
			if (team == null) return;

			target.ShowComponent("Input", true, JsonConvert.SerializeObject(new
			{
				Title = "Fraktionseinladung",
				Message = $"Du wurdest von {player.Name} in die Fraktion {team.Name} eingeladen!",
				Type = (int)InputType.CONFIRM,
				CallbackEvent = "Server:Team:AcceptInvite",
				CallbackArgs = new List<object>() { team.Id }
			}));
		}

		#endregion

		#region Lifeinvader

		private static void RequestLifeinvaderData(RPPlayer player)
		{
			player.EmitBrowser("Phone:Lifeinvader:SetData", JsonConvert.SerializeObject(LifeinvaderModule.Posts));
		}

		#endregion

		#region Contacts

		private static void RequestContacts(RPPlayer player)
		{
			var contacts = PhoneService.GetContacts(player.DbId);
			player.EmitBrowser("Phone:Contacts:SetData", JsonConvert.SerializeObject(contacts));
		}

		private static void UpdateContact(RPPlayer player, int contactId, string name, int number, bool fav)
		{
			var contact = PhoneService.GetContact(contactId);
			if (contact == null || contact.AccountId != player.DbId) return;

			contact.Name = name;
			contact.Number = number;
			contact.Favorite = fav;
			PhoneService.UpdateContact(contact);
		}

		private static void AddContact(RPPlayer player, string name, int number)
		{
			if (PhoneService.HasContact(player.DbId, number))
			{
				player.Notify("Information", "Du hast bereits einen Kontakt mit dieser Nummer!", NotificationType.ERROR);
				return;
			}

			var contact = new PhoneContactModel(player.DbId, name, number, false);
			PhoneService.AddContact(contact);
			player.Notify("Information", "Du hast einen Kontakt erstellt!", NotificationType.SUCCESS);
		}

		private static void RemoveContact(RPPlayer player, int contactId)
		{
			var contact = PhoneService.GetContact(contactId);
			if (contact == null || contact.AccountId != player.DbId) return;

			PhoneService.RemoveContact(contact);
		}

		#endregion

		#region SMS

		private static void RequestChats(RPPlayer player)
		{
			var data = new List<object>();
			var chats = PhoneService.GetChats(player.PhoneNumber);
			foreach(var chat in chats)
			{
				var lastMessage = PhoneService.GetChatMessage(chat.LastChatMessageId);
				var targetNumber = player.PhoneNumber == chat.Account1 ? chat.Account2 : chat.Account1;
				var contact = PhoneService.GetContactByNumber(player.DbId, targetNumber);

				data.Add(new
				{
					Id = chat.Id,
					TargetName = contact == null ? "" : contact.Name,
					TargetNumber = targetNumber,
					LastMessage = lastMessage?.Message
				});
			}

			player.EmitBrowser("Phone:SMS:SetChats", JsonConvert.SerializeObject(data));
		}

		private static void CreateChat(RPPlayer player, int number)
		{
			if (PhoneService.GetChat(player.PhoneNumber, number) != null) return;

			var chat = new PhoneChatModel(player.PhoneNumber, number, 0);
			PhoneService.AddChat(chat);
		}

		private static void DeleteChat(RPPlayer player, int number)
		{
			var chat = PhoneService.GetChat(player.PhoneNumber, number);
			if (chat == null) return;

			PhoneService.RemoveChat(chat);
		}

		private static void RequestChatHistory(RPPlayer player, int number)
		{
			var chat = PhoneService.GetChat(player.PhoneNumber, number);
			if (chat == null) return;

			var messages = PhoneService.GetChatMessages(chat.Id);

			player.EmitBrowser("Phone:SMS:SetHistory", JsonConvert.SerializeObject(messages));
		}

		private static void SendChatMessage(RPPlayer player, int number, string message)
		{
			var chat = PhoneService.GetChat(player.PhoneNumber, number);
			if (chat == null) return;

			var msg = new PhoneChatMessageModel(chat.Id, player.PhoneNumber, message, DateTime.Now);
			PhoneService.AddChatMessage(msg);

			chat.LastChatMessageId = msg.Id;
			PhoneService.UpdateChat(chat);

			var target = RPPlayer.All.FirstOrDefault(x => x.PhoneNumber == number);
			if (target == null) return;

			target.EmitBrowser("Phone:SMS:PushMessage", JsonConvert.SerializeObject(msg));
			target.Notify("SMS", "Du hast eine Nachricht erhalten!", NotificationType.INFO);
		}

		#endregion

		#region Banking

		private static void RequestBankingData(RPPlayer player)
		{
			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			var history = BankService.GetHistory(account.Id, TransactionType.PLAYER, 10);

			player.EmitBrowser("Phone:Banking:SetData", JsonConvert.SerializeObject(new
			{
				Money = account.BankMoney,
				History = GetBankingHistoryData(history)
			}));
		}

		private static List<object> GetBankingHistoryData(List<BankHistoryModel> history)
		{
			var result = new List<object>();

			foreach(var historyItem in history)
			{
				result.Add(new
				{
					Withdraw = historyItem.Withdraw,
					Amount = historyItem.Amount,
					Date = historyItem.Date
				});
			}

			return result;
		}

		#endregion

		#region Dispatch

		private static void CreateDispatch(RPPlayer player, string message, int team)
		{
			if(LaptopModule.Dispatches.Any(x => x.CreatorId == player.DbId))
			{
				player.Notify("Information", "Du hast bereits einen Dispatch offen!", NotificationType.ERROR);
				return;
			}

			var dispatch = new Core.Models.Laptop.Dispatch(player.DbId, player.Name, message, player.Position, "", DateTime.Now.ToString("HH:mm"), team);
			LaptopModule.Dispatches.Add(dispatch);
			foreach(var target in RPPlayer.All.ToList())
			{
				if (!LaptopModule.HasAccessToDispatch(dispatch, target.TeamId)) continue;

				target.Notify("Dispatch", $"Es ist ein Dispatch von {player.Name} eingegangen!", NotificationType.INFO);
			}
		}


		private static void CloseDispatch(RPPlayer player)
		{
			var dispatch = LaptopModule.Dispatches.FirstOrDefault(x => x.CreatorId == player.DbId);
			if (dispatch == null)
			{
				player.Notify("Information", "Du hast keinen Dispatch offen!", NotificationType.INFO);
				return;
			}

			LaptopModule.Dispatches.Remove(dispatch);
			player.Notify("Information", "Du hast deinen Dispatch abgebrochen!", NotificationType.INFO);

            foreach (var target in RPPlayer.All.ToList())
            {
                if (!LaptopModule.HasAccessToDispatch(dispatch, target.TeamId)) continue;

                target.Notify("Dispatch", $"{player.Name} hat seinen Dispatch abgebrochen!", NotificationType.INFO);
            }
        }

		#endregion

		#region Settings

		private static void RequestSettings(RPPlayer player)
		{
			player.Emit("Client:Hud:Phone:SetData");
		}

		#endregion

		#region Profile

		private static void RequestProfileData(RPPlayer player)
		{
			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			var team = TeamService.Get(player.TeamId);

			player.EmitBrowser("Phone:Profile:SetData", player.Name, account.Level, account.PhoneNumber, account.Created.ToString("dd.MM.yyyy"), (team == null ? "Zivilisten" : team.ShortName), "Kein Business");
		}

		#endregion

		private static void StartCall(RPPlayer player, int number)
		{
			if(player.PhoneNumber == number)
			{
				player.Notify("Information", "Du kannst dich nicht selbst anrufen!", NotificationType.ERROR);
				return;
			}

			var target = RPPlayer.All.FirstOrDefault(x => x.LoggedIn && x.PhoneNumber == number);
			if (target == null || !target.Phone)
			{
				player.Notify("Information", "Die angegebene Rufnummer ist aktuell nicht erreichbar!", NotificationType.ERROR);
				return;
			}

			var playerContact = PhoneService.GetContactByNumber(player.DbId, target.PhoneNumber);
			var targetContact = PhoneService.GetContactByNumber(target.DbId, player.PhoneNumber);

			var now = DateTime.Now;

			player.CallPartner = target.DbId;
			player.CallState = CallState.CALLING;
			player.CallStarted = now;
			player.CallMute = false;
			player.EmitBrowser("Phone:ShowCallScreen", true, JsonConvert.SerializeObject(new
			{
				State = (int)CallState.CALLING,
				Started = now,
				Partner = target.PhoneNumber,
				PartnerName = playerContact == null ? "" : playerContact.Name,
				Mute = player.CallMute
			}));
			player.EmitBrowser("Phone:PlayRinging", 100);

			target.CallPartner = player.DbId;
			target.CallState = CallState.GET_CALLED;
			target.CallStarted = now;
			target.CallMute = false;
			target.EmitBrowser("Phone:ShowCallScreen", true, JsonConvert.SerializeObject(new
			{
				State = (int)CallState.GET_CALLED,
				Started = now.ToString("o"),
				Partner = player.PhoneNumber,
				PartnerName = targetContact == null ? "" : targetContact.Name
			}));
			target.EmitBrowser("Phone:PlayRingtone", 100);
			target.Notify("Information", $"Eingehender Anruf", NotificationType.INFO);
		}

		private static void AcceptCall(RPPlayer player)
		{
			if (player.CallPartner < 1) return;

			var partner = RPPlayer.All.FirstOrDefault(x => x.DbId == player.CallPartner);
			if (partner == null) return;

			player.CallState = CallState.ACTIVE_CALL;
			player.CallStarted = DateTime.Now;
			player.EmitBrowser("Phone:UpdateCallState", (int)player.CallState, player.CallMute);
			player.EmitBrowser("Phone:StopRingtone");

			partner.CallState = CallState.ACTIVE_CALL;
			partner.CallStarted = DateTime.Now;
			partner.EmitBrowser("Phone:UpdateCallState", (int)partner.CallState, partner.CallMute);
			partner.EmitBrowser("Phone:StopRingtone");

			VoiceModule.CallPlayer(player, partner, true);
		}

		private static void EndCall(RPPlayer player)
		{
			if (player.CallPartner < 1) return;

			var partner = RPPlayer.All.FirstOrDefault(x => x.DbId == player.CallPartner);
			if (partner == null) return;

			player.CallPartner = 0;
			player.CallState = CallState.NONE;
			player.CallStarted = DateTime.Now;
			player.CallMute = false;
			player.EmitBrowser("Phone:ShowCallScreen", false);
			player.EmitBrowser("Phone:StopRingtone");

			partner.CallPartner = 0;
			partner.CallState = CallState.NONE;
			partner.CallStarted = DateTime.Now;
			partner.CallMute = false;
			partner.EmitBrowser("Phone:ShowCallScreen", false);
			partner.EmitBrowser("Phone:StopRingtone");

			VoiceModule.CallPlayer(player, partner, false);
		}

		private static void MuteCall(RPPlayer player)
		{
			if(player.CallState == CallState.NONE) return;

			player.CallMute = !player.CallMute;
			player.EmitBrowser("Phone:UpdateCallState", (int)player.CallState, player.CallMute);

			VoiceModule.MuteOnPhone(player, player.CallMute);
		}

		private static void Open(RPPlayer player)
		{
			if (!player.Phone) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			var callPartner = RPPlayer.All.FirstOrDefault(x => x.DbId == player.CallPartner);

			player.PlayAnimation(AnimationType.PHONE);
			player.ShowComponent("Phone", true, JsonConvert.SerializeObject(new
			{
				Background = account.PhoneBackground,
				Team = account.TeamId,
				TeamLeader = account.TeamAdmin,
				Business = 0,
				BusinessLeader = false,

				CallActive = player.CallPartner > 0,
				CallState = (int)player.CallState,
				CallStarted = player.CallStarted.ToString("o"),
				CallPartner = callPartner?.PhoneNumber,
				CallPartnerName = callPartner?.Name
			}));
		}
	}
}