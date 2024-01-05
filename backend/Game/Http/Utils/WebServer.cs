using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Game.Http.Utils
{
	public class WebServer
	{
		private readonly HttpListener _listener = new HttpListener();
		private readonly Func<HttpListenerRequest, string> _responderMethod;

		private WebServer(IReadOnlyCollection<string> prefixes, Func<HttpListenerRequest, string> method)
		{
			if (!HttpListener.IsSupported)
				throw new NotSupportedException("Needs Windows XP SP2, Server 2003 or later.");

			if (prefixes == null || prefixes.Count == 0)
				throw new ArgumentException("prefixes");

			if (method == null)
				throw new ArgumentException("method");

			foreach (var s in prefixes)
				_listener.Prefixes.Add(s);

			_responderMethod = method;
			_listener.Start();
		}

		public WebServer(Func<HttpListenerRequest, string> method, params string[] prefixes) : this(prefixes, method) { }

		public void Run()
		{
			ThreadPool.QueueUserWorkItem(o =>
			{
				Console.WriteLine("Webserver running...");
				try
				{
					while (_listener.IsListening)
					{
						ThreadPool.QueueUserWorkItem(c =>
						{
							var context = c as HttpListenerContext;
							try
							{
								if (context == null) throw new NullReferenceException();
								var rstr = _responderMethod(context.Request);
								var buf = Encoding.UTF8.GetBytes(rstr);
								context.Response.ContentLength64 = buf.Length;
								context.Response.OutputStream.Write(buf, 0, buf.Length);
							}
							catch
							{
								//
							}
							finally
							{
								context?.Response.OutputStream.Close();
							}
						}, _listener.GetContext());
					}
				}
				catch
				{
					// suppress any exception
				}
			});
		}

		public void Stop()
		{
			_listener.Stop();
			_listener.Close();
		}
	}
}
