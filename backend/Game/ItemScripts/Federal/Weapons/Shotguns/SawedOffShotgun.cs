namespace Game.ItemScripts.Federal.Weapons.Shotguns
{
	public class SawedOffShotgun : WeaponItemScript
	{
		public SawedOffShotgun() : base(263, 2017895192, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SHOTGUN)
		{
		}
	}

	public class SawedOffShotgunAmmo : AmmoItemScript
	{
		public SawedOffShotgunAmmo() : base(264, 2017895192, 8, true)
		{
		}
	}
}