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
    public class OrderEntity : BaseEntity
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public int UserId { get; set; }// Foreign key to UserEntity

        // Relationships
        // one order can be associated with one user
        public UserEntity User { get; set; }

        // one order can have one payment
        public PaymentEntity Payment { get; set; }

        //List of TicketOrderEntity to handle many-to-many relationship with tickets
        public ICollection<TicketOrderEntity> TicketOrders { get; set; }

    }

    public class OrderConfiguration : BaseConfiguration<OrderEntity>
    {
        public override void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.OrderDate).IsRequired();
            builder.Property(x => x.TotalAmount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.Status).HasConversion<string>().IsRequired();


        }
    }
}
