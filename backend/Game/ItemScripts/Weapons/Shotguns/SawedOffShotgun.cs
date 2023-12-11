namespace Game.ItemScripts.Weapons.Shotguns
{
	public class SawedOffShotgun : WeaponItemScript
	{
		public SawedOffShotgun() : base(138, 2017895192, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SHOTGUN)
		{
		}
	}

	public class SawedOffShotgunAmmo : AmmoItemScript
	{
		public SawedOffShotgunAmmo() : base(137, 2017895192, 8)
		{
		}
	}
}