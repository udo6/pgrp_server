namespace Game.ItemScripts.Federal.Weapons.Pistol
{
	public class PistolMk2 : WeaponItemScript
	{
		public PistolMk2() : base(193, 3219281620, true, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.PISTOL)
		{
		}
	}

	public class PistolMk2Ammo : AmmoItemScript
	{
		public PistolMk2Ammo() : base(227, 3219281620, 12, true)
		{
		}
	}
}