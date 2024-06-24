namespace Core.Models.Hud
{
	public class HudClientData
	{
		public int Money { get; set; }
		public int Hunger { get; set; }
		public int Thirst { get; set; }

		public HudClientData()
		{

		}

		public HudClientData(int money, int hunger, int thirst)
		{
			Money = money;
            Hunger = hunger;
            Thirst = thirst;
		}
	}
}