using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Core.Enums;
using Core.Models.NativeMenu;
using Core.Models.Player;
using Logs.Models;
using Newtonsoft.Json;
using System.Numerics;

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
		public bool DamageCap { get; set; }
		public bool AdminNotifications { get; set; } = true;

		public int CallPartner { get; set; }
		public CallState CallState { get; set; }
		public DateTime CallStarted { get; set; }
		public bool CallMute { get; set; }

		public List<WeaponModel> Weapons { get; set; }

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

		// ANTICHEAT
		public DateTime LastHealthChange { get; set; }
		public DateTime LastGodmodeChange { get; set; }
		public DateTime LastPositionChange { get; set; }

		public int AllowedHealth { get; set; } = 200;
		public int ExplosionsCaused { get; set; }

		public int KillerId { get; set; }
		public uint KillerWeapon { get; set; }
		public DateTime KillerDate { get; set; }

		public float VoiceRange { get; set; }
		public bool VoiceFirstConnect { get; set; }
		public int MaxVoiceRange { get; set; }
		public bool ForceMuted { get; set; }
		public string TeamspeakName { get; set; }

		public int VoicePluginClientId { get; set; }
		public bool VoicePluginForceMuted { get; set; }
		public float VoicePluginRange { get; set; }

		public bool RadioActive { get; set; }
		public int RadioFrequency { get; set; }
		public bool RadioMute { get; set; }
		public bool RadioTalking { get; set; }

		public List<IBlip> TemporaryBlips { get; set; }

		public (uint, uint) TemporaryTattoo { get; set; }

        // JOBS
        public bool IsInGarbageJob { get; set; } = false;
        public bool HasGarbageInHand { get; set; } = false;

        public bool IsInGardenerJob { get; set; } = false;
        public bool IsInMoneyTruckJob { get; set; } = false;
        public bool HasMoneyInHand { get; set; } = false;

        // JOB VEHICLES
        public RPVehicle? JobVehicle = null;

		// CLOTHES
		public bool HatState { get; set; } = true;
		public bool MaskState { get; set; } = true;
		public bool GlassesState { get; set; } = true;
		public bool EarsState { get; set; } = true;
		public bool TopState { get; set; } = true;
		public bool UndershirtState { get; set; } = true;
		public bool WatchState { get; set; } = true;
		public bool BraceletState { get; set; } = true;
		public bool PantsState { get; set; } = true;
		public bool ShoesState { get; set; } = true;
		public bool AccessoriesState { get; set; } = true;
		public bool DecalsState { get; set; } = true;

		public bool IsInJob()
        {
            return (IsInGarbageJob || IsInGardenerJob || IsInMoneyTruckJob);
        }

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
			TemporaryBlips = new();
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

		public void AddWeapon(uint weapon, int ammo, bool equip, byte tintIndex, List<uint> components)
		{
			Emit("Client:PlayerModule:AddWeapon", weapon);
			Weapons.Add(new(weapon, tintIndex, components));
			GiveWeapon(weapon, ammo, equip);
		}

		public void AddAttatchment(uint hash, uint attatchment)
		{
			var weapon = Weapons.FirstOrDefault(x => x.Hash == hash);
			if (weapon == null) return;

			weapon.Components.Add(attatchment);
			AddWeaponComponent(weapon.Hash, attatchment);
		}

		public void RemoveAttatchment(uint hash, uint attatchment)
		{
			var weapon = Weapons.FirstOrDefault(x => x.Hash == hash);
			if (weapon == null) return;

			weapon.Components.Remove(attatchment);
			RemoveWeaponComponent(weapon.Hash, attatchment);
		}

		public void SetObjectProp(string propName, int boneIndex, double posX, double posY, double posZ, double rotX, double rotY, double rotZ)
        {
            Emit("Client:PropSyncModule:AddProp", propName, boneIndex, posX, posY, posZ, rotX, rotY, rotZ);

            var data = new
            {
				propName,
				bone = boneIndex,
				posX,
				posY,
				posZ,
				rotX,
				rotY,
				rotZ
            };

            SetSyncedMetaData("Player:PropSyncModule:Prop", JsonConvert.SerializeObject(data));
        }

		public void DeleteWeapon(uint hash)
		{
			var weapon = Weapons.FirstOrDefault(x => x.Hash == hash);
			if (weapon == null) return;

			RemoveWeapon(weapon.Hash);
			Emit("Client:PlayerModule:RemoveWeapon", weapon.Hash);
			Weapons.Remove(weapon);
		}

		public void SetInvincible(bool state)
		{
			LastGodmodeChange = DateTime.Now;
			Invincible = state;
			Emit("Client:AnticheatModule:SetGodmode", state);
			SetStreamSyncedMetaData("GODMODE", state);
		}

		public void SetPosition(Position position)
		{
			LastPositionChange = DateTime.Now;
			Position = position;
			Emit("Client:AnticheatModule:SetPosition", position);
		}

		public void SetHealth(ushort health)
		{
			LastHealthChange = DateTime.Now;
			Health = health;
			AllowedHealth = Health + Armor;
			Emit("Client:AnticheatModule:SetHealth", AllowedHealth);
		}

		public void SetArmor(ushort armor)
		{
			LastHealthChange = DateTime.Now;
			Armor = armor;
			AllowedHealth = Health + Armor;
			Emit("Client:AnticheatModule:SetHealth", AllowedHealth);
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