using EdenEats.Domain.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Domain.Entities
{
    public sealed class Instruction : BaseEntity, ISoftDeletable
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public Guid FoodId { get; set; }
        public Food Food { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
