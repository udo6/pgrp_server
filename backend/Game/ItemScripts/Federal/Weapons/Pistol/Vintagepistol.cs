namespace Game.ItemScripts.Federal.Weapons.Pistol
{
	public class Vintagepistol : WeaponItemScript
	{
		public Vintagepistol() : base(199, 137902532, true, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.PISTOL)
		{
		}
	}

	public class VintagepistolAmmo : AmmoItemScript
	{
		public VintagepistolAmmo() : base(232, 137902532, 7, true)
		{
		}
	}
}