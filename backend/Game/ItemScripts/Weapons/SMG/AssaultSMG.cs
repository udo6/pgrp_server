namespace Game.ItemScripts.Weapons.SMG
{
	public class AssaultSMG : WeaponItemScript
	{
		public AssaultSMG() : base(80, 4024951519, false, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.SMG)
		{
		}
	}

	public class AssaultSMGAmmo : AmmoItemScript
	{
		public AssaultSMGAmmo() : base(120, 4024951519, 30)
		{
		}
	}
}