namespace Game.ItemScripts.Weapons.Pistol
{
	public class Heavypistol : WeaponItemScript
	{
		public Heavypistol() : base(74, 3523564046, false, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.PISTOL)
		{
		}
	}

	public class HeavypistolAmmo : AmmoItemScript
	{
		public HeavypistolAmmo() : base(97, 3523564046, 18)
		{
		}
	}
}