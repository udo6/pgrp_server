namespace Game.ItemScripts.Weapons.Pistol
{
	public class RevolverMk2 : WeaponItemScript
	{
		public RevolverMk2() : base(69, 3415619887, false, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.PISTOL)
		{
		}
	}

	public class RevolverMk2Ammo : AmmoItemScript
	{
		public RevolverMk2Ammo() : base(103, 3415619887, 6)
		{
		}
	}
}