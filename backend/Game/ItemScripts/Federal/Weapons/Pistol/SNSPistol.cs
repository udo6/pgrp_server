namespace Game.ItemScripts.Federal.Weapons.Pistol
{
	public class SNSPistol : WeaponItemScript
	{
		public SNSPistol() : base(196, 3218215474, true, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.PISTOL)
		{
		}
	}

	public class SNSPistolAmmo : AmmoItemScript
	{
		public SNSPistolAmmo() : base(230, 3218215474, 6, true)
		{
		}
	}
}