namespace Game.ItemScripts.Federal.Weapons.SMG
{
	public class SMGMk2 : WeaponItemScript
	{
		public SMGMk2() : base(205, 2024373456, true, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.SMG)
		{
		}
	}

	public class SMGMk2Ammo : AmmoItemScript
	{
		public SMGMk2Ammo() : base(253, 2024373456, 30, true)
		{
		}
	}
}