using Core.Attribute;
using Core.Entities;
using Database.Models;
using Database.Models.FFA;
using Database.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Commands.Gamedesign
{
	public static class FFACommands
	{
		[Command("createffa")]
		public static void CreateFFA(RPPlayer player, string name, int max)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var model = new FFAModel(name.Replace('_', ' '), max);
			FFAService.Add(model);
			player.Notify("Gamedesign", $"ID: {model.Id}", Core.Enums.NotificationType.SUCCESS);
		}

		[Command("addffaspawn")]
		public static void AddFFASpawn(RPPlayer player, int ffaId)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var pos = new PositionModel(player.Position, player.Rotation);
			PositionService.Add(pos);

			var model = new FFASpawnModel(ffaId, pos.Id);
			FFAService.AddSpawn(model);
			player.Notify("Gamedesign", $"Spawn hinzugefügt", Core.Enums.NotificationType.SUCCESS);
		}

		[Command("addffaweapon")]
		public static void AddFFAWeapon(RPPlayer player, int ffaId, int weaponHash)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var model = new FFAWeaponModel(ffaId, (uint)weaponHash);
			FFAService.AddWeapon(model);
			player.Notify("Gamedesign", $"Waffe hinzugefügt", Core.Enums.NotificationType.SUCCESS);
		}
	}
}
