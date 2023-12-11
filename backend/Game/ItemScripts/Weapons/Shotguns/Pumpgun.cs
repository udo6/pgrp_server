namespace Game.ItemScripts.Weapons.Shotguns
{
	public class Pumpgun : WeaponItemScript
	{
		public Pumpgun() : base(133, 487013001, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SHOTGUN)
		{
		}
	}

	public class PumpgunAmmo : AmmoItemScript
	{
		public PumpgunAmmo() : base(134, 487013001, 8)
		{
		}
	}
}