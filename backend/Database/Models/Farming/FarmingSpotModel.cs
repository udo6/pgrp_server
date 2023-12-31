using AltV.Net.Elements.Entities;
using Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.Farming
{
    public class FarmingSpotModel
    {
        public int Id { get; set; }
        public int FarmingId { get; set; }
        public int PositionId { get; set; }

        [NotMapped]
        public RPShape? Shape { get; set; }

        [NotMapped]
        public int Health { get; set; } = 100;

        [NotMapped]
        public DateTime Despawned { get; set; }

        [NotMapped]
        public IObject? Object { get; set; }

        public FarmingSpotModel() { }

        public FarmingSpotModel(int farmingId, int positionId)
        {
            FarmingId = farmingId;
            PositionId = positionId;
        }
    }

	public class FarmingSpotModelConfiguration : IEntityTypeConfiguration<FarmingSpotModel>
	{
		public void Configure(EntityTypeBuilder<FarmingSpotModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_farming_spots");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.FarmingId).HasColumnName("farming_id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
		}
	}
}