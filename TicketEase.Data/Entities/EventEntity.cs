using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketEase.Data.Enums;

namespace TicketEase.Data.Entities
{
    public class EventEntity : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public EventType EventType { get; set; }

        public int VenueId { get; set; }
        // Foreign key to VenueEntity

        //Relationships
        //one event can have one venue
        public VenueEntity Venue { get; set; }
        //one event can have many tickets
        public ICollection<TicketEntity> Tickets { get; set; }

        public int OrganizerId { get; set; }
        public UserEntity Organizer { get; set; }
    }

    public class EventConfiguration : BaseConfiguration<EventEntity> 
    { 
        public override void Configure(EntityTypeBuilder<EventEntity> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).IsRequired(false).HasMaxLength(1000);
            builder.Property(x => x.StartDate).IsRequired();
            builder.Property(x => x.EventType).HasConversion<string>().IsRequired();
            builder.HasIndex(x => new { x.VenueId, x.Name }).IsUnique();
            builder.HasOne(e => e.Organizer)
            .WithMany() // Eğer UserEntity’de Event collection yoksa boş bırakabilirsin
            .HasForeignKey(e => e.OrganizerId)
            .OnDelete(DeleteBehavior.Restrict); // Organizer silindiğinde event silinmesin
            builder.Property(e => e.OrganizerId).HasDefaultValue(1).IsRequired();

        }
    }
}
