using System.Globalization;
using System.Reflection;

namespace Game.Http.Utils
{
	public class RequestParser
	{
		public string Request {  get; set; }
		public int RequestParams { get; set; }
		public ParameterInfo[] Parameters { get; set; }
		public MethodInfo Method { get; set; }

		public RequestParser(string request, int requestParams, ParameterInfo[] parameters, MethodInfo method)
		{
			Request = request;
			RequestParams = requestParams;
			Parameters = parameters;
			Method = method;
		}

		public bool Parse(Dictionary<string, string> parameters)
		{
			if (parameters.Count != RequestParams)
			{
				Console.WriteLine($"[REQUEST PARSER] {Method.Name}: Parameter count mismatch");
				return false;
			}

			var arguments = new object[parameters.Count];
			for (var i = 0; i < parameters.Count; i++)
			{
				try
				{
					arguments[i] = Convert.ChangeType(parameters.First(x => x.Key == Parameters[i].Name).Value, Parameters[i].ParameterType, CultureInfo.InvariantCulture);
				}
				catch (Exception e)
				{
					Console.WriteLine($"[REQUEST PARSER] {Method.Name}: Argument {i} cant be parsed! ({e.Message})");
					return false;
				}
			}

			try
			{
				Method.Invoke(null, arguments);
			}
			catch (Exception e)
			{
				Console.WriteLine($"[REQUEST PARSER] {Method.Name}: Cant invoke method! ({e.Message})");
				return false;
			}

			return true;
		}
	}
}
