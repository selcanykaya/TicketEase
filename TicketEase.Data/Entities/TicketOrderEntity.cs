using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TicketEase.Data.Entities
{
    public class TicketOrderEntity : BaseEntity
    {
        public int TicketId { get; set; }
        public int OrderId { get; set; }

        // Relationships
        public OrderEntity Order { get; set; }
        public TicketEntity Ticket { get; set; }
    }

    public class TicketOrderConfiguration : BaseConfiguration<TicketOrderEntity>
    {
        public override void Configure(EntityTypeBuilder<TicketOrderEntity> builder)
        {
            base.Configure(builder);

            builder.Ignore(x => x.Id);

            // Configure composite key
            builder.HasKey("TicketId", "OrderId");

            // 🔹 DÜZENLEME: Cascade ilişkiler
            builder.HasOne(to => to.Ticket)
                .WithMany(t => t.TicketOrders)
                .HasForeignKey(to => to.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(to => to.Order)
                .WithMany(o => o.TicketOrders)
                .HasForeignKey(to => to.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
