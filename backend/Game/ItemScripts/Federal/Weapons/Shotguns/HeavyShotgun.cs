namespace Game.ItemScripts.Federal.Weapons.Shotguns
{
	public class HeavyShotgun : WeaponItemScript
	{
		public HeavyShotgun() : base(269, 984333226, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SHOTGUN)
		{
		}
	}

	public class HeavyShotgunAmmo : AmmoItemScript
	{
		public HeavyShotgunAmmo() : base(270, 984333226, 6, true)
		{
		}
	}
}