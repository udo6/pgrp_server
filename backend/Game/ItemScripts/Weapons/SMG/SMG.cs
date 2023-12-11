namespace Game.ItemScripts.Weapons.SMG
{
	public class SMG : WeaponItemScript
	{
		public SMG() : base(78, 736523883, false, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.SMG)
		{
		}
	}

	public class SMGAmmo : AmmoItemScript
	{
		public SMGAmmo() : base(126, 736523883, 30)
		{
		}
	}
}