namespace Database.Models.Lootdrop
{
    public class LootdropModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MainPositionId { get; set; }
        public int Box1PositionId { get; set; }
        public int Box2PositionId { get; set; }
        public int Box3PositionId { get; set; }

        public LootdropModel()
        {
            Name = string.Empty;
        }

        public LootdropModel(string name, int mainPositionId, int box1PositionId, int box2PositionId, int box3PositionId)
		{
			Name = name;
			MainPositionId = mainPositionId;
            Box1PositionId = box1PositionId;
            Box2PositionId = box2PositionId;
            Box3PositionId = box3PositionId;
        }
    }
}