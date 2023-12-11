namespace Game.ItemScripts.Federal.Weapons.Pistol
{
	public class Marksmanpistol : WeaponItemScript
	{
		public Marksmanpistol() : base(190, 3696079510, true, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.PISTOL)
		{
		}
	}

	public class MarksmanpistolAmmo : AmmoItemScript
	{
		public MarksmanpistolAmmo() : base(224, 3696079510, 10, true)
		{
		}
	}
}