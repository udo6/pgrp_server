using Database.Models.GarbageJob;
using Database.Models.MoneyTruckJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Services.Jobs
{
    public static class MoneyTruckJobService
    {
        public static List<MoneyTruckJobModel> GetAll()
        {
            using var ctx = new Context();
            return ctx.MoneyTruckJobs.ToList();
        }

        public static void Add(MoneyTruckJobModel model)
        {
            using var ctx = new Context();
            ctx.MoneyTruckJobs.Add(model);
            ctx.SaveChanges();
        }

        public static void Remove(MoneyTruckJobModel model)
        {
            using var ctx = new Context();
            ctx.MoneyTruckJobs.Remove(model);
            ctx.SaveChanges();
        }

        public static MoneyTruckJobModel? Get(int id)
        {
            using var ctx = new Context();
            return ctx.MoneyTruckJobs.FirstOrDefault(x => x.Id == id);
        }
    }
}
