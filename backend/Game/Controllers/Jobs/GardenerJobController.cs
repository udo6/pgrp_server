using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using AltV.Net;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Database.Models.GardenerJob;
using Database.Services;

namespace Game.Controllers.Jobs;

public class GardenerJobController
{
    public static void LoadGardenerJobs(GardenerJobModel model)
    {
        var position = PositionService.Get(model.PositionId);
        if (position == null) return;

        var startJobShape = (RPShape)Alt.CreateColShapeCylinder(position.Position.Down(), 2f, 2f);
        startJobShape.Id = model.Id;
        startJobShape.ShapeType = ColshapeType.GARDENER_JOB_START;
        startJobShape.Size = 2f;

        var ped = Alt.CreatePed(PedModel.Gardener01SMM, position.Position, position.Rotation);
        ped.Frozen = true;
        ped.Health = 8000;
        ped.Armour = 8000;

        var blip = Alt.CreateBlip(true, BlipType.Destination, position.Position, Array.Empty<IPlayer>());
        blip.Name = "Gärtner";
        blip.Sprite = (ushort)761;
        blip.Color = 69;
        blip.ShortRange = true;
    }
}