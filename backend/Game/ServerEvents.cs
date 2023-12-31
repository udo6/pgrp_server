using AltV.Net;
using AltV.Net.Elements.Entities;
using System.Reflection;
using Core.Enums;

namespace Game
{
	public static class ServerEvents
	{
		private static List<MethodInfo> Events = new();

		public static void Initialize()
		{
			Events = Assembly.GetExecutingAssembly().GetTypes().SelectMany(t => t.GetMethods()).Where(m => m.GetCustomAttributes(typeof(Core.Attribute.ServerEventAttribute), false).Length > 0).ToList();

			Alt.OnPlayerConnect += OnPlayerConnect;
			Alt.OnPlayerDisconnect += OnPlayerDisconnect;
			Alt.OnPlayerDead += OnPlayerDeath;
			Alt.OnColShape += OnEntityColshape;
		}

		private static void OnPlayerConnect(IPlayer player, string reason)
		{
			foreach (var method in Events)
			{
				var info = GetEventAttribute(method);
				if (info.EventType != ServerEventType.PLAYER_CONNECT) continue;
				Invoke(method, player, reason);
			}
		}

		private static void OnPlayerDisconnect(IPlayer player, string reason)
		{
			foreach (var method in Events)
			{
				var info = GetEventAttribute(method);
				if (info.EventType != ServerEventType.PLAYER_DISCONNECT) continue;
				Invoke(method, player, reason);
			}
		}

		private static void OnPlayerDeath(IPlayer player, IEntity? killer, uint weapon)
		{
			foreach (var method in Events)
			{
				var info = GetEventAttribute(method);
				if (info.EventType != ServerEventType.PLAYER_DEATH) continue;
				Invoke(method, player, killer, weapon);
			}
		}

		private static void OnEntityColshape(IColShape shape, IWorldObject entity, bool entered)
		{
			foreach (var method in Events)
			{
				var info = GetEventAttribute(method);
				if (info.EventType != ServerEventType.ENTITY_COLSHAPE) continue;
				Invoke(method, shape, entity, entered);
			}
		}

		private static void Invoke(MethodInfo method, params object?[] args)
		{
			method.Invoke(null, args);
		}

		private static Core.Attribute.ServerEventAttribute GetEventAttribute(MethodInfo method)
		{
			return (Core.Attribute.ServerEventAttribute)method.GetCustomAttribute(typeof(Core.Attribute.ServerEventAttribute))!;
		}
	}
}