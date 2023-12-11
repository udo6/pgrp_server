namespace Game.ItemScripts.Federal.Weapons.SMG
{
	public class AssaultSMG : WeaponItemScript
	{
		public AssaultSMG() : base(206, 4024951519, true, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.SMG)
		{
		}
	}

	public class AssaultSMGAmmo : AmmoItemScript
	{
		public AssaultSMGAmmo() : base(246, 4024951519, 30, true)
		{
		}
	}
}