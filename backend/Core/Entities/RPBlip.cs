using AltV.Net;
using AltV.Net.Elements.Entities;

namespace Core.Entities
{
    public class RPBlip : Blip
    {
        public static List<RPBlip> All = new();
        public DateTime Created { get; set; }
        public DateTime DeleteAt { get; set; }

        public RPBlip(ICore core, nint nativePointer, uint id) : base(core, nativePointer, id)
        {
            Created = DateTime.Now;
            DeleteAt = DateTime.MaxValue;

            All.Add(this);
        }
    }
}