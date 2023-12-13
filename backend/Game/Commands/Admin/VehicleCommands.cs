using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Game.Controllers;

namespace Game.Commands.Admin
{
	public static class VehicleCommands
	{
		[Command("veh")]
		public static void SpawnVehicle(RPPlayer player, string vehName)
		{
			if (!player.LoggedIn || player.AdminRank < AdminRank.ADMINISTRATOR) return;

			var hash = Alt.Hash(vehName);

			var veh = (RPVehicle)Alt.CreateVehicle(hash, player.Position, player.Rotation);
			veh.Dimension = player.Dimension;
			veh.NumberplateText = "PGRP";
			veh.SetLockState(false);
			veh.SetEngineState(true);
			veh.SetFuel(1000);
			veh.SetMaxFuel(1000);
			player.SetIntoVehicle(veh, 1);
		}

		[Command("dv")]
		public static void DeleteVehicle(RPPlayer player)
		{
			if (!player.LoggedIn || player.AdminRank < AdminRank.MODERATOR  || !player.IsInVehicle) return;

			var veh = (RPVehicle)player.Vehicle;

			VehicleController.StoreVehicle(veh, -1, true);
		}

		[Command("dvradius")]
		public static void DeleteVehicleRadius(RPPlayer player, int radius = 5)
		{
			if (!player.LoggedIn || player.AdminRank < AdminRank.ADMINISTRATOR) return;

			foreach(var vehicle in RPVehicle.All.ToList())
			{
				if(player.Position.Distance(vehicle.Position) > radius) continue;

				VehicleController.StoreVehicle(vehicle, -1, true);
			}
		}

		[Command("fixveh")]
		public static void FixVehicle(RPPlayer player)
		{
			if (!player.LoggedIn || !player.IsInVehicle || player.AdminRank < AdminRank.SUPERADMINISTRATOR) return;

			player.Vehicle.Repair();
		}
	}
}