namespace Game.ItemScripts.Weapons.Shotguns
{
	public class PumpgunMk2 : WeaponItemScript
	{
		public PumpgunMk2() : base(135, 1432025498, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SHOTGUN)
		{
		}
	}

	public class PumpgunMk2Ammo : AmmoItemScript
	{
		public PumpgunMk2Ammo() : base(136, 1432025498, 8)
		{
		}
	}
}