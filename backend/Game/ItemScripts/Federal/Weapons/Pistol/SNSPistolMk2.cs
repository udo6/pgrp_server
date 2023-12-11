namespace Game.ItemScripts.Federal.Weapons.Pistol
{
	public class SNSPistolMk2 : WeaponItemScript
	{
		public SNSPistolMk2() : base(197, 2285322324, true, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.PISTOL)
		{
		}
	}

	public class SNSPistolMk2Ammo : AmmoItemScript
	{
		public SNSPistolMk2Ammo() : base(231, 2285322324, 6, true)
		{
		}
	}
}