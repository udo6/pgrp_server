namespace Game.ItemScripts.Weapons.Pistol
{
	public class APPistol : WeaponItemScript
	{
		public APPistol() : base(61, 584646201, false, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.PISTOL)
		{
		}
	}

	public class APPistolAmmo : AmmoItemScript
	{
		public APPistolAmmo() : base(94, 584646201, 18)
		{
		}
	}
}