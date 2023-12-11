namespace Game.ItemScripts.Federal.Weapons.Shotguns
{
	public class Pumpgun : WeaponItemScript
	{
		public Pumpgun() : base(259, 487013001, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SHOTGUN)
		{
		}
	}

	public class PumpgunAmmo : AmmoItemScript
	{
		public PumpgunAmmo() : base(260, 487013001, 8, true)
		{
		}
	}
}