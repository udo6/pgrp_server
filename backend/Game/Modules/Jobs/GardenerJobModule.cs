using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Database.Services.Jobs;
using Game.Controllers.Jobs;
using System;
using AltV.Net.Enums;
using Core.Enums;
using Core.Models.NativeMenu;
using Database.Services;
using Game.Controllers;

namespace Game.Modules.Jobs;

public static class GardenerJobModule
{
    [Initialize]
    public static void Initialize()
    {
        foreach (var job in GardenerJobService.GetAll()) GardenerJobController.LoadGardenerJobs(job);

        Alt.OnClient<RPPlayer>("Server:GardenerJob:Open", Open);
        Alt.OnClient<RPPlayer, int>("Server:GardenerJob:Start", Start);
        Alt.OnClient<RPPlayer, int>("Server:GardenerJob:Stop", Stop);
        Alt.OnClient<RPPlayer, int>("Server:GardenerJob:StopJob", StopJob);
    }

    private static void Open(RPPlayer player)
    {
        if (!player.LoggedIn || player == null) return;

        if (player.TeamDuty)
        {
            player.Notify("Gärtner", "Du kannst den Job nicht im Dienst starten.", NotificationType.ERROR);
            return;
        }

        var shape = RPShape.All.FirstOrDefault(x => x.Dimension == player.Dimension && x.ShapeType == ColshapeType.GARDENER_JOB_START && x.Position.Distance(player.Position) <= x.Size);
        if (shape == null) return;

        var model = GardenerJobService.Get(shape.ShapeId);
        if (model == null) return;

        var jobMenu = new NativeMenu("Gärtner", new List<NativeMenuItem>()
        {
            new (player.IsInGardenerJob ? "Job beenden" : "Job starten", true, player.IsInGardenerJob ? "Server:GardenerJob:Stop" : "Server:GardenerJob:Start", model.Id),
        });

        player.ShowNativeMenu(true, jobMenu);
    }

    private static void Start(RPPlayer player, int id)
    {
        if (!player.LoggedIn || player == null || id <= 0) return;

        if (player.IsInJob())
        {
            player.Notify("Gärtner", "Du bist bereits in einem Job.", NotificationType.INFO);
            return;
        }

        var model = GardenerJobService.Get(id);
        if (model == null) return;

        player.IsInGardenerJob = true;

        var customization = CustomizationService.Get(player.CustomizationId);
        if (customization == null) return;

        var spawnPosition = PositionService.Get(model.VehicleSpawnPositionId);
        if (spawnPosition == null) return;

        player.TempClothesId = customization.Gender ? 65 : 66;
        PlayerController.ApplyPlayerClothes(player);
        player.Notify("Gärtner", "Du hast den Job gestartet.", NotificationType.SUCCESS);

        if (player.JobVehicle != null)
        {
            player.JobVehicle.Delete();
            player.JobVehicle = null!;
        }

        var mower = (RPVehicle) Alt.CreateVehicle(VehicleModel.Mower, spawnPosition.Position, spawnPosition.Rotation);
        mower.OwnerId = player.DbId;
        mower.OwnerType = OwnerType.PLAYER;
        mower.Dimension = player.Dimension;
        mower.SetLockState(true);
        mower.SetEngineState(false);
        mower.SetFuel(150);
        mower.SetMaxFuel(150);
        mower.NumberplateText = "GARDENER-0" + player.Id * 2;
        player.JobVehicle = mower;

        player.Emit("Client:GardenerJob:StartJob");
    }

    private static void Stop(RPPlayer player, int id)
    {
        if (!player.LoggedIn || player == null || id <= 0) return;

        if (!player.IsInGardenerJob)
        {
            player.Notify("Gärtner", "Du bist nicht in diesem Beruf.", NotificationType.INFO);
            return;
        }

        var model = GardenerJobService.Get(id);
        if (model == null) return;

        var spawnPosition = PositionService.Get(model.VehicleSpawnPositionId);
        if (spawnPosition == null) return;

        var vehicle = RPVehicle.All.FirstOrDefault(x => x.OwnerId == player.DbId && x.OwnerType == OwnerType.PLAYER);
        if (vehicle == null)
        {
            if(player.JobVehicle != null)
            {
				player.IsInGardenerJob = false;
				player.Notify("Gärtner", "Du hast den Job beendet.", NotificationType.ERROR);
				player.Emit("Client:GardenerJob:StopJob");
				player.TempClothesId = 0;
				PlayerController.ApplyPlayerClothes(player);
				player.JobVehicle = null;
			}

            return;
        }

        if (vehicle.Position.Distance(spawnPosition.Position) > 40f)
        {
            player.Notify("Gärtner", "Dein Rasenmäher ist nicht in der nähe.", NotificationType.ERROR);
            return;
        }

        player.IsInGardenerJob = false;
        player.Notify("Gärtner", "Du hast den Job beendet.", NotificationType.SUCCESS);
        player.Emit("Client:GardenerJob:StopJob");
        player.TempClothesId = 0;
        PlayerController.ApplyPlayerClothes(player);

        if (player.JobVehicle != null)
        {
            player.JobVehicle.Delete();
            player.JobVehicle = null!;
        }
    }

    private static void StopJob(RPPlayer player, int count)
    {
        if (!player.LoggedIn || player == null || player.JobVehicle == null) return;

        if (count <= 0)
        {
            player.Notify("Gärtner", "Du hast kein Rasen geschnitten.", NotificationType.INFO);
            return;
        }

        PlayerController.AddMoney(player, count * 5);
        player.Notify("Gärtner", "Du hast dein Geld erhalten.", NotificationType.SUCCESS);
    }
}