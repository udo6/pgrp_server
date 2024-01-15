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
using Core.Enums;

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

            if (player.TeamDuty)
            {
                player.Notify("Müllabfuhr", "Du kannst den Job nicht im Dienst starten.", Core.Enums.NotificationType.ERROR);
                return;
            }

            var shape = RPShape.All.FirstOrDefault(x => x.Dimension == player.Dimension && x.ShapeType == Core.Enums.ColshapeType.GARBAGE_JOB_START && x.Position.Distance(player.Position) <= x.Size);
            if (shape == null) return;

            var model = GarbageJobService.Get(shape.ShapeId);
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

            var houses = HouseService.GetAll();
            if (houses == null) return;

            houses.ForEach(house => {
                var position = PositionService.Get(house.PositionId);
                if (position == null) return;

                var blip = Alt.CreateBlip(false, BlipType.Destination, position.Position, new[] { player });
                blip.ShortRange = false;
                blip.SetData("BLIP_ID", position.Id);
                blip.ScaleXY = new Vector2(0.85f, 0.85f);
                blip.Sprite = 1;
                blip.Name = "Muell";

                player.TemporaryBlips.Add(blip);
            });
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

            var spawnPosition = PositionService.Get(model.VehicleSpawnPositionId);
            if (spawnPosition == null) return;

            var vehicle = player.JobVehicle;
            if (vehicle != null)
            {
                if (vehicle.Position.Distance(spawnPosition.Position) > 40f)
                {
                    player.Notify("Müllabfuhr", "Dein Fahrzeug ist nicht in der nähe.", NotificationType.ERROR);
                    return;
                }

                vehicle.Delete();
                player.JobVehicle = null!;
            }

            player.IsInGarbageJob = false;
            player.Notify("Müllabfuhr", "Du hast den Job beendet.", Core.Enums.NotificationType.SUCCESS);
            player.TempClothesId = 0;
            PlayerController.ApplyPlayerClothes(player);

            foreach (var blip in player.TemporaryBlips)
            {
                blip.Destroy();
            }
            
            player.TemporaryBlips.Clear();
        }

        private static void Return(RPPlayer player)
        {
            if (!player.LoggedIn || player == null || player.JobVehicle == null) return;

            var shape = RPShape.All.FirstOrDefault(x => x.Dimension == player.Dimension && x.ShapeType == ColshapeType.GARBAGE_JOB_RETURN && x.Position.Distance(player.Position) <= x.Size);
            if (shape == null) return;

            var model = GarbageJobService.Get(shape.ShapeId);
            if (model == null) return;

            if (!player.IsInGarbageJob)
            {
                player.Notify("Müllabfuhr", "Du bist nicht in dem Job.", NotificationType.ERROR);
                return;
            }

            if (player.JobVehicle.Position.Distance(shape.Position) > 9f)
            {
                player.Notify("Müllabfuhr", "Das Fahrzeug ist nicht am Müllplatz.", NotificationType.ERROR);
                return;
            }

            player.JobVehicle.GetData("GARBAGE_COUNT", out int garbageCount);
            if (garbageCount <= 0)
            {
                player.Notify("Müllabfuhr", "Der Müllwagen ist leer.", NotificationType.ERROR);
                return;
            }

            player.StartInteraction(() =>
            {
                player.JobVehicle.GetData("GARBAGE_COUNT", out int garbageCount);
                player.JobVehicle.SetData("GARBAGE_COUNT", 0);
                player.Notify("Müllabfuhr", "Du hast den Müll entladen.", NotificationType.SUCCESS);
                PlayerController.AddMoney(player, model.Price * garbageCount);
            }, (1000 * 2) * garbageCount);
        }

        private static void Spawn(RPPlayer player, int jobId)
        {
            if (!player.LoggedIn || player == null || jobId <= 0) return;

            var model = GarbageJobService.Get(jobId);
            if (model == null) return;

            if (player.JobVehicle != null)
            {
                player.Notify("Müllabfuhr", "Du hast besitzt bereits einen Müllwagen.", NotificationType.INFO);
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

            player.JobVehicle = garbageTruck;

            player.Notify("Müllabfuhr", "Du hast einen Müllwagen gespawnt.", Core.Enums.NotificationType.SUCCESS);
        }

        private static void Throw(RPPlayer player, int vehicleId)
        {
            if (!player.LoggedIn || player == null || !player.IsInGarbageJob ||!player.HasGarbageInHand) return;

            RPVehicle vehicle = RPVehicle.All.FirstOrDefault(x => x.Id == vehicleId)!;
            if (vehicle == null || vehicle.DbId > 0) return;

            vehicle.GetData("GARBAGE_COUNT", out int garbageCount);

            if (garbageCount >= 75)
            {
                player.Notify("Müllabfuhr", "Der Müllwagen ist voll.", Core.Enums.NotificationType.ERROR);
                player.Emit("Client:PropSyncModule:Clear");
                player.SetData("HOLDING_GARBAGE", false);
                player.HasGarbageInHand = false;
                return;
            }

            vehicle.SetData("GARBAGE_COUNT", (garbageCount + 1));
            /* 
             * TODO: 
             * Play animation
             */
            player.Emit("Client:PropSyncModule:Clear");
            player.SetData("HOLDING_GARBAGE", false);
            player.HasGarbageInHand = false;

            player.Notify("Müllabfuhr", $"Du hast den Müll in den Müllwagen geworfen ({garbageCount + 1}/75).", Core.Enums.NotificationType.INFO);
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
            player.Emit("Client:PropSyncModule:AddProp", "hei_prop_heist_binbag", 0xdead, 0.075, 0, 0, 360, 270);
            player.HasGarbageInHand = true;

            player.Notify("Müllabfuhr", "Du hast den Müll abgeholt.", Core.Enums.NotificationType.INFO);
        }
    }
}
