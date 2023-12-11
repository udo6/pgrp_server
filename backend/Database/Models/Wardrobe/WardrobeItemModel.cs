using Core.Enums;

namespace Database.Models.Wardrobe
{
    public class WardrobeItemModel
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public OwnerType OwnerType { get; set; }
        public string Label { get; set; }
        public int Gender { get; set; }
        public int Component { get; set; }
        public int Drawable { get; set; }
        public int Texture { get; set; }
        public uint Dlc { get; set; }
        public bool Prop { get; set; }

        public WardrobeItemModel()
        {
            Label = string.Empty;
        }

        public WardrobeItemModel(int ownerId, OwnerType ownerType, string label, int gender, int component, int drawable, int texture, uint dlc, bool prop)
        {
            OwnerId = ownerId;
            OwnerType = ownerType;
            Label = label;
            Gender = gender;
            Component = component;
            Drawable = drawable;
            Texture = texture;
            Dlc = dlc;
            Prop = prop;
        }
    }
}