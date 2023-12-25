using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Game.Modules;

namespace Game.Commands.Admin
{
	public static class RadioCommands
	{
		[Command("joinradio")]
		public static void JoinRadio(RPPlayer player, int radio)
		{
			if (player.AdminRank < AdminRank.SUPERADMIN) return;

			VoiceModule.EnableRadio(player, true);
			VoiceModule.ChangeRadioFrequency(player, radio);
			// set ptt
		}
	}
}
