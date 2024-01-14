namespace Game.ItemScripts.Weapons.Rifles
{
	public class Carbinerifle : WeaponItemScript
	{
		public Carbinerifle() : base(53, 2210333304u, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class CarbinerifleAmmo : AmmoItemScript
	{
		public CarbinerifleAmmo() : base(112, 2210333304u, 30)
		{
		}
	}

	public class CarbinerifleSuppressor : AttatchmentItemScript
	{
		public CarbinerifleSuppressor() : base(390, 0x837445AA, 2210333304u)
		{
		}
	}

	public class CarbinerifleScope : AttatchmentItemScript
	{
		public CarbinerifleScope() : base(389, 0xA0D89C42, 2210333304u)
		{
		}
	}

	public class CarbinerifleGrip : AttatchmentItemScript
	{
		public CarbinerifleGrip() : base(388, 0xC164F53, 2210333304u)
		{
		}
	}
}