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

		[Command("fixveh")]
		public static void FixVehicle(RPPlayer player)
		{
			if (!player.LoggedIn || !player.IsInVehicle || player.AdminRank < AdminRank.SUPERADMIN) return;

			player.Vehicle.Repair();
		}

		[Command("drift")]
		public static void ToggleDriftMode(RPPlayer player)
		{
			if (!player.LoggedIn || !player.IsInVehicle || player.AdminRank < AdminRank.SUPERADMIN) return;

			player.Vehicle.DriftMode = !player.Vehicle.DriftMode;
			player.Notify("Information", $"Du hast den Drift mode auf {player.Vehicle.DriftMode} gesetzt!", NotificationType.INFO);
		}

		[Command("applyteamtuning")]
		public static void ApplyTeamTuning(RPPlayer player, int color, int type)
		{
			if (!player.LoggedIn || !player.IsInVehicle || player.AdminRank < AdminRank.SUPERADMIN) return;

			var vehicle = (RPVehicle)player.Vehicle;

			// 0 = armored schafter, 1 = schafter, 2 = drafter
			var tuningBase = type == 2 ? 311 : type == 1 ? 312 : 313;

			var tuning = TuningService.Get(tuningBase);
			if (tuning == null) return;

			var vehTuning = TuningService.Get(vehicle.TuningId);
			if (vehTuning == null) return;

			vehTuning.PrimaryColor = (byte)color;
			vehTuning.SecondaryColor = (byte)color;
			vehTuning.PearlColor = (byte)color;
			vehTuning.Spoiler = tuning.Spoiler;
			vehTuning.FrontBumper = tuning.FrontBumper;
			vehTuning.RearBumper = tuning.RearBumper;
			vehTuning.SideSkirt = tuning.SideSkirt;
			vehTuning.Exhaust = tuning.Exhaust;
			vehTuning.Frame = tuning.Frame;
			vehTuning.Grille = tuning.Grille;
			vehTuning.Hood = tuning.Hood;
			vehTuning.Fender = tuning.Fender;
			vehTuning.RightFender = tuning.RightFender;
			vehTuning.Roof = tuning.Roof;
			vehTuning.Engine = tuning.Engine;
			vehTuning.Brakes = tuning.Brakes;
			vehTuning.Transmission = tuning.Transmission;
			vehTuning.Horns = tuning.Horns;
			vehTuning.Suspension = tuning.Suspension;
			vehTuning.Armor = tuning.Armor;
			vehTuning.Turbo = tuning.Turbo;
			vehTuning.Xenon = tuning.Xenon;
			vehTuning.Wheels = tuning.Wheels;
			vehTuning.WheelType = tuning.WheelType;
			vehTuning.WheelColor = tuning.WheelColor;
			vehTuning.PlateHolders = tuning.PlateHolders;
			vehTuning.TrimDesign = tuning.TrimDesign;
			vehTuning.WindowTint = tuning.WindowTint;
			vehTuning.HeadlightColor = tuning.HeadlightColor;
			vehTuning.Livery = tuning.Livery;
			TuningService.Update(vehTuning);

			VehicleController.ApplyVehicleTuning(vehicle);
		}
	}
}