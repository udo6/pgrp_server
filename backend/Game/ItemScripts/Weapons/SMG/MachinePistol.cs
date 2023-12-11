namespace Game.ItemScripts.Weapons.SMG
{
	public class MachinePistol : WeaponItemScript
	{
		public MachinePistol() : base(83, 3675956304, false, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.SMG)
		{
		}
	}

	public class MachinePistolAmmo : AmmoItemScript
	{
		public MachinePistolAmmo() : base(123, 3675956304, 12)
		{
		}
	}
}