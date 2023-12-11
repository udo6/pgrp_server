namespace Game.ItemScripts.Weapons.SMG
{
	public class CombatPDW : WeaponItemScript
	{
		public CombatPDW() : base(81, 171789620, false, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.SMG)
		{
		}
	}

	public class CombatPDWAmmo : AmmoItemScript
	{
		public CombatPDWAmmo() : base(121, 171789620, 30)
		{
		}
	}
}