using AltV.Net.Elements.Entities;
using Core.Models.MoneyTruck;
using Database.Models.GarbageJob;
using Database.Models.MoneyTruckJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Services.Jobs
{
    public static class MoneyTruckJobRouteService
    {
        public static List<MoneyTruckActiveRouteModel> ActiveRoutes = new List<MoneyTruckActiveRouteModel>();

        public static List<MoneyTruckJobRouteModel> GetAll()
        {
            using var ctx = new Context();
            return ctx.MoneyTruckJobRoutes.ToList();
        }

        public static void Add(MoneyTruckJobRouteModel model)
        {
            using var ctx = new Context();
            ctx.MoneyTruckJobRoutes.Add(model);
            ctx.SaveChanges();
        }

        public static void Remove(MoneyTruckJobRouteModel model)
        {
            using var ctx = new Context();
            ctx.MoneyTruckJobRoutes.Remove(model);
            ctx.SaveChanges();
        }

        public static MoneyTruckJobRouteModel? Get(int id)
        {
            using var ctx = new Context();
            return ctx.MoneyTruckJobRoutes.FirstOrDefault(x => x.Id == id);
        }

        public static MoneyTruckActiveRouteModel? GetRouteByPlayerId(int playerId)
        {
            return ActiveRoutes.FirstOrDefault(x => x.PlayerId == playerId);
        }

        public static MoneyTruckActiveRouteModel? GetFreeRoute()
        {
            return ActiveRoutes.FirstOrDefault(x => !x.InWork && DateTime.Now >= x.LastUsed.AddMinutes(10));
        }
    }
}
