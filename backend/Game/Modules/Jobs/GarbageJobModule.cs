using AltV.Net.Elements.Entities;
using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Core.Models.NativeMenu;
using Database.Services;
using Game.Controllers;
using Game.Controllers.Jobs;
using Database.Services.Jobs;
using Core.Extensions;
using AltV.Net.Enums;

namespace Game.Modules.Jobs
{
    public static class GarbageJobModule
    {
        [Initialize]
        public static void Initialize()
        {
            foreach (var job in GarbageJobService.GetAll()) GarbageJobController.LoadGarbageJobs(job);

            Alt.OnClient<RPPlayer>("Server:GarbageJob:Open", Open);
            Alt.OnClient<RPPlayer, int>("Server:GarbageJob:Start", Start);
            Alt.OnClient<RPPlayer, int>("Server:GarbageJob:Stop", Stop);
            Alt.OnClient<RPPlayer>("Server:GarbageJob:Return", Return);
            Alt.OnClient<RPPlayer, int>("Server:GarbageJob:Spawn", Spawn);
            Alt.OnClient<RPPlayer, int>("Server:GarbageJob:Throw", Throw);
        }

        private static void Open(RPPlayer player)
        {
            if (!player.LoggedIn || player == null) return;

            var shape = RPShape.All.FirstOrDefault(x => x.Dimension == player.Dimension && x.ShapeType == Core.Enums.ColshapeType.GARBAGE_JOB_START && x.Position.Distance(player.Position) <= x.Size);
            if (shape == null) return;

            var model = GarbageJobService.Get(shape.Id);
            if (model == null) return;

            var jobItems = new List<NativeMenuItem>()
            {
                new NativeMenuItem("Job starten", true, "Server:GarbageJob:Start", model.Id),
            };

            var inJobItems = new List<NativeMenuItem>()
            {
                new NativeMenuItem("Müllwagen spawnen", true, "Server:GarbageJob:Spawn", model.Id),
                new NativeMenuItem("Job beenden", true, "Server:GarbageJob:Stop", model.Id),
            };

            var jobMenu = new NativeMenu("Müllabfuhr", player.IsInGarbageJob ? inJobItems : jobItems);
            player.ShowNativeMenu(true, jobMenu);
        }

        private static void Start(RPPlayer player, int jobId)
        {
            if (!player.LoggedIn || player == null || jobId <= 0) return;

            if (player.IsInJob())
            {
                player.Notify("Müllabfuhr", "Du bist bereits in einem Job.", Core.Enums.NotificationType.INFO);
                return;
            }

            var model = GarbageJobService.Get(jobId);
            if (model == null) return;

            player.IsInGarbageJob = true;

            var customization = CustomizationService.Get(player.CustomizationId);
            if (customization == null) return;

            player.TempClothesId = customization.Gender ? 65 : 66;
            PlayerController.ApplyPlayerClothes(player);
            player.Notify("Müllabfuhr", "Du hast den Job gestartet.", Core.Enums.NotificationType.SUCCESS);
        }

        private static void Stop(RPPlayer player, int jobId)
        {
            if (!player.LoggedIn || player == null) return;

            if (!player.IsInGarbageJob)
            {
                player.Notify("Müllabfuhr", "Du bist nicht in dem Job.", Core.Enums.NotificationType.ERROR);
                return;
            }

            var model = GarbageJobService.Get(jobId);
            if (model == null) return;

            player.IsInGarbageJob = false;
            player.Notify("Müllabfuhr", "Du hast den Job beendet.", Core.Enums.NotificationType.SUCCESS);
            player.TempClothesId = 0;
            PlayerController.ApplyPlayerClothes(player);

            if (player.GarbageTruck != null)
            {
                player.GarbageTruck.Delete();
                player.GarbageTruck = null!;
            }
        }

        private static void Return(RPPlayer player)
        {
            Console.WriteLine(01);
            if (!player.LoggedIn || player == null || player.GarbageTruck == null) return;
            Console.WriteLine(02);

            var shape = RPShape.All.FirstOrDefault(x => x.Dimension == player.Dimension && x.ShapeType == Core.Enums.ColshapeType.GARBAGE_JOB_RETURN && x.Position.Distance(player.Position) <= x.Size);
            if (shape == null) return;
            Console.WriteLine(03);

            var model = GarbageJobService.Get(shape.Id);
            if (model == null) return;
            Console.WriteLine(04);

            if (!player.IsInGarbageJob)
            {
                Console.WriteLine(05);
                player.Notify("Müllabfuhr", "Du bist nicht in dem Job.", Core.Enums.NotificationType.ERROR);
                return;
            }
            Console.WriteLine(06);

            if (player.GarbageTruck.Position.Distance(shape.Position) > 9f)
            {
                Console.WriteLine(07);
                player.Notify("Müllabfuhr", "Du bist nicht am Müllplatz.", Core.Enums.NotificationType.ERROR);
                return;
            }

            Console.WriteLine(08);
            player.GarbageTruck.GetData("GARBAGE_COUNT", out int garbageCount);

            player.StartInteraction(() =>
            {
                player.GarbageTruck.GetData("GARBAGE_COUNT", out int garbageCount);
                player.GarbageTruck.SetData("GARBAGE_COUNT", 0);
                player.Notify("Müllabfuhr", "Du hast den Müll entladen.", Core.Enums.NotificationType.SUCCESS);
                PlayerController.AddMoney(player, model.Price * garbageCount);
            }, (1000 * 2) * garbageCount);
        }

        private static void Spawn(RPPlayer player, int jobId)
        {
            if (!player.LoggedIn || player == null || jobId <= 0) return;

            var model = GarbageJobService.Get(jobId);
            if (model == null) return;

            if (player.GarbageTruck != null)
            {
                player.Notify("Müllabfuhr", "Du hast besitzt bereits einen Müllwagen.", Core.Enums.NotificationType.INFO);
                return;
            }

            if (model.TruckCount >= 5)
            {
                player.Notify("Müllabfuhr", "Es sind bereits 5 Müllwagen unterwegs.", Core.Enums.NotificationType.ERROR);
                return;
            }

            var spawnPosition = PositionService.Get(model.VehicleSpawnPositionId);
            if (spawnPosition == null) return;

            RPVehicle garbageTruck = (RPVehicle)Alt.CreateVehicle(VehicleModel.Trash, spawnPosition.Position, spawnPosition.Rotation);
            garbageTruck.OwnerId = player.DbId;
            garbageTruck.OwnerType = Core.Enums.OwnerType.PLAYER;
            garbageTruck.Dimension = player.Dimension;
            garbageTruck.SetLockState(true);
            garbageTruck.SetEngineState(false);
            garbageTruck.SetFuel(200);
            garbageTruck.SetMaxFuel(200);
            garbageTruck.NumberplateText = "MUELL-0" + player.Id * 2;
            garbageTruck.SetData("GARBAGE_COUNT", 0);

            player.GarbageTruck = garbageTruck;

            player.Notify("Müllabfuhr", "Du hast einen Müllwagen gespawnt.", Core.Enums.NotificationType.SUCCESS);
        }

        private static void Throw(RPPlayer player, int vehicleId)
        {
            if (!player.LoggedIn || player == null || !player.IsInGarbageJob ||!player.HasGarbageInHand) return;

            RPVehicle vehicle = RPVehicle.All.FirstOrDefault(x => x.Id == vehicleId)!;
            if (vehicle == null) return;

            vehicle.GetData("GARBAGE_COUNT", out int garbageCount);

            if (garbageCount >= 75)
            {
                player.Notify("Müllabfuhr", "Der Müllwagen ist voll.", Core.Enums.NotificationType.ERROR);
                return;
            }

            vehicle.SetData("GARBAGE_COUNT", (garbageCount + 1));
            /* 
             * TODO: 
             * Play animation
             * Clear prop task
             */
            player.Emit("Client:PlayerModule:SetGarbageProp", false);
            player.SetData("HOLDING_GARBAGE", false);
            player.HasGarbageInHand = false;
        }

        public static void PickupGarbage(RPPlayer player)
        {
            if (!player.LoggedIn || player == null || !player.IsInGarbageJob) return;

            var shape = RPShape.All.FirstOrDefault(x => x.Dimension == player.Dimension && x.ShapeType == Core.Enums.ColshapeType.HOUSE && x.Position.Distance(player.Position) <= x.Size);
            if (shape == null) return;

            shape.GetData("GARBAGE_PICKED_UP", out DateTime lastPickedUp);

            if (DateTime.Now < lastPickedUp.AddMinutes(10))
            {
                player.Notify("Müllabfuhr", "Der Müll wurde hier vor kurzem abgeholt.", Core.Enums.NotificationType.ERROR);
                return;
            }

            shape.SetData("GARBAGE_PICKED_UP", DateTime.Now);
            player.Emit("Client:PlayerModule:SetGarbageProp", true);
            player.HasGarbageInHand = true;
        }
    }
}
