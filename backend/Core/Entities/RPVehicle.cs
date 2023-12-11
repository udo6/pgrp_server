using AltV.Net;
using AltV.Net.Elements.Entities;
using Core.Enums;

namespace Core.Entities
{
	public class RPVehicle : Vehicle
	{
		public static readonly List<RPVehicle> All = new();

		public int DbId { get; set; }
		public int OwnerId { get; set; }
		public int KeyHolderId { get; set; }
		public int TuningId { get; set; }
		public int TrunkId { get; set; }
		public int GloveBoxId { get; set; }
		public int BaseId { get; set; }
		public int PositionId { get; set; }
		public OwnerType OwnerType { get; set; }
		public GarageType GarageType { get; set; }
		public float Fuel { get; set; }

		public bool Locked { get; set; }
		public bool TrunkLocked { get; set; }
		public bool Engine { get; set; }
		public DateTime LastUse { get; set; }
		public bool SirenSoundActive { get; private set; }

		public bool Gangwar { get; set; }

		public RPVehicle(ICore core, nint nativePointer, uint id) : base(core, nativePointer, id)
		{
			All.Add(this);
			SetStreamSyncedMetaData("SIREN_SOUND", false);
			SetEngineState(false);
			SetLockState(true);

			ModKit = 1;
		}

		public void SetFuel(float value)
		{
			Fuel = value;
			SetStreamSyncedMetaData("FUEL", value);
		}

		public void SetMaxFuel(float value)
		{
			SetStreamSyncedMetaData("MAX_FUEL", value);
		}

		public void SetEngineState(bool state)
		{
			Engine = state;
			EngineOn = state;
			SetStreamSyncedMetaData("ENGINE", state);
		}

		public void SetLockState(bool state)
		{
			Locked = state;
			LockState = state ? AltV.Net.Enums.VehicleLockState.Locked : AltV.Net.Enums.VehicleLockState.Unlocked;
			SetStreamSyncedMetaData("LOCKED", state);
			TrunkLocked = true;
			SetDoorState(5, 0);
		}

		public void SetTrunkState(bool state)
		{
			TrunkLocked = state;
			SetDoorState(5, (byte)(state ? 0 : 7));
		}

		public void SetSirenSoundState(bool state)
		{
			SirenSoundActive = state;
			SetStreamSyncedMetaData("SIREN_SOUND", state);
		}

		public void Delete()
		{
			All.Remove(this);
			Destroy();
		}
	}
}