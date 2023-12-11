namespace Game.ItemScripts.Federal.Weapons.MG
{
	public class MG : WeaponItemScript
	{
		public MG() : base(201, 2634544996, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SMG)
		{
		}
	}

	public class MGAmmo : AmmoItemScript
	{
		public MGAmmo() : base(219, 2634544996, 54, true)
		{
		}
	}
}