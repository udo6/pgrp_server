using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Core.Models.NativeMenu;
using Database.Services.Jobs;
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
            foreach (var points in MoneyTruckJobRoutesService.GetAll()) MoneyTruckJobRouteController.LoadMoneyTruckJobRoutes(points);

            Alt.OnClient<RPPlayer>("Server:MoneyTruckJob:Open", Open);
        }

        private static void Open(RPPlayer player)
        {
            if (!player.LoggedIn || player == null) return;

            if (player.TeamDuty)
            {
                player.Notify("Müllabfuhr", "Du kannst den Job nicht im Dienst starten.", Core.Enums.NotificationType.ERROR);
                return;
            }

            var shape = RPShape.All.FirstOrDefault(x => x.Dimension == player.Dimension && x.ShapeType == ColshapeType.MONEY_TRUCK_JOB_START && x.Position.Distance(player.Position) <= x.Size);
            if (shape == null) return;

            var model = MoneyTruckJobService.Get(shape.Id);
            if (model == null) return;

            var jobItems = new List<NativeMenuItem>()
            {
                new NativeMenuItem("Job starten", true, "Server:MoneyTruckJob:Start", model.Id),
            };

            var inJobItems = new List<NativeMenuItem>()
            {
                new NativeMenuItem("Müllwagen spawnen", true, "Server:MoneyTruckJob:Spawn", model.Id),
                new NativeMenuItem("Job beenden", true, "Server:MoneyTruckJob:Stop", model.Id),
            };

            var jobMenu = new NativeMenu("Geldtransport", player.IsInMoneyTruckJob ? inJobItems : jobItems);
            player.ShowNativeMenu(true, jobMenu);
        }
    }
}
