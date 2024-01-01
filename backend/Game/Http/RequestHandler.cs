using Game.Http.Utils;
using System.Net;
using System.Reflection;
using System.Web;

namespace Game.Http
{
	public static class RequestHandler
	{
		private static readonly List<RequestParser> Requests = new List<RequestParser>();
		private static readonly List<RequestScript> LoadedScripts = new List<RequestScript>();
		private static List<string> _ipWhitelist = new()
		{
			"176.96.138.70" // forum/ucp server
		};
		private static WebServer? _webServer;
		private static string _key = "Ik8KtINYumyZyfnhbHQYVlN4LaXx2SXEmIMnogREBrwiaJkL6i";

		public static void Register()
		{
			lock (Requests)
			{
				foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
				{
					if (!type.IsSubclassOf(typeof(RequestScript)) || type.IsAbstract) continue;
					if (Activator.CreateInstance(type) is RequestScript script) LoadedScripts.Add(script);
				}

				LoadedScripts.ToList().ForEach(script =>
				{

					var methods = script.GetType().GetMethods();
					var requests = methods.Where(x => x.CustomAttributes.Any(attr => attr.AttributeType == typeof(RequestAttribute)));
					foreach (var request in requests)
					{
						var req = request.GetCustomAttribute<RequestAttribute>();
						if (req == null) continue;

						var args = request.GetParameters();
						var parser = new RequestParser(req.RequestString, args.Length, args, request);

						lock (Requests) Requests.Add(parser);

						Console.WriteLine($"Registered Request: {script.GetType().Name}:{request.Name}.");
					}
				});
			}

			WebserverStart();
		}

		private static bool Parse(string request, Dictionary<string, string> parameters)
		{
			var result = false;

			lock (Requests)
			{
				var requests = Requests.Where(x => x.Request == request).ToList();
				foreach (var req in requests)
				{
					result = result || req.Parse(parameters);
				}
			}

			return result;
		}

		private static void WebserverStart()
		{
			var host = FetchPublicIp().GetAwaiter().GetResult();

			if (host != null)
			{
				try
				{
					host = $"http://{host}:19800/";
					_webServer = new WebServer(HandleRequest, host);
					_webServer.Run();
					Console.WriteLine("Webserver is listening on Port {0}!", host);
				}
				catch (Exception)
				{
					Console.WriteLine("Port 19800 is not allowed for your public IP.");
				}
			}
		}

		private static async Task<string?> FetchPublicIp()
		{
			using (var client = new HttpClient())
			{
				try
				{
					var response = await client.GetStringAsync("http://ipinfo.io/ip");
					return response.Trim();
				}
				catch (HttpRequestException e)
				{
					Console.WriteLine(e);
					return null;
				}
			}
		}

		private static string HandleRequest(HttpListenerRequest request)
		{
			if (request == null || request.RawUrl == null || request.HttpMethod != "POST") return "METHOD NOT ALLOWED";

			var data = GetPostData(request);

			data.Remove("secret", out var secretKey);
			if (secretKey != _key)
			{
				Console.WriteLine($"[REQUEST HANDLER] A request has been refused. (SECRET KEY MISMATCH)");
				return "UNAUTHORIZED";
			}

			var from = request.RemoteEndPoint?.Address.ToString();
			if (from == null || !_ipWhitelist.Contains(from))
			{
				Console.WriteLine($"[REQUEST HANDLER] A request has been refused. (IP BLOCK)");
				return "UNAUTHORIZED";
			}

			var success = Parse(request.RawUrl, data);
			return success ? "OK" : "BAD REQUEST";
		}

		private static Dictionary<string, string> GetPostData(HttpListenerRequest requst)
		{
			var values = new Dictionary<string, string>();

			using (var stream = requst.InputStream)
			{
				using (var reader = new StreamReader(stream, requst.ContentEncoding))
				{
					try
					{
						foreach (var part in reader.ReadToEnd().Split("&"))
						{
							var text = HttpUtility.UrlDecode(part);
							if (text == null) continue;
							var split = text.Split("=");
							values.TryAdd(split[0], split[1]);
						}
					}
					catch { /* */ }
				}
			}

			return values;
		}
	}
}
