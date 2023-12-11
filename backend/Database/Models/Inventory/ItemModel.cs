namespace Database.Models.Inventory
{
    public class ItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int StackSize { get; set; }
        public float Weight { get; set; }

        public ItemModel()
        {
            Name = string.Empty;
            Icon = string.Empty;
        }

        public ItemModel(string name, string icon, int stackSize, float weight)
        {
            Name = name;
            Icon = icon;
            StackSize = stackSize;
            Weight = weight;
        }
    }
}