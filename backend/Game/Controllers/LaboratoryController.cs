using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Database.Models.Team;
using Database.Services;
using Game.Streamer;

namespace Game.Controllers
{
    public static class LaboratoryController
	{
		public static List<int> LabInsidePositionIds = new()
		{
			31,
			32
		};

		public static Dictionary<LaboratoryType, (Position Fuel, Position Interaction, Position Input, Position Output, Position Rob)> Positions = new()
		{
			{ LaboratoryType.WEED, (
				new(1064.7429f, -3187.6353f, -39.164062f),
				new(1044.567f, -3194.8748f, -38.16992f),
				new(1032.0527f, -3204.5935f, -38.186768f),
				new(1036.1406f, -3206.5054f, -38.186768f),
				new(1039.2924f, -3195.9165f, -38.16992f)
			)},

			{ LaboratoryType.COKE, (
				new(1103.2351f, -3198.3955f, -38.995605f),
				new(1087.3846f, -3194.2550f, -38.995605f),
				new(1099.7142f, -3194.9143f, -38.995605f),
				new(1102.1143f, -3193.8594f, -38.995605f),
				new(1103.5912f, -3195.8770f, -38.995605f)
			)},
		};

		public static void LoadLaboratory(LaboratoryModel model)
		{
			var fuelPos = Positions[model.Type].Fuel.Down();
			var fuel = (RPShape)Alt.CreateColShapeCylinder(fuelPos, 2f, 2f);
			fuel.Id = model.Id;
			fuel.ShapeType = ColshapeType.LABORATORY_FUEL;
			fuel.Size = 2f;
			fuel.Dimension = model.Id;
			fuel.InventoryId = model.FuelInventoryId;
			fuel.InventoryAccess = new() { (model.TeamId, OwnerType.TEAM) };

			MarkerStreamer.AddMarker(new(1, fuelPos, new(0, 155, 255, 255), 0, model.Id));

			var interactionPos = Positions[model.Type].Interaction.Down();
			var interaction = (RPShape)Alt.CreateColShapeCylinder(interactionPos, 2f, 2f);
			interaction.Id = model.Id;
			interaction.ShapeType = ColshapeType.LABORATORY_INTERACTION;
			interaction.Size = 2f;
			interaction.Dimension = model.Id;

			MarkerStreamer.AddMarker(new(1, interactionPos, new(0, 155, 255, 255), 0, model.Id));

			var inputPos = Positions[model.Type].Input.Down();
			var input = (RPShape)Alt.CreateColShapeCylinder(inputPos, 2f, 2f);
			input.Id = model.Id;
			input.ShapeType = ColshapeType.LABORATORY_INPUT;
			input.Size = 2f;
			input.Dimension = model.Id;

			MarkerStreamer.AddMarker(new(1, inputPos, new(0, 155, 255, 255), 0, model.Id));

			var outputPos = Positions[model.Type].Output.Down();
			var output = (RPShape)Alt.CreateColShapeCylinder(outputPos, 2f, 2f);
			output.Id = model.Id;
			output.ShapeType = ColshapeType.LABORATORY_OUTPUT;
			output.Size = 2f;
			output.Dimension = model.Id;

			MarkerStreamer.AddMarker(new(1, outputPos, new(0, 155, 255, 255), 0, model.Id));

			var robPos = Positions[model.Type].Rob.Down();
			var rob = (RPShape)Alt.CreateColShapeCylinder(robPos, 2f, 2f);
			rob.Id = model.Id;
			rob.ShapeType = ColshapeType.LABORATORY_ROB;
			rob.Size = 2f;
			rob.Dimension = model.Id;
			rob.InventoryId = model.RobInventoryId;

			MarkerStreamer.AddMarker(new(1, robPos, new(0, 155, 255, 255), 0, model.Id));
		}
	}
}