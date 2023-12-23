using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Models.MoneyTruckJob;
using Database.Services;
using Database.Services.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Commands.Gamedesign
{
    public static class MoneyTruckCommands
    {
        [Command("createroute")]
        public static void CreateRoute(RPPlayer player, string routeName, int reward)
        {
            if (routeName.Contains("_")) routeName.Replace('_', ' ');

            var route = new MoneyTruckJobRouteModel(routeName, reward);
            MoneyTruckJobRouteService.Add(route);

            player.Notify("MoneyJob", $"Du hast die Route {routeName} erstellt.", NotificationType.SUCCESS);
        }
        [Command("addroutepos")]
        public static void CreateRoute(RPPlayer player, int routeId, int positionId)
        {
            if (routeId <= 0 || positionId <= 0) return;

            var route = new MoneyTruckJobRoutePositionModel(routeId, positionId);
            MoneyTruckJobRoutePositionService.Add(route);

            player.Notify("MoneyJob", $"Du hast die Routen Position hinzugefügt.", NotificationType.SUCCESS);
        }
    }
}
