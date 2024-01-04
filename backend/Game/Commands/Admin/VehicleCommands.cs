using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Services;
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

		[Command("dvteam")]
		public static void DeleteTeamVehicle(RPPlayer player, int team)
		{
			if (!player.LoggedIn || player.AdminRank < AdminRank.ADMINISTRATOR) return;

			foreach (var vehicle in RPVehicle.All.ToList())
			{
				if (vehicle.OwnerType != OwnerType.TEAM || vehicle.OwnerId != team) continue;

				VehicleController.StoreVehicle(vehicle, -1, true);
			}
		}

		[Command("drift")]
		public static void ToggleDriftMode(RPPlayer player)
		{
			if (!player.LoggedIn || !player.IsInVehicle || player.AdminRank < AdminRank.SUPERADMIN) return;

			player.Vehicle.DriftMode = !player.Vehicle.DriftMode;
			player.Notify("Information", $"Du hast den Drift mode auf {player.Vehicle.DriftMode} gesetzt!", NotificationType.INFO);
		}

		[Command("applyteamtuning")]
		public static void ApplyTeamTuning(RPPlayer player, int color)
		{
			if (!player.LoggedIn || !player.IsInVehicle || player.AdminRank < AdminRank.SUPERADMIN) return;

			var vehicle = (RPVehicle)player.Vehicle;

			var vehTuning = TuningService.Get(vehicle.TuningId);
			if (vehTuning == null) return;

			vehicle.PrimaryColor = (byte)color;
			vehicle.SecondaryColor = (byte)color;
			vehicle.PearlColor = (byte)color;

			vehTuning.PrimaryColor = (byte)color;
			vehTuning.SecondaryColor = (byte)color;
			vehTuning.PearlColor = (byte)color;
			TuningService.Update(vehTuning);
		}

		[Command("fixvehradius")]
		public static void FixVehRadius(RPPlayer player, int radius)
		{
			if (!player.LoggedIn || player.AdminRank < AdminRank.MODERATOR) return;

			foreach(var vehicle in RPVehicle.All.ToList())
			{
				if(player.Position.Distance(vehicle.Position) > radius) continue;

				vehicle.Repair();
				var oldZ = vehicle.Rotation.Yaw;
				vehicle.Rotation = new(0, 0, oldZ);
			}

			player.Notify("Administration", "Du hast alle Fahrzeuge im Umkreis repariert!", NotificationType.SUCCESS);
		}

		[Command("fixveh")]
		public static void FixVeh(RPPlayer player)
		{
			if (!player.LoggedIn || !player.IsInVehicle || player.AdminRank < AdminRank.SUPERADMIN) return;

			player.Vehicle.Repair();
			player.Notify("Administration", "Du hast dein Fahrzeug repariert!", NotificationType.SUCCESS);
		}
	}
}