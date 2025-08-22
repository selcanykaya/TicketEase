using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketEase.Data.Enums;

namespace TicketEase.Data.Entities
{ 
    public class VenueEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public City City { get; set; }
        public int Capacity { get; set; }

        //Relationships
        // one venue can have many events
        public ICollection<EventEntity> Events { get; set; }

    }
    public class VenueConfiguration : BaseConfiguration<VenueEntity>
    {
        public override void Configure(EntityTypeBuilder<VenueEntity> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Address).IsRequired().HasMaxLength(200);
            builder.Property(x => x.City).HasConversion<string>().IsRequired();
            builder.Property(x => x.Capacity).IsRequired();
            builder.HasIndex(x => new { x.Name, x.Address, x.City }).IsUnique();

        }
    }
}
