namespace Game.ItemScripts.Federal.Weapons.SMG
{
	public class SMG : WeaponItemScript
	{
		public SMG() : base(204, 736523883, true, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.SMG)
		{
		}
	}

	public class SMGAmmo : AmmoItemScript
	{
		public SMGAmmo() : base(252, 736523883, 30, true)
		{
		}
	}
}