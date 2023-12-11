namespace Game.ItemScripts.Federal.Weapons.SMG
{
	public class Gusenberg : WeaponItemScript
	{
		public Gusenberg() : base(208, 1627465347, true, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.SMG)
		{
		}
	}

	public class GusenbergAmmo : AmmoItemScript
	{
		public GusenbergAmmo() : base(248, 1627465347, 30, true)
		{
		}
	}
}