using AltV.Net;
using AltV.Net.Elements.Entities;
using Core.Entities;

namespace Core.Factories
{
    public class BlipFactory : IBaseObjectFactory<IBlip>
    {
        public IBlip Create(ICore core, nint baseObjectPointer, uint id)
        {
            return new RPBlip(core, baseObjectPointer, id);
        }
    }
}