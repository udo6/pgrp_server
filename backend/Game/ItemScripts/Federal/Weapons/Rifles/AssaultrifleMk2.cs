namespace Game.ItemScripts.Federal.Weapons.Rifles
{
	public class AssaultrifleMk2 : WeaponItemScript
	{
		public AssaultrifleMk2() : base(176, 961495388u, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class AssaultrifleMk2Ammo : AmmoItemScript
	{
		public AssaultrifleMk2Ammo() : base(235, 961495388u, 30, true)
		{
		}
	}
}