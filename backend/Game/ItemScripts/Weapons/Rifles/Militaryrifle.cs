namespace Game.ItemScripts.Weapons.Rifles
{
	public class Militaryrifle : WeaponItemScript
	{
		public Militaryrifle() : base(57, 2636060646u, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class MilitaryrifleAmmo : AmmoItemScript
	{
		public MilitaryrifleAmmo() : base(116, 2636060646u, 30)
		{
		}
	}

	public class MilitaryrifleSuppressor : AttatchmentItemScript
	{
		public MilitaryrifleSuppressor() : base(392, 0x837445AA, 2636060646u)
		{
		}
	}

	public class MilitaryrifleScope : AttatchmentItemScript
	{
		public MilitaryrifleScope() : base(391, 0xAA2C45B4, 2636060646u)
		{
		}
	}
}