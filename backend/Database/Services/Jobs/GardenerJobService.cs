using Database.Models.GardenerJob;

namespace Database.Services.Jobs;

public static class GardenerJobService
{
    public static List<GardenerJobModel> GetAll()
    {
        using var ctx = new Context();
        return ctx.GardenerJobs.ToList();
    }

    public static void Add(GardenerJobModel model)
    {
        using var ctx = new Context();
        ctx.GardenerJobs.Add(model);
        ctx.SaveChanges();
    }

    public static void Remove(GardenerJobModel model)
    {
        using var ctx = new Context();
        ctx.GardenerJobs.Remove(model);
        ctx.SaveChanges();
    }

    public static GardenerJobModel? Get(int id)
    {
        using var ctx = new Context();
        return ctx.GardenerJobs.FirstOrDefault(x => x.Id == id);
    }
}