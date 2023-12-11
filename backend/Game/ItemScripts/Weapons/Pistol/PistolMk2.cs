namespace Game.ItemScripts.Weapons.Pistol
{
	public class PistolMk2 : WeaponItemScript
	{
		public PistolMk2() : base(67, 3219281620, false, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.PISTOL)
		{
		}
	}

	public class PistolMk2Ammo : AmmoItemScript
	{
		public PistolMk2Ammo() : base(101, 3219281620, 12)
		{
		}
	}
}