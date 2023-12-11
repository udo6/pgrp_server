using Core.Attribute;
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
	}
}