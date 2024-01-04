using AltV.Net;
using AltV.Net.Elements.Entities;
using Core;
using Core.Factories;

namespace Game
{
	public class Main : Resource
	{
		public override void OnStart()
		{
			Discord.Main.Start(); // soll nicht awaited werden
			Initializer.Initialize();

			Logger.LogInfo("Resource started!");
		}

		public override void OnStop()
		{
			Logger.LogInfo("Resource stopped!");
		}

		public override IEntityFactory<IPlayer> GetPlayerFactory()
		{
			return new PlayerFactory();
		}

		public override IEntityFactory<IVehicle> GetVehicleFactory()
		{
			return new VehicleFactory();
		}

		public override IBaseObjectFactory<IColShape> GetColShapeFactory()
		{
			return new ColshapeFactory();
		}
	}
}