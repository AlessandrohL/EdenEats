using EdenEats.Domain.Contracts.Entities;
using EdenEats.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Domain.Entities
{
    public sealed class FoodComment : BaseEntity, ISoftDeletable
    {
        public Guid Id { get; set; }
        public Guid FoodId { get; set; }
        public Food Food { get; set; } = null!;
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        public string? Content { get; set; }
        public CommentRating Rating { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
