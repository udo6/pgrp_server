using Core.Attribute;
using Core.Entities;
using Database.Services;
using Game.Controllers;

namespace Game.Modules
{
	public static class BlipModule
	{
		[Initialize]
		public static void Initialize()
		{
			foreach(var blip in BlipService.GetAll())
				BlipController.LoadBlip(blip);
		}

		[EveryMinute]
		public static void EveryMinute()
		{
			foreach(var blip in RPBlip.All)
			{
				if(blip.DeleteAt < DateTime.Now)
				{
					blip.Destroy();
				}
			}
		}
	}
}