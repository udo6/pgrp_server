namespace Game.ItemScripts.Weapons.Pistol
{
	public class SNSPistol : WeaponItemScript
	{
		public SNSPistol() : base(70, 3218215474, false, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.PISTOL)
		{
		}
	}

	public class SNSPistolAmmo : AmmoItemScript
	{
		public SNSPistolAmmo() : base(104, 3218215474, 6)
		{
		}
	}
}