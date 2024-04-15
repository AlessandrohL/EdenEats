using EdenEats.Domain.Contracts.Entities;
using EdenEats.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Domain.Entities
{
    public sealed class Order : BaseEntity, ISoftDeletable
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        public OrderStatus Status { get; set; } = OrderStatus.CREATED;
        public string Address { get; set; } = null!;
        public decimal Total { get; set; }
        public ICollection<OrderFood> OrderFoods { get; set; } = null!;
        public ICollection<Food> Foods { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
