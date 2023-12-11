using Core.Enums;

namespace Database.Models.Jumpoint
{
    public class JumppointModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OutsidePositionId { get; set; }
        public int OutsideDimension { get; set; }
        public int InsidePositionId { get; set; }
        public int InsideDimension { get; set; }
        public int OwnerId { get; set; }
        public int KeyHolderId { get; set; }
        public OwnerType OwnerType { get; set; }
        public bool Locked { get; set; }
        public DateTime LastCrack { get; set; }
        public JumppointType Type { get; set; }

        public JumppointModel()
        {
            Name = string.Empty;
        }

        public JumppointModel(string name, int outsidePositionId, int outsideDimension, int insidePositionId, int insideDimension, int ownerId, int keyHolderId, OwnerType ownerType, bool locked, DateTime lastCrack, JumppointType type)
        {
            Name = name;
            OutsidePositionId = outsidePositionId;
            OutsideDimension = outsideDimension;
            InsidePositionId = insidePositionId;
            InsideDimension = insideDimension;
            OwnerId = ownerId;
            KeyHolderId = keyHolderId;
            OwnerType = ownerType;
            Locked = locked;
            LastCrack = lastCrack;
            Type = type;
        }
    }
}