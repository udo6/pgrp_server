namespace Game.ItemScripts.Federal.Weapons.SMG
{
	public class MachinePistol : WeaponItemScript
	{
		public MachinePistol() : base(209, 3675956304, true, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.SMG)
		{
		}
	}

	public class MachinePistolAmmo : AmmoItemScript
	{
		public MachinePistolAmmo() : base(249, 3675956304, 12, true)
		{
		}
	}
}