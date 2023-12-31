using Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

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

	public class JumppointModelConfiguartion : IEntityTypeConfiguration<JumppointModel>
	{
		public void Configure(EntityTypeBuilder<JumppointModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_jumppoints");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(255)");
			builder.Property(x => x.OutsidePositionId).HasColumnName("outside_position_id").HasColumnType("int(11)");
			builder.Property(x => x.OutsideDimension).HasColumnName("outside_dimension").HasColumnType("int(11)");
			builder.Property(x => x.InsidePositionId).HasColumnName("inside_position_id").HasColumnType("int(11)");
			builder.Property(x => x.InsideDimension).HasColumnName("inside_dimension").HasColumnType("int(11)");
			builder.Property(x => x.OwnerId).HasColumnName("owner_id").HasColumnType("int(11)");
			builder.Property(x => x.KeyHolderId).HasColumnName("key_holder_id").HasColumnType("int(11)");
			builder.Property(x => x.OwnerType).HasColumnName("owner_type").HasColumnType("int(11)");
			builder.Property(x => x.Locked).HasColumnName("locked").HasColumnType("tinyint(1)");
			builder.Property(x => x.LastCrack).HasColumnName("last_crack").HasColumnType("datetime");
			builder.Property(x => x.Type).HasColumnName("type").HasColumnType("int(11)");
		}
	}
}