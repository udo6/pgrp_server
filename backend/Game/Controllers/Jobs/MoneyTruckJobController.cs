using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using AltV.Net;
using Core.Entities;
using Database.Models.GarbageJob;
using Database.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;
using Database.Models.MoneyTruckJob;
using Core.Extensions;

namespace Game.Controllers.Jobs
{
    public class MoneyTruckJobController
    {
        public static void LoadMoneyTruckJobs(MoneyTruckJobModel model)
        {
            var position = PositionService.Get(model.StartLocationId);
            if (position == null) return;

            var startJobShape = (RPShape)Alt.CreateColShapeCylinder(position.Position.Down(), 4f, 4f);
            startJobShape.Id = model.Id;
            startJobShape.ShapeType = ColshapeType.MONEY_TRUCK_JOB_START;
            startJobShape.Size = 4f;

            var ped = Alt.CreatePed(PedModel.Casey, position.Position, position.Rotation);
            ped.Frozen = true;
            ped.Health = 8000;
            ped.Armour = 8000;

            var blip = Alt.CreateBlip(true, BlipType.Destination, position.Position, Array.Empty<IPlayer>());
            blip.Name = "Geldtransporter";
            blip.Sprite = (ushort)67;
            blip.Color = 46;
            blip.ShortRange = true;
        }
    }
}
