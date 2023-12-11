namespace Game.ItemScripts.Weapons.Pistol
{
	public class Pistol50 : WeaponItemScript
	{
		public Pistol50() : base(66, 2578377531, false, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.PISTOL)
		{
		}
	}

	public class Pistol50Ammo : AmmoItemScript
	{
		public Pistol50Ammo() : base(100, 2578377531, 9)
		{
		}
	}
}