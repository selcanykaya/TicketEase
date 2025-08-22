using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketEase.Data.Enums;

namespace TicketEase.Data.Entities
{
    public class PaymentEntity : BaseEntity
    {
        public PaymentMethod Method { get; set; } 
        public string TransactionId { get; set; } 
        public DateTime PaymentDate { get; set; } 
        public bool IsSuccessful { get; set; }
   

        public int OrderId { get; set; } // Foreign key to OrderEntity

        //Relationships
        // one payment can be associated with one order
        public OrderEntity Order { get; set; }

    }

    public class PaymentConfiguration : BaseConfiguration<PaymentEntity>
    {
        public override void Configure(EntityTypeBuilder<PaymentEntity> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Method).HasConversion<string>().IsRequired();
            builder.Property(x => x.TransactionId).IsRequired().HasMaxLength(25);
            builder.Property(x => x.PaymentDate).IsRequired();
            builder.Property(x => x.IsSuccessful).IsRequired();
            builder.HasIndex(x => new { x.OrderId, x.TransactionId }).IsUnique();
        }
    }
}
