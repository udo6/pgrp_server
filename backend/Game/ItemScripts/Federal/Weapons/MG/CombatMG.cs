namespace Game.ItemScripts.Federal.Weapons.MG
{
	public class CombatMG : WeaponItemScript
	{
		public CombatMG() : base(202, 2144741730, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SMG)
		{
		}
	}

	public class CombatMGAmmo : AmmoItemScript
	{
		public CombatMGAmmo() : base(217, 2144741730, 100, true)
		{
		}
	}
}