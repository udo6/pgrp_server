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

	public class AssaultrifleSuppressor : AttatchmentItemScript
	{
		public AssaultrifleSuppressor() : base(382, 0xA73D4664, 3220176749u)
		{
		}
	}

	public class AssaultrifleScope : AttatchmentItemScript
	{
		public AssaultrifleScope() : base(383, 0x9D2FBF29, 3220176749u)
		{
		}
	}

	public class AssaultrifleGrip : AttatchmentItemScript
	{
		public AssaultrifleGrip() : base(384, 0xC164F53, 3220176749u)
		{
		}
	}
}