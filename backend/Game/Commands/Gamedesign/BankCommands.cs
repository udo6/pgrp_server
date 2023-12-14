using Core;
using Core.Attribute;
using Core.Entities;
using Database.Models;
using Database.Models.Bank;
using Database.Services;
using Game.Controllers;

namespace Game.Commands.Gamedesign
{
	public static class BankCommands
	{
		[Command("createbank")]
		public static void CreateBank(RPPlayer player, string name)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var pos = new PositionModel(player.Position);
			PositionService.Add(pos);

			var bank = new BankModel(name.Replace('_', ' '), pos.Id, Core.Enums.BankType.BANK);
			BankService.Add(bank);
			BankController.LoadBank(bank);
		}

		[Command("createatm")]
		public static void CreateATM(RPPlayer player)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var pos = new PositionModel(player.Position);
			PositionService.Add(pos);

			var bank = new BankModel("ATM", pos.Id, Core.Enums.BankType.ATM);
			BankService.Add(bank);
			BankController.LoadBank(bank);
		}
	}
}