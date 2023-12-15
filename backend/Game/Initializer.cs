using Game.Controllers;
using System.Reflection;

namespace Game
{
	public static class Initializer
	{
		public static void Initialize()
		{
			var types = Assembly.GetExecutingAssembly().GetTypes();

			// Load item scripts
			var itemScriptsTypes = types.Where(t => (t.BaseType == typeof(ItemScript) || t.BaseType == typeof(WeaponItemScript) || t.BaseType == typeof(AttatchmentItemScript) || t.BaseType == typeof(AmmoItemScript) || t.BaseType == typeof(TempClothesItemScript)) && !t.IsAbstract).ToList();
			foreach(var itemScriptsType in itemScriptsTypes)
			{
				var instance = (ItemScript)Activator.CreateInstance(itemScriptsType)!;
				if (instance == null) continue;
				InventoryController.ItemScripts.Add(instance);
			}

			// Load Timers
			var everyFifteenMinutesTimers = types.SelectMany(t => t.GetMethods()).Where(m => m.GetCustomAttributes(typeof(Core.Attribute.EveryFifteenMinutesAttribute), false).Length > 0).ToList();
			foreach (var timer in everyFifteenMinutesTimers)
			{
				Timer.EveryFifteenMinuteActions.Add(timer);
			}

			var everyMinuteTimers = types.SelectMany(t => t.GetMethods()).Where(m => m.GetCustomAttributes(typeof(Core.Attribute.EveryMinuteAttribute), false).Length > 0).ToList();
			foreach (var timer in everyMinuteTimers)
			{
				Timer.EveryMinuteActions.Add(timer);
			}

			var everyTenSecondTimers = types.SelectMany(t => t.GetMethods()).Where(m => m.GetCustomAttributes(typeof(Core.Attribute.EveryTenSecondsAttribute), false).Length > 0).ToList();
			foreach (var timer in everyTenSecondTimers)
			{
				Timer.EveryTenSecondsActions.Add(timer);
			}

			var everyFiveSecondTimers = types.SelectMany(t => t.GetMethods()).Where(m => m.GetCustomAttributes(typeof(Core.Attribute.EveryFiveSeconds), false).Length > 0).ToList();
			foreach (var timer in everyFiveSecondTimers)
			{
				Timer.EveryFiveSecondsActions.Add(timer);
			}

			// Load Commands
			var commands = types.SelectMany(t => t.GetMethods()).Where(m => m.GetCustomAttributes(typeof(Core.Attribute.CommandAttribute), false).Length > 0).ToList();
			foreach (var command in commands)
			{
				ServerCommands.AllCommands.Add(command);
			}

			// Initialize everything
			var methods = types.SelectMany(t => t.GetMethods()).Where(m => m.GetCustomAttributes(typeof(Core.Attribute.InitializeAttribute), false).Length > 0).ToList();
			foreach (var method in methods)
			{
				method.Invoke(null, Array.Empty<object>());
			}

			ServerEvents.Initialize();
		}
	}
}