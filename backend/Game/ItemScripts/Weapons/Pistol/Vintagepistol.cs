namespace Game.ItemScripts.Weapons.Pistol
{
	public class Vintagepistol : WeaponItemScript
	{
		public Vintagepistol() : base(73, 137902532, false, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.PISTOL)
		{
		}
	}

	public class VintagepistolAmmo : AmmoItemScript
	{
		public VintagepistolAmmo() : base(106, 137902532, 7)
		{
		}
	}
}