namespace Game.ItemScripts.Federal.Weapons.Pistol
{
	public class Pistol : WeaponItemScript
	{
		public Pistol() : base(191, 453432689, true, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.PISTOL)
		{
		}
	}

	public class PistolAmmo : AmmoItemScript
	{
		public PistolAmmo() : base(225, 453432689, 12, true)
		{
		}
	}
}