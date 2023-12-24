using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Models;
using Database.Models.MoneyTruckJob;
using Database.Services;
using Database.Services.Jobs;
namespace Game.Commands.Gamedesign
{
    public static class MoneyTruckCommands
    {
        [Command("createroute")]
        public static void CreateRoute(RPPlayer player, string routeName, int reward)
        {
            var route = new MoneyTruckJobRouteModel(routeName.Replace('_', ' '), reward);
            MoneyTruckJobRouteService.Add(route);

            player.Notify("MoneyJob", $"Du hast die Route {routeName} erstellt.", NotificationType.SUCCESS);
        }

        [Command("addroutepos")]
        public static void CreateRoute(RPPlayer player, int routeId)
        {
            var pos = new PositionModel(player.Position);
            PositionService.Add(pos);

            var route = new MoneyTruckJobRoutePositionModel(routeId, pos.Id);
            MoneyTruckJobRoutePositionService.Add(route);

            player.Notify("MoneyJob", $"Du hast die Routen Position hinzugefügt.", NotificationType.SUCCESS);
        }
    }
}
