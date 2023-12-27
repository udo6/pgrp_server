using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Newtonsoft.Json;

namespace Game.Modules
{
	public static class VoiceModule
	{
		private static Random Random = new Random();

		private static List<string> Names = new();
		private static List<RPShape> VoiceRangeShapes = new();
		private static Dictionary<int, List<RPPlayer>> RadioFrequencies = new();

		[Initialize]
		public static void Initialize()
		{
			Alt.OnClient<RPPlayer, bool>("server:yaca:enableRadio", EnableRadio);
			Alt.OnClient<RPPlayer, bool>("server:yaca:radioTalkingState", RadioTalkingState);
			Alt.OnClient<RPPlayer, float>("server:yaca:changeVoiceRange", ChangeVoiceRange);
			Alt.OnClient<RPPlayer, bool>("server:yaca:lipsync", Lipsync);
			Alt.OnClient<RPPlayer, int>("server:yaca:addPlayer", AddNewPlayer);
			Alt.OnClient<RPPlayer>("server:yaca:noVoicePlugin", OnPlayerNoVoice);
			Alt.OnClient<RPPlayer, bool>("server:yaca:wsReady", OnPlayerReconnect);
		}

		private static void Lipsync(RPPlayer player, bool state)
		{
			player.SetStreamSyncedMetaData("yaca:lipsync", state);
		}

		public static void ConnectToVoice(RPPlayer player)
		{
			var name = "";
			bool nameFound = false;
			for(var i = 0; i < 100; i++)
			{
				name = GenerateRandomName();
				if(!Names.Any(x => x == name))
				{
					Names.Add(name);
					nameFound = true;
					break;
				}
			}

			if (!nameFound)
			{
				player.Kick("Es ist ein Fehler aufgetreten! Bitte starte dein Spiel neu.");
				return;
			}

			player.VoiceRange = 3;
			player.MaxVoiceRange = 15;
			player.VoiceFirstConnect = false;
			player.ForceMuted = false;
			player.TeamspeakName = name;

			player.RadioActive = false;
			player.RadioFrequency = 0;

			Connect(player);
		}

		public static void OnPlayerDisconnect(RPPlayer player)
		{
			Names.Remove(player.TeamspeakName);

			foreach(var radio in RadioFrequencies)
			{
				radio.Value.Remove(player);
				if (radio.Value.Count <= 0) RadioFrequencies.Remove(radio.Key);
			}
		}

		public static void OnPlayerColshape(RPPlayer player, RPShape shape, bool entered)
		{
			// idk what this does
		}

		public static void OnPlayerAliveChange(RPPlayer player, bool state)
		{
			if (player.Alive != state) return;

			player.ForceMuted = !state;
			player.VoicePluginForceMuted = !state;
			Alt.EmitAllClients("client:yaca:muteTarget", player.Id, !state);
		}

		public static void OnPlayerNoVoice(RPPlayer player)
		{
			player.Kick("Du benötigst das YACA Voiceplugin!");
		}

		public static void OnPlayerReconnect(RPPlayer player, bool isFirstConnect)
		{
			if (!player.VoiceFirstConnect) return;

			if (!isFirstConnect)
			{
				var name = "";
				for (var i = 0; i < 100; i++)
				{
					name = GenerateRandomName();
					if (!Names.Any(x => x == name))
					{
						Names.Add(name);
						break;
					}
				}

				Names.Remove(player.TeamspeakName);
				player.TeamspeakName = name;
			}

			Connect(player);
		}

		public static void ChangeVoiceRange(RPPlayer player, float range)
		{
			if (player.MaxVoiceRange < range)
			{
				player.Emit("client:yaca:setMaxVoiceRange", 15);
				return;
			}

			player.VoiceRange = range;
			player.VoicePluginRange = range;
			Alt.EmitAllClients("client:yaca:changeVoiceRange", player.Id, range);
		}

		public static void Connect(RPPlayer player)
		{
			player.VoiceFirstConnect = true;

			player.Emit("client:yaca:init", JsonConvert.SerializeObject(new
			{
				suid = "kcFLSO0CrIupmxWtnRaMQbDdae8=",
				chid = 9986,
				deChid = 9790,
				channelPassword = "gmkojdrjgrJNAJUWEJNF8837853",
				ingameName = player.TeamspeakName,
				useWhisper = false
			}));
		}

		public static void AddNewPlayer(RPPlayer player, int clientId)
		{
			player.VoicePluginClientId = clientId;
			player.VoicePluginForceMuted = player.ForceMuted;
			player.VoicePluginRange = player.VoiceRange;

			Alt.EmitAllClients("client:yaca:addPlayers", JsonConvert.SerializeObject(new
			{
				clientId = clientId,
				forceMuted = player.ForceMuted,
				range = player.VoiceRange,
				playerId = player.Id
			}));

			var allPlayersData = new List<object>();
			foreach(var target in RPPlayer.All.ToList())
			{
				if (player.Id == target.Id) continue;

				allPlayersData.Add(new
				{
					clientId = target.VoicePluginClientId,
					forceMuted = target.ForceMuted,
					range = target.VoiceRange,
					playerId = target.Id
				});
			}

			player.Emit("client:yaca:addPlayers", JsonConvert.SerializeObject(allPlayersData));
		}

		public static void EnableRadio(RPPlayer player, bool state)
		{
			player.RadioActive = state;
			player.Emit("client:yaca:setRadioEnabled", state);
		}

		public static void ChangeRadioFrequency(RPPlayer player, int frequency)
		{
			if (!player.RadioActive) return;

			if(frequency <= 0)
			{
				LeaveRadio(player);
				return;
			}

			if (player.RadioFrequency != frequency)
			{
				LeaveRadio(player);
			}

			if (!RadioFrequencies.ContainsKey(frequency)) RadioFrequencies[frequency] = new();
			RadioFrequencies[frequency].Add(player);

			player.RadioFrequency = frequency;
			player.Emit("client:yaca:setRadioFreq", frequency);
			player.SetSyncedMetaData("RADIO_CHANNEL", frequency);

			foreach(var target in RPPlayer.All.ToList())
			{
				if (target.RadioFrequency != frequency || !target.RadioTalking) continue;

				player.Emit("client:yaca:radioTalking", target.Id, frequency, true);
			}
		}

		public static void LeaveRadio(RPPlayer player)
		{
			player.RadioFrequency = 0;

			var players = new List<RPPlayer>();
			foreach(var freq in RadioFrequencies)
			{
				players.AddRange(freq.Value);
			}

			Alt.EmitClients(players.ToArray(), "client:yaca:leaveRadioChannel", player.VoicePluginClientId);
		}

		public static void RadioMute(RPPlayer player)
		{
			player.RadioMute = !player.RadioMute;
			player.Emit("client:yaca:setRadioMuteState", player.RadioMute);
		}

		public static void RadioTalkingState(RPPlayer player, bool state)
		{
			if (!player.RadioActive || player.RadioFrequency <= 0 || !RadioFrequencies.ContainsKey(player.RadioFrequency)) return;

			player.RadioTalking = state;

			var players = RadioFrequencies[player.RadioFrequency].ToList();
			var targets = new List<RPPlayer>();
			var targetsToSender = new List<RPPlayer>();
			foreach(var target in players)
			{
				if (target.RadioMute)
				{
					if(target.Id == player.Id)
					{
						targets = new();
						break;
					}

					continue;
				}

				if (target.Id == player.Id || !target.RadioActive) continue;

				targets.Add(target);
				targetsToSender.Add(target);
			}

			Alt.EmitClients(targets.ToArray(), "client:yaca:radioTalking", player.Id, player.RadioFrequency, state);
		}

		public static void CallPlayer(RPPlayer player, RPPlayer target, bool state)
		{
			player.Emit("client:yaca:phone", target.Id, state);
			player.SetSyncedMetaData("CALL_PARTNER", state ? target.Id : -1);
			target.Emit("client:yaca:phone", player.Id, state);
			target.SetSyncedMetaData("CALL_PARTNER", state ? player.Id : -1);
		}

		public static void MuteOnPhone(RPPlayer player, bool state)
		{
			if (state)
			{
				player.SetSyncedMetaData("yaca:isMutedOnPhone", true);
			}
			else
			{
				player.DeleteSyncedMetaData("yaca:isMutedOnPhone");
			}
		}

		public static void EnablePhoneSpeaker(RPPlayer player, bool state, List<int> phoneCallMemberIds)
		{
			// soon
		}

		private static string GenerateRandomName()
		{
			var length = 15;
			var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";
			var result = "";

			for (int i = 0; i < length; i++)
			{
				var random = Random.Next(0, chars.Length);
				result += chars[random];
			}

			return result;
		}
	}
}