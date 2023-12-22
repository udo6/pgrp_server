using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Core.Models.NativeMenu;
using Database.Services;
using Database.Services.Jobs;
using Game.Controllers;
using Game.Controllers.Jobs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Modules.Jobs
{
    public static class MoneyTruckJobModule
    {
        [Initialize]
        public static void Initialize()
        {
            foreach (var job in MoneyTruckJobService.GetAll()) MoneyTruckJobController.LoadMoneyTruckJobs(job);
            foreach (var points in MoneyTruckJobRoutePositionService.GetAll()) MoneyTruckJobRoutePositionController.LoadMoneyTruckJobRoutePostion(points);

            Alt.OnClient<RPPlayer>("Server:MoneyTruckJob:Open", Open);
            Alt.OnClient<RPPlayer, int>("Server:MoneyTruckJob:Start", Start);
            Alt.OnClient<RPPlayer, int>("Server:MoneyTruckJob:Stop", Stop);
            Alt.OnClient<RPPlayer>("Server:MoneyTruckJob:Pickup", Pickup);
        }

        private static void Open(RPPlayer player)
        {
            if (!player.LoggedIn || player == null) return;

            if (player.TeamDuty)
            {
                player.Notify("Geldtransporter", "Du kannst den Job nicht im Dienst starten.", NotificationType.ERROR);
                return;
            }

            var shape = RPShape.All.FirstOrDefault(x => x.Dimension == player.Dimension && x.ShapeType == ColshapeType.MONEY_TRUCK_JOB_START && x.Position.Distance(player.Position) <= x.Size);
            if (shape == null) return;

            var model = MoneyTruckJobService.Get(shape.Id);
            if (model == null) return;

            var jobMenu = new NativeMenu("Geldtransporter", new List<NativeMenuItem>()
            {
                new NativeMenuItem(player.IsInMoneyTruckJob ? "Job beenden" : "Job starten", true, player.IsInMoneyTruckJob ? "Server:MoneyTruckJob:Stop" : "Server:MoneyTruckJob:Start", model.Id),
            });

            player.ShowNativeMenu(true, jobMenu);
        }

        private static void Start(RPPlayer player, int id)
        {
            if (!player.LoggedIn || player == null) return;

            if (player.IsInJob())
            {
                player.Notify("Geldtransporter", "Du bist bereits in einem Job.", NotificationType.INFO);
                return;
            }

            var model = MoneyTruckJobService.Get(id);
            if (model == null) return;

            var freeRoute = MoneyTruckJobRouteService.GetAll().FirstOrDefault(x => !x.InWork && DateTime.Now >= x.LastUsed.AddMinutes(10));
            if (freeRoute == null) return;

            var routePositions = MoneyTruckJobRoutePositionService.GetAll().Where(x => x.RouteId == freeRoute.Id).ToList();
            if (routePositions.Count == 0) return;

            var customization = CustomizationService.Get(player.CustomizationId);
            if (customization == null) return;

            var spawnPosition = PositionService.Get(model.SpawnLocationId);
            if (spawnPosition == null) return;

            player.IsInMoneyTruckJob = true;
            player.TempClothesId = customization.Gender ? 92 : 93;
            PlayerController.ApplyPlayerClothes(player);
            player.Notify("Geldtransporter", "Du hast den Job gestartet.", NotificationType.SUCCESS);
            player.Notify("Geldtransporter", $"Du hast die Route {freeRoute.Name} erhalten", NotificationType.INFO);

            if (player.JobVehicle != null)
            {
                player.JobVehicle.Delete();
                player.JobVehicle = null!;
            }

            RPVehicle moneyTruck = (RPVehicle)Alt.CreateVehicle(VehicleModel.Stockade, spawnPosition.Position, spawnPosition.Rotation);
            moneyTruck.OwnerId = player.DbId;
            moneyTruck.OwnerType = OwnerType.PLAYER;
            moneyTruck.Dimension = player.Dimension;
            moneyTruck.SetLockState(true);
            moneyTruck.SetEngineState(false);
            moneyTruck.SetFuel(150);
            moneyTruck.SetMaxFuel(150);
            moneyTruck.NumberplateText = "MONEY-0" + player.Id * 2;

            player.JobVehicle = moneyTruck;
            freeRoute.LastUsed = DateTime.Now;

            routePositions.ForEach(position =>
            {
                var pos = PositionService.Get(position.PositionId);
                if (pos == null) return;

                player.TemporaryBlips.Add(Alt.CreateBlip(false, BlipType.Destination, pos.Position, new[] { player }));
            });
        }

        private static void Stop(RPPlayer player, int id)
        {
            if (!player.LoggedIn || player == null) return;

            if (!player.IsInMoneyTruckJob)
            {
                player.Notify("Geldtransporter", "Du bist nicht in dem Job.", NotificationType.ERROR);
                return;
            }

            var model = MoneyTruckJobService.Get(id);
            if (model == null) return;

            player.IsInMoneyTruckJob = false;
            player.Notify("Geldtransporter", "Du hast den Job beendet.", NotificationType.SUCCESS);
            player.Emit("Client:PropSyncModule:Clear");
            player.TempClothesId = 0;
            PlayerController.ApplyPlayerClothes(player);

            if (player.JobVehicle != null)
            {
                player.JobVehicle.Delete();
                player.JobVehicle = null!;
            }

            player.TemporaryBlips.ForEach(blip =>
            {
                if (blip == null) return;

                blip.Destroy();
                player.TemporaryBlips.Remove(blip);
            });
        }

        private static void Pickup(RPPlayer player)
        {
            if (!player.LoggedIn || player == null || !player.IsInMoneyTruckJob) return;

            var shape = RPShape.All.FirstOrDefault(x => player.Position.Distance(x.Position) <= x.Size && x.ShapeType == ColshapeType.MONEY_TRUCK_JOB_PICKUP && player.Dimension == x.Dimension);
            if (shape == null) return;

            var model = MoneyTruckJobRoutePositionService.Get(shape.Id);
            if (model == null) return;

            shape.GetData("MONEY_TRUCK_PICKED_UP", out bool pickedUp);

            if (pickedUp)
            {
                player.Notify("Geldtransporter", "Das Geld wurde hier bereits abgeholt.", NotificationType.ERROR);
                return;
            }

            shape.SetData("MONEY_TRUCK_PICKED_UP", true);
            player.Notify("Geldtransporter", "Du hast das Geld abgeholt.", NotificationType.SUCCESS);
            player.Emit("Client:PropSyncModule:AddProp", "prop_money_bag_01", 57005, 0, 0, 0, 0, 0, 0);
            // TODO: Play animation
        }
    }
}
