namespace Database.Models.Animation
{
    public class AnimationFavoriteModel
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int AnimationId { get; set; }
        public int Slot { get; set; }

        public AnimationFavoriteModel()
        {
        }

        public AnimationFavoriteModel(int accountId, int animationId, int slot)
        {
            AccountId = accountId;
            AnimationId = animationId;
            Slot = slot;
        }
    }
}