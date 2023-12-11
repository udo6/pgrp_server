using AltV.Net.Enums;

namespace Game.ItemScripts.Weapons.Rifles
{
	public class Assaultrifle : WeaponItemScript
	{
		public Assaultrifle() : base(49, 3220176749u, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class AssaultrifleAmmo : AmmoItemScript
	{
		public AssaultrifleAmmo() : base(108, 3220176749u, 30)
		{
		}
	}
}