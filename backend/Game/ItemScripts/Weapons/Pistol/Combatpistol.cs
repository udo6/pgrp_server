namespace Game.ItemScripts.Weapons.Pistol
{
	public class Combatpistol : WeaponItemScript
	{
		public Combatpistol() : base(62, 1593441988, false, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.PISTOL)
		{
		}
	}

	public class CombatpistolAmmo : AmmoItemScript
	{
		public CombatpistolAmmo() : base(95, 1593441988, 12)
		{
		}
	}
}