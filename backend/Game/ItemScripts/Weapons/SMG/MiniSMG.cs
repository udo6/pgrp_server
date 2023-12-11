namespace Game.ItemScripts.Weapons.SMG
{
	public class MiniSMG : WeaponItemScript
	{
		public MiniSMG() : base(84, 3173288789, false, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.SMG)
		{
		}
	}

	public class MiniSMGAmmo : AmmoItemScript
	{
		public MiniSMGAmmo() : base(125, 3173288789, 16)
		{
		}
	}
}