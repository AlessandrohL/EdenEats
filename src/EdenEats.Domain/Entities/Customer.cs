using EdenEats.Domain.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Domain.Entities
{
    public sealed class Customer : BaseEntity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string Phone { get; set; } = null!;
        public Guid IdentityId { get; private set; }
        public ICollection<Address>? Addresses { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<FoodComment>? Comments { get; set; }

        public void AssignIdentity(Guid identityId)
        {
            if (identityId == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(identityId)} cannot be an empty GUID.");
            }

            IdentityId = identityId;
        }
    }
}
