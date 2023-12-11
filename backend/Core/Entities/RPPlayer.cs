﻿using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Core.Enums;
using Core.Models.NativeMenu;
using Newtonsoft.Json;

namespace Core.Entities
{
	public class RPPlayer : Player
	{
		public static readonly List<RPPlayer> All = new();

		public int DbId { get; set; }
		public int CustomizationId { get; set; }
		public int ClothesId { get; set; }
		public int TempClothesId { get; set; }
		public int InventoryId { get; set; }
		public int LaboratoryInputInventoryId { get; set; }
		public int LaboratoryOutputInventoryId { get; set; }
		public int LockerInventoryId { get; set; }
		public int PositionId { get; set; }
		public int TeamId { get; set; }
		public int BusinessId { get; set; }
		public int LicenseId { get; set; }
		public int VestItemId { get; set; }

		public int PhoneNumber { get; set; }
		public AdminRank AdminRank { get; set; }
		public DateTime DeathTime { get; set; }
		public InjuryType InjuryType { get; set; }
		public bool LoggedIn { get; set; }
		public DateTime ComaTime { get; set; }
		public bool AdminDuty { get; set; }
		public int Hunger { get; set; }
		public int Thirst { get; set; }
		public bool HasBeenSearched { get; set; }
		public bool Alive { get; set; }
		public bool Coma { get; set; }
		public bool Stabilized { get; set; }
		public bool LabRunning { get; set; }
		public int CurrentGangwarId { get; set; }
		public bool IsGangwar => CurrentGangwarId > 0;
		public int GangwarWeapon { get; set; }
		public bool SWATDuty { get; set; }
		public DateTime Joined { get; set; }
		public DateTime LastLifeinvaderPost { get; set; }
		public int Level { get; set; }
		public int Xp { get; set; }
		public int XpTicks { get; set; }
		public bool TeamDuty { get; set; }

		public int CallPartner { get; set; }
		public CallState CallState { get; set; }
		public DateTime CallStarted { get; set; }
		public bool CallMute { get; set; }

		public List<uint> Weapons { get; set; }

		public bool Phone { get; set; }
		public bool Laptop { get; set; }

		public int Jailtime { get; set; }

		public bool IsInHospital { get; set; }

		public int FFAId { get; set; }
		public bool IsInFFA => FFAId > 0;

		public DateTime LastLocatedByFIB { get; set; }
		public int FarmingSpotId { get; set; }
		public bool IsFarming { get; set; }
		public RPShape? FarmingShape { get; set; }

		private CancellationTokenSource InteractionCancelToken { get; set; }
		private Task? InteractionTask { get; set; }
		private Action? OnInteractionCancel { get; set; }

		public bool Interaction => InteractionTask != null;
		public bool InInterior { get; set; }
		public Position OutsideInteriorPosition { get; set; }

		private string ClientInteraction { get; set; } = string.Empty;
		private string ClientCachedInteraction { get; set; } = string.Empty;

		public RPPlayer(ICore core, nint nativePointer, uint id) : base(core, nativePointer, id)
		{
			All.Add(this);
			DbId = -1;
			Alive = true;
			DeathTime = DateTime.Now;
			ComaTime = DateTime.Now;
			InteractionCancelToken = new();
			GangwarWeapon = -1;
			Joined = DateTime.Now;
			Weapons = new();
			LastLocatedByFIB = DateTime.Now;
		}

		public void Notify(string title, string msg, NotificationType type)
		{
			Emit("Client:Hud:PushNotification", JsonConvert.SerializeObject(new { Title = title, Message = msg, Type = type, Duration = 3500 }));
		}

		public void EmitBrowser(string eventName, params object[] args)
		{
			Emit("Client:BrowserModule:CallEvent", eventName, args);
		}

		public void ShowComponent(string comp, bool state, string data = "")
		{
			Emit("Client:BrowserModule:ShowComponent", comp, state, data);
		}

		public void StartInteraction(Action action, int duration, Action? onCancel = null)
		{
			Emit("Client:Hud:SetProgressbarState", true, duration);
			ClientCachedInteraction = ClientInteraction;
			SetInteraction("KEY_E", "INTERACTION");
			OnInteractionCancel = onCancel;

			InteractionTask = Task.Run(async () =>
			{
				await Task.Delay(duration, InteractionCancelToken.Token);

				action();
				StopInteraction(false);
			}, InteractionCancelToken.Token);
		}

		public void StopInteraction(bool cancelled = true)
		{
			if (!Interaction) return;

			if(cancelled && OnInteractionCancel != null)
			{
				OnInteractionCancel.Invoke();
			}

			InteractionCancelToken.Cancel();
			InteractionCancelToken = new();
			InteractionTask = null;
			StopAnimation();
			SetInteraction("KEY_E", ClientCachedInteraction);
			Emit("Client:Hud:SetProgressbarState", false);
		}

		public void SetInteraction(string key, string value)
		{
			ClientInteraction = value;
			Emit("Client:KeyHandler:SetInteraction", key, value);
		}

		public void AddAmmo(uint weapon, int amount)
		{
			Emit("Client:PlayerModule:AddAmmo", weapon, amount);
		}

		public void SetAmmo(uint weapon, int amount)
		{
			Emit("Client:PlayerModule:SetAmmo", weapon, amount);
		}

		public void SetFood(int hunger, int thirst, int strength)
		{
			EmitBrowser("Hud:SetFood", hunger, thirst, strength);
			Emit("Client:PlayerModule:SetRunSpeedMultiplier", Math.Clamp(strength, 80, 110) / 100);
		}

		public void PlayAnimation(AnimationType type)
		{
			Emit("Client:Animation:Play", (int)type);
		}

		public void PlayAnimation(string dict, string name, int flags)
		{
			Emit("Client:Animation:Play2", dict, name, flags);
		}

		public void StopAnimation()
		{
			Emit("Client:Animation:ClearTasks");
		}

		public void SetClothing(int slot, int drawable, int texture, uint dlc)
		{
			if (dlc == 0) SetClothes((byte)slot, (ushort)drawable, (byte)texture, 0);
			else SetDlcClothes((byte)slot, (byte)drawable, (byte)texture, 0, dlc);
		}

		public void SetProp(int slot, int drawable, int texture, uint dlc)
		{
			if(drawable == -1)
			{
				ClearProps((byte)slot);
				return;
			}

			if (dlc == 0) SetProps((byte)slot, (byte)drawable, (byte)texture);
			else SetDlcProps((byte)slot, (byte)drawable, (byte)texture, dlc);
		}

		public void ShowNativeMenu(bool state, NativeMenu menu)
		{
			Emit("Client:Hud:ShowNativeMenu", state, JsonConvert.SerializeObject(menu));
		}

		public void SetDimension(int dimension)
		{
			Emit("Client:PlayerModule:SetDimension", dimension);
			Dimension = dimension;
		}

		public void ApplyTeam()
		{
			Emit("Client:PlayerModule:SetTeam", TeamId);
		}

		public void ApplyAdmin()
		{
			Emit("Client:PlayerModule:SetAdmin", (int)AdminRank);
		}

		public void AddWeapon(uint weapon, int ammo, bool equip)
		{
			Emit("Client:PlayerModule:AddWeapon", weapon);
			Weapons.Add(weapon);
			GiveWeapon(weapon, ammo, equip);
		}

		public void DeleteWeapon(uint weapon)
		{
			RemoveWeapon(weapon);
			Emit("Client:PlayerModule:RemoveWeapon", weapon);
			Weapons.Remove(weapon);
		}

		public void SetInvincible(bool state)
		{
			Invincible = state;
			Emit("Client:AnticheatModule:SetGodmode", state);
			SetStreamSyncedMetaData("GODMODE", state);
		}

		public void SetPosition(Position position)
		{
			Position = position;
			Emit("Client:AnticheatModule:SetPosition", position);
		}

		public void SetHealth(ushort health)
		{
			Health = health;
			Emit("Client:AnticheatModule:SetHealth", Health + Armor);
		}

		public void SetArmor(ushort armor)
		{
			Armor = armor;
			Emit("Client:AnticheatModule:SetHealth", Health + Armor);
		}

		public void UpdateVoiceHud()
		{
			EmitBrowser("Hud:UpdateVoice", true, 0, 0);
		}

		public void LoadIPL(string ipl)
		{
			Emit("LoadIPL", ipl);
		}

		public void UnloadIPL(string ipl)
		{
			Emit("UnloadIPL", ipl);
		}
	}
}