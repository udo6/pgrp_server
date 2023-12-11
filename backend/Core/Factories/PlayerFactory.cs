using AltV.Net;
using AltV.Net.Elements.Entities;
using Core.Entities;

namespace Core.Factories
{
	public class PlayerFactory : IEntityFactory<IPlayer>
	{
		public IPlayer Create(ICore core, nint entityPointer, uint id)
		{
			return new RPPlayer(core, entityPointer, id);
		}
	}
}