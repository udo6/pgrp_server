using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Attribute
{
	[AttributeUsage(AttributeTargets.Method)]
	public class EveryTwoMinuteAttribute : System.Attribute
	{
		public EveryTwoMinuteAttribute() { }
	}
}
