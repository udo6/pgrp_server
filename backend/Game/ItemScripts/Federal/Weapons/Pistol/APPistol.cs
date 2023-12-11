namespace Game.ItemScripts.Federal.Weapons.Pistol
{
	public class APPistol : WeaponItemScript
	{
		public APPistol() : base(187, 584646201, true, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.PISTOL)
		{
		}
	}

	public class APPistolAmmo : AmmoItemScript
	{
		public APPistolAmmo() : base(220, 584646201, 18, true)
		{
		}
	}
}