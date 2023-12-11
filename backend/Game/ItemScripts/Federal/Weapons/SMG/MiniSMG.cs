namespace Game.ItemScripts.Federal.Weapons.SMG
{
	public class MiniSMG : WeaponItemScript
	{
		public MiniSMG() : base(210, 3173288789, true, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.SMG)
		{
		}
	}

	public class MiniSMGAmmo : AmmoItemScript
	{
		public MiniSMGAmmo() : base(251, 3173288789, 16, true)
		{
		}
	}
}