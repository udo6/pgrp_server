namespace Game.ItemScripts.Federal.Weapons.Pistol
{
	public class Heavypistol : WeaponItemScript
	{
		public Heavypistol() : base(200, 3523564046, true, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.PISTOL)
		{
		}
	}

	public class HeavypistolAmmo : AmmoItemScript
	{
		public HeavypistolAmmo() : base(223, 3523564046, 18, true)
		{
		}
	}
}