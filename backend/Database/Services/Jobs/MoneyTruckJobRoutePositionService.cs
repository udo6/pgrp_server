using Database.Models.MoneyTruckJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Services.Jobs
{
    public class MoneyTruckJobRoutePositionService
    {
        public static List<MoneyTruckJobRoutePositionModel> GetAll()
        {
            using var ctx = new Context();
            return ctx.MoneyTruckJobRoutePosition.ToList();
        }

        public static void Add(MoneyTruckJobRoutePositionModel model)
        {
            using var ctx = new Context();
            ctx.MoneyTruckJobRoutePosition.Add(model);
            ctx.SaveChanges();
        }

        public static void Remove(MoneyTruckJobRoutePositionModel model)
        {
            using var ctx = new Context();
            ctx.MoneyTruckJobRoutePosition.Remove(model);
            ctx.SaveChanges();
        }

        public static MoneyTruckJobRoutePositionModel? Get(int id)
        {
            using var ctx = new Context();
            return ctx.MoneyTruckJobRoutePosition.FirstOrDefault(x => x.Id == id);
        }

        public static List<MoneyTruckJobRoutePositionModel> GetPositionsByRouteId(int routeId)
        {
            using var ctx = new Context();
            return ctx.MoneyTruckJobRoutePosition.Where(x => x.RouteId == routeId).ToList();
        }
    }
}
