namespace Game.ItemScripts.Weapons.Pistol
{
	public class Pistol : WeaponItemScript
	{
		public Pistol() : base(65, 453432689, false, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.PISTOL)
		{
		}
	}

	public class PistolAmmo : AmmoItemScript
	{
		public PistolAmmo() : base(99, 453432689, 12)
		{
		}
	}
}