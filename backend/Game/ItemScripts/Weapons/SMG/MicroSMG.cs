namespace Game.ItemScripts.Weapons.SMG
{
	public class MicroSMG : WeaponItemScript
	{
		public MicroSMG() : base(85, 324215364, false, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.SMG)
		{
		}
	}

	public class MicroSMGAmmo : AmmoItemScript
	{
		public MicroSMGAmmo() : base(124, 324215364, 16)
		{
		}
	}
}