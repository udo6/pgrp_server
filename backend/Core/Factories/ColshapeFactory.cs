using AltV.Net;
using AltV.Net.Elements.Entities;
using Core.Entities;

namespace Core.Factories
{
	public class ColshapeFactory : IBaseObjectFactory<IColShape>
	{
		public IColShape Create(ICore core, nint baseObjectPointer, uint id)
		{
			return new RPShape(core, baseObjectPointer, id);
		}
	}
}