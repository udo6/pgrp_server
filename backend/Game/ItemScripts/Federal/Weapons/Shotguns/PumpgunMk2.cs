namespace Game.ItemScripts.Federal.Weapons.Shotguns
{
	public class PumpgunMk2 : WeaponItemScript
	{
		public PumpgunMk2() : base(261, 1432025498, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SHOTGUN)
		{
		}
	}

	public class PumpgunMk2Ammo : AmmoItemScript
	{
		public PumpgunMk2Ammo() : base(262, 1432025498, 8, true)
		{
		}
	}
}