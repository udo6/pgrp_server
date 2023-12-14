using Database.Models.GarbageJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Services.Jobs
{
    public static class GarbageJobService
    {
        public static List<GarbageJobModel> GetAll()
        {
            using var ctx = new Context();
            return ctx.GarbageJobs.ToList();
        }

        public static void Add(GarbageJobModel model)
        {
            using var ctx = new Context();
            ctx.GarbageJobs.Add(model);
            ctx.SaveChanges();
        }

        public static void Remove(GarbageJobModel model)
        {
            using var ctx = new Context();
            ctx.GarbageJobs.Remove(model);
            ctx.SaveChanges();
        }

        public static GarbageJobModel? Get(int id)
        {
            using var ctx = new Context();
            return ctx.GarbageJobs.FirstOrDefault(x => x.Id == id);
        }
    }
}
