namespace Game.ItemScripts.Federal.Weapons.SMG
{
	public class CombatPDW : WeaponItemScript
	{
		public CombatPDW() : base(207, 171789620, true, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.SMG)
		{
		}
	}

	public class CombatPDWAmmo : AmmoItemScript
	{
		public CombatPDWAmmo() : base(247, 171789620, 30, true)
		{
		}
	}
}