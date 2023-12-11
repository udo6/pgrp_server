namespace Game.ItemScripts.Federal.Weapons.Pistol
{
	public class RevolverMk2 : WeaponItemScript
	{
		public RevolverMk2() : base(195, 3415619887, true, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.PISTOL)
		{
		}
	}

	public class RevolverMk2Ammo : AmmoItemScript
	{
		public RevolverMk2Ammo() : base(229, 3415619887, 6, true)
		{
		}
	}
}