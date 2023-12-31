﻿using AltV.Net.Elements.Entities;
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
using Database.Services.Jobs;

namespace Game.Controllers.Jobs
{
    public class MoneyTruckJobRoutePositionController
    {
        public static void LoadMoneyTruckJobRoutePostion(MoneyTruckJobRoutePositionModel model)
        {
            var position = PositionService.Get(model.PositionId);
            if (position == null) return;

            var pickupPoint = (RPShape)Alt.CreateColShapeCylinder(position.Position.Down(), 1.5f, 1.5f);
            pickupPoint.ShapeId = model.Id;
            pickupPoint.Dimension = 0;
            pickupPoint.ShapeType = ColshapeType.MONEY_TRUCK_JOB_PICKUP;
            pickupPoint.Size = 1.5f;
            pickupPoint.SetData("MONEY_TRUCK_PICKED_UP", false);
        }
    }
}
