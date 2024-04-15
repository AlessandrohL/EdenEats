using EdenEats.Domain.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Domain.Entities
{
    public sealed class District : BaseEntity, ISoftDeletable
    {
        public int Id { get; set; }
        public int ProvinceId { get; set; }
        public Province? Province { get; set; }
        public string? Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
