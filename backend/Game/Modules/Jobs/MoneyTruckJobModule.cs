using AltV.Net;
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
            Alt.OnClient<RPPlayer, int>("Server:MoneyTruckJob:Pickup", Pickup);
        }

        private static void Open(RPPlayer player)
        {
            if (!player.LoggedIn || player == null) return;

            if (player.TeamDuty)
            {
                player.Notify("Müllabfuhr", "Du kannst den Job nicht im Dienst starten.", NotificationType.ERROR);
                return;
            }

            var shape = RPShape.All.FirstOrDefault(x => x.Dimension == player.Dimension && x.ShapeType == ColshapeType.MONEY_TRUCK_JOB_START && x.Position.Distance(player.Position) <= x.Size);
            if (shape == null) return;

            var model = MoneyTruckJobService.Get(shape.Id);
            if (model == null) return;

            var jobMenu = new NativeMenu("Geldtransport", new List<NativeMenuItem>()
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

            var freeRoute = MoneyTruckJobRouteService.GetAll().FirstOrDefault(x => !x.InWork);
            if (freeRoute == null) return;

            player.IsInMoneyTruckJob = true;

            var customization = CustomizationService.Get(player.CustomizationId);
            if (customization == null) return;

            player.TempClothesId = customization.Gender ? 65 : 66;
            PlayerController.ApplyPlayerClothes(player);
            player.Notify("Geldtransporter", "Du hast den Job gestartet.", NotificationType.SUCCESS);

            if (player.JobVehicle != null)
            {
                player.JobVehicle.Delete();
                player.JobVehicle = null!;
            }

            var spawnPosition = PositionService.Get(model.SpawnLocationId);
            if (spawnPosition == null) return;

            RPVehicle moneyTruck = (RPVehicle)Alt.CreateVehicle(VehicleModel.Stockade, spawnPosition.Position, spawnPosition.Rotation);
            moneyTruck.OwnerId = player.DbId;
            moneyTruck.OwnerType = OwnerType.PLAYER;
            moneyTruck.Dimension = player.Dimension;
            moneyTruck.SetLockState(true);
            moneyTruck.SetEngineState(false);
            moneyTruck.SetFuel(150);
            moneyTruck.SetMaxFuel(150);
            moneyTruck.NumberplateText = "MONEYTRUCK-0" + player.Id * 2;
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
            player.TempClothesId = 0;
            PlayerController.ApplyPlayerClothes(player);

            if (player.JobVehicle != null)
            {
                player.JobVehicle.Delete();
                player.JobVehicle = null!;
            }
        }

        private static void Pickup(RPPlayer player, int id)
        {
            if (!player.LoggedIn || player == null || !player.IsInMoneyTruckJob) return;

            var model = MoneyTruckJobRoutePositionService.Get(id);
            if (model == null) return;
            
            if (model.HasBeenPickedUp)
            {
                player.Notify("Geldtransporter", "Das Geld wurde hier bereits abgeholt.", NotificationType.ERROR);
                return;
            }

            model.HasBeenPickedUp = true;
            // TODO: Player animation and set prop
        }
    }
}
