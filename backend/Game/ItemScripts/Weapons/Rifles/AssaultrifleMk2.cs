namespace Game.ItemScripts.Weapons.Rifles
{
	public class AssaultrifleMk2 : WeaponItemScript
	{
		public AssaultrifleMk2() : base(50, 961495388u, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class AssaultrifleMk2Ammo : AmmoItemScript
	{
		public AssaultrifleMk2Ammo() : base(109, 961495388u, 30)
		{
		}
	}
}