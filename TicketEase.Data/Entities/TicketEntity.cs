using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TicketEase.Data.Entities
{
    public class TicketEntity : BaseEntity
    {
        public string SeatNumber { get; set; }
        public decimal Price { get; set; }
        public bool IsSold { get; set; }

        public int EventId { get; set; } // Foreign key to EventEntity

        //Relationships
        // one ticket can be associated with one event
        public EventEntity Event { get; set; }



        // List of TicketOrderEntity to handle many-to-many relationship with tickets
        public ICollection<TicketOrderEntity> TicketOrders { get; set; } 
    }

    public class TicketConfiguration : BaseConfiguration<TicketEntity>
    {
        public override void Configure(EntityTypeBuilder<TicketEntity> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.SeatNumber).IsRequired().HasMaxLength(10);
            builder.Property(x => x.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.IsSold).IsRequired().HasDefaultValue(false);
            builder.HasIndex(x => new { x.EventId, x.SeatNumber }).IsUnique();



        }
    }
}
