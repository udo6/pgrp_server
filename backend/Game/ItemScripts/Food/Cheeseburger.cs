using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ItemScripts.Food
{
	public class Cheeseburger : FoodItemScript
	{
		public Cheeseburger() : base(350, 20, 0)
		{
		}
	}

	public class DoubleCheeseburger : FoodItemScript
	{
		public DoubleCheeseburger() : base(351, 30, 0)
		{
		}
	}

	public class ChilliCheeseburger : FoodItemScript
	{
		public ChilliCheeseburger() : base(352, 20, 0)
		{
		}
	}

	public class ChilliCheesenuggets : FoodItemScript
	{
		public ChilliCheesenuggets() : base(353, 30, 0)
		{
		}
	}
}
