using EdenEats.Domain.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Domain.Entities
{
    public sealed class OrderFood
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid FoodId { get; set; }
        public Order Order { get; set; } = null!;
        public Food Food { get; set; } = null!;
        public int ItemQuantity { get; set; }
        public decimal SubTotal { get; set; }
    }
}
