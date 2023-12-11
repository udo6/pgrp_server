namespace Core.Models.Hud
{
	public class HudClientData
	{
		public int Money { get; set; }
		public int Starvation { get; set; }
		public int Hydration { get; set; }
		public float Strength { get; set; }

		public HudClientData()
		{

		}

		public HudClientData(int money, int starvation, int hydration)
		{
			Money = money;
			Starvation = starvation;
			Hydration = hydration;
			Strength = 110;
		}
	}
}