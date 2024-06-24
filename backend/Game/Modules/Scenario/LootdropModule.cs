using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Database.Models.Inventory;
using Database.Services;
using Game.Controllers;
using Newtonsoft.Json;

namespace Game.Modules.Scenario
{
	public class ActiveLootDrop
	{
		public bool Open { get; set; }
		public int DbId { get; set; }
		public Position Position { get; set; }
		public IObject MainObject { get; set; }
		public List<IObject> LootBoxes { get; set; }

		public ActiveLootDrop(int dbId, Position position, IObject obj)
		{
			Open = false;
			DbId = dbId;
			Position = position;
			MainObject = obj;
			LootBoxes = new();
		}
	}

    public static class LootdropModule
	{
		public static readonly Random Random = new Random();
		public static readonly List<ActiveLootDrop> ActiveLootDrops = new();

		[Initialize]
		public static void Initialize()
		{
			Alt.OnClient<RPPlayer>("Server:Lootdrop:Pickup", PickupLoot);
		}

		private static void PickupLoot(RPPlayer player)
		{
			var shape = RPShape.All.FirstOrDefault(x => x.ShapeType == ColshapeType.DROP_BOX && x.Dimension == player.Dimension && x.Position.Distance(player.Position) <= x.Size);
			if (shape == null) return;

			shape.Object?.Destroy();
			shape.Remove2();

			var amount = Random.Next(3, 7);
			InventoryController.AddItem(player.InventoryId, 363, amount);
		}

		public static bool Spawn()
		{
			var drops = LootdropService.GetAll().Where(x => !ActiveLootDrops.Any(e => e.DbId == x.Id)).ToList();
			if(drops.Count == 0) return false;

			var drop = drops[Random.Next(0, drops.Count)];

			var pos = PositionService.Get(drop.MainPositionId);
			if (pos == null) return false;

			var obj = Alt.CreateObject(249853152, pos.Position, pos.Rotation);
			obj.Frozen = true;

			ActiveLootDrops.Add(new(drop.Id, pos.Position, obj));

			var data = JsonConvert.SerializeObject(new
			{
				Title = "Militärischer Absturz",
				Message = $"Es wurde der Absturz einer Militärischen Flugmaschine in folgendem Gebiet gemeldet: {drop.Name}",
				Duration = 15000,
                Type = GlobalNotifyType.DROP
            });

			foreach (var player in RPPlayer.All.ToList())
			{
				player.EmitBrowser("Hud:ShowGlobalNotification", data);
			}

			return true;
		}

		public static void SpawnLoot(ActiveLootDrop activeDrop)
		{
			activeDrop.Open = true;

			var drop = LootdropService.Get(activeDrop.DbId);
			if (drop == null) return;

			var pos1 = PositionService.Get(drop.Box1PositionId);
			if (pos1 == null) return;

			var box1 = (RPShape)Alt.CreateColShapeCylinder(pos1.Position.Down(), 2f, 2f);
			box1.ShapeId = drop.Id;
			box1.ShapeType = ColshapeType.DROP_BOX;
			box1.Size = 2f;
			box1.Object = Alt.CreateObject(1776043012, pos1.Position.Down(), pos1.Rotation);
			box1.Object.Frozen = true;

			var pos2 = PositionService.Get(drop.Box2PositionId);
			if (pos2 == null) return;

			var box2 = (RPShape)Alt.CreateColShapeCylinder(pos2.Position.Down(), 2f, 2f);
			box2.ShapeId = drop.Id;
			box2.ShapeType = ColshapeType.DROP_BOX;
			box2.Size = 2f;
			box2.Object = Alt.CreateObject(1776043012, pos2.Position.Down(), pos2.Rotation);
			box2.Object.Frozen = true;

			var pos3 = PositionService.Get(drop.Box3PositionId);
			if (pos3 == null) return;

			var box3 = (RPShape)Alt.CreateColShapeCylinder(pos3.Position.Down(), 2f, 2f);
			box3.ShapeId = drop.Id;
			box3.ShapeType = ColshapeType.DROP_BOX;
			box3.Size = 2f;
			box3.Object = Alt.CreateObject(1776043012, pos3.Position.Down(), pos3.Rotation);
			box3.Object.Frozen = true;
		}

		public static ItemModel? GetLoot()
		{
			var loot = LootdropService.GetAllItems();
			var items = InventoryService.GetItems();
			float randomValue = Random.NextSingle();

			float cumulativeProbability = 0;
			foreach (var lootItem in loot)
			{
				var item = items.FirstOrDefault(x => x.Id == lootItem.ItemId);
				if (item == null) continue;

				cumulativeProbability += lootItem.Probability;
				if (randomValue < cumulativeProbability)
					return item;
			}

			return null;
		}

		[EveryFifteenMinutes]
		public static void Tick()
		{
			if (DateTime.Now.Hour < 16 || new Random().Next(0, 100) < 94) return;

			Spawn();
		}
	}
}