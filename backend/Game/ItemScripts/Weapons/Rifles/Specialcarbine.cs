namespace Game.ItemScripts.Weapons.Rifles
{
	public class Specialcarbine : WeaponItemScript
	{
		public Specialcarbine() : base(58, 3231910285u, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class SpecialcarbineAmmo : AmmoItemScript
	{
		public SpecialcarbineAmmo() : base(117, 3231910285u, 30)
		{
		}
	}

	public class SpecialcarbineSuppressor : AttatchmentItemScript
	{
		public SpecialcarbineSuppressor() : base(395, 0xA73D4664, 3231910285u)
		{
		}
	}

	public class SpecialcarbineScope : AttatchmentItemScript
	{
		public SpecialcarbineScope() : base(394, 0xA0D89C42, 3231910285u)
		{
		}
	}

	public class SpecialcarbineGrip : AttatchmentItemScript
	{
		public SpecialcarbineGrip() : base(393, 0xC164F53, 3231910285u)
		{
		}
	}
}