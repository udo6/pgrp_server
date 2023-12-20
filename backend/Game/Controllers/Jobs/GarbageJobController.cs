using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Database.Models.GarbageJob;
using Database.Services;
using Game.Modules.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Controllers.Jobs
{
    public class GarbageJobController
    {
        public static void LoadGarbageJobs(GarbageJobModel model)
        {
            var startPosition = PositionService.Get(model.PositionId);
            if (startPosition == null) return;

            var returnPosition = PositionService.Get(model.GarbageReturnPositionId);
            if (returnPosition == null) return;

            var startJobShape = (RPShape)Alt.CreateColShapeCylinder(startPosition.Position.Down(), 3f, 3f);
            startJobShape.Id = model.Id;
            startJobShape.ShapeType = ColshapeType.GARBAGE_JOB_START;
            startJobShape.Size = 3f;

            var returnJobShape = (RPShape)Alt.CreateColShapeCylinder(returnPosition.Position.Down(), 6f, 6f);
            returnJobShape.Id = model.Id;
            returnJobShape.ShapeType = ColshapeType.GARBAGE_JOB_RETURN;
            returnJobShape.Size = 6f;

            var ped = Alt.CreatePed(PedModel.Construct01SMY, startPosition.Position, startPosition.Rotation);
            ped.Frozen = true;
            ped.Health = 8000;
            ped.Armour = 8000;

            var blip = Alt.CreateBlip(true, BlipType.Destination, startPosition.Position, Array.Empty<IPlayer>());
            blip.Name = "Müllmann";
            blip.Sprite = (ushort)318;
            blip.Color = 47;
            blip.ShortRange = true;
        }
    }
}
