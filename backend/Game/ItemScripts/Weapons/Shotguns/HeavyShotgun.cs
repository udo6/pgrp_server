namespace Game.ItemScripts.Weapons.Shotguns
{
	public class HeavyShotgun : WeaponItemScript
	{
		public HeavyShotgun() : base(143, 984333226, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SHOTGUN)
		{
		}
	}

	public class HeavyShotgunAmmo : AmmoItemScript
	{
		public HeavyShotgunAmmo() : base(144, 984333226, 6)
		{
		}
	}
}