namespace Game.ItemScripts.Weapons.MG
{
	public class CombatMG : WeaponItemScript
	{
		public CombatMG() : base(76, 2144741730, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SMG)
		{
		}
	}

	public class CombatMGAmmo : AmmoItemScript
	{
		public CombatMGAmmo() : base(91, 2144741730, 100)
		{
		}
	}
}