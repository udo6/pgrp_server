namespace Game.ItemScripts.Federal.Weapons.SMG
{
	public class MicroSMG : WeaponItemScript
	{
		public MicroSMG() : base(211, 324215364, true, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.SMG)
		{
		}
	}

	public class MicroSMGAmmo : AmmoItemScript
	{
		public MicroSMGAmmo() : base(250, 324215364, 16, true)
		{
		}
	}
}