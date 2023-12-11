using AltV.Net;
using Core.Attribute;
using Core.Entities;
using System.Reflection;

namespace Game
{
	public static class ServerCommands
	{
		public static List<MethodInfo> AllCommands = new();

		[Initialize]
		public static void Initialize()
		{
			Alt.OnClient<RPPlayer, string, string[]>("Server:Command", OnCommand);
		}

		private static void OnCommand(RPPlayer player, string command, string[] args)
		{
			try
			{
				foreach (var method in AllCommands)
				{
					var info = GetCommandAttribute(method);
					if (info.EventName != command) continue;

					if (info.GreedyArg)
					{
						method.Invoke(null, new object[] { player, string.Join(' ', args) });
					}
					else
					{
						var args2 = new object[] { player }.Concat(args.Select(arg => int.TryParse(arg, out var intValue) ? (object)intValue : arg)).ToArray();
						method.Invoke(null, args2);
					}
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine($"An Command Exception has been thrown by {player.Name}: /{command}");
			}
		}

		private static CommandAttribute GetCommandAttribute(MethodInfo method)
		{
			return (CommandAttribute)method.GetCustomAttribute(typeof(CommandAttribute))!;
		}
	}
}