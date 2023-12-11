namespace Game.ItemScripts.Weapons.MG
{
	public class MG : WeaponItemScript
	{
		public MG() : base(75, 2634544996, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SMG)
		{
		}
	}

	public class MGAmmo : AmmoItemScript
	{
		public MGAmmo() : base(93, 2634544996, 54)
		{
		}
	}
}