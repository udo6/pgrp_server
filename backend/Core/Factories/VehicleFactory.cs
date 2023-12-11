using AltV.Net;
using AltV.Net.Elements.Entities;
using Core.Entities;

namespace Core.Factories
{
	public class VehicleFactory : IEntityFactory<IVehicle>
	{
		public IVehicle Create(ICore core, nint entityPointer, uint id)
		{
			return new RPVehicle(core, entityPointer, id);
		}
	}
}