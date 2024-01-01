using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Http.Utils
{
	public class RequestAttribute : Attribute
	{
		public readonly string RequestString;

		public RequestAttribute(string request)
		{
			RequestString = request;
		}
	}
}
