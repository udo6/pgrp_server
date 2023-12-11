namespace Game.ItemScripts.Weapons.Pistol
{
	public class Revolver : WeaponItemScript
	{
		public Revolver() : base(68, 3249783761, false, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.PISTOL)
		{
		}
	}

	public class RevolverAmmo : AmmoItemScript
	{
		public RevolverAmmo() : base(102, 3249783761, 6)
		{
		}
	}
}