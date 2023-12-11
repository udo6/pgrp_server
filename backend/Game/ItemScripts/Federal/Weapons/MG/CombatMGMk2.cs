namespace Game.ItemScripts.Federal.Weapons.MG
{
	public class CombatMGMk2 : WeaponItemScript
	{
		public CombatMGMk2() : base(203, 3686625920, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SMG)
		{
		}
	}

	public class CombatMGMk2Ammo : AmmoItemScript
	{
		public CombatMGMk2Ammo() : base(218, 3686625920, 100, true)
		{
		}
	}
}