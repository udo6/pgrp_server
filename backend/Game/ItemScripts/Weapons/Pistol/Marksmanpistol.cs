namespace Game.ItemScripts.Weapons.Pistol
{
	public class Marksmanpistol : WeaponItemScript
	{
		public Marksmanpistol() : base(64, 3696079510, false, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.PISTOL)
		{
		}
	}

	public class MarksmanpistolAmmo : AmmoItemScript
	{
		public MarksmanpistolAmmo() : base(98, 3696079510, 10)
		{
		}
	}
}