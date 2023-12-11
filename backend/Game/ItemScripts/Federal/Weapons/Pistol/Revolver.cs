namespace Game.ItemScripts.Federal.Weapons.Pistol
{
	public class Revolver : WeaponItemScript
	{
		public Revolver() : base(194, 3249783761, true, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.PISTOL)
		{
		}
	}

	public class RevolverAmmo : AmmoItemScript
	{
		public RevolverAmmo() : base(228, 3249783761, 6, true)
		{
		}
	}
}