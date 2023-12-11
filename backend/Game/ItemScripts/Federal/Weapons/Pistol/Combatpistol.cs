namespace Game.ItemScripts.Federal.Weapons.Pistol
{
	public class Combatpistol : WeaponItemScript
	{
		public Combatpistol() : base(188, 1593441988, true, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.PISTOL)
		{
		}
	}

	public class CombatpistolAmmo : AmmoItemScript
	{
		public CombatpistolAmmo() : base(221, 1593441988, 12, true)
		{
		}
	}
}