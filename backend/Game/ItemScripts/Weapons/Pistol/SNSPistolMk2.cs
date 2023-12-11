namespace Game.ItemScripts.Weapons.Pistol
{
	public class SNSPistolMk2 : WeaponItemScript
	{
		public SNSPistolMk2() : base(71, 2285322324, false, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.PISTOL)
		{
		}
	}

	public class SNSPistolMk2Ammo : AmmoItemScript
	{
		public SNSPistolMk2Ammo() : base(105, 2285322324, 6)
		{
		}
	}
}