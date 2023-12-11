namespace Game.ItemScripts.Weapons.SMG
{
	public class Gusenberg : WeaponItemScript
	{
		public Gusenberg() : base(82, 1627465347, false, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.SMG)
		{
		}
	}

	public class GusenbergAmmo : AmmoItemScript
	{
		public GusenbergAmmo() : base(122, 1627465347, 30)
		{
		}
	}
}