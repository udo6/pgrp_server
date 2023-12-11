namespace Game.ItemScripts.Federal.Weapons.Pistol
{
	public class Pistol50 : WeaponItemScript
	{
		public Pistol50() : base(192, 2578377531, true, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.PISTOL)
		{
		}
	}

	public class Pistol50Ammo : AmmoItemScript
	{
		public Pistol50Ammo() : base(226, 2578377531, 9, true)
		{
		}
	}
}