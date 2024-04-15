using EdenEats.Domain.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Domain.Entities
{
    public sealed class Allergen : BaseEntity, ISoftDeletable
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<IngredientAllergen>? IngredientAllergens { get; set; }
        public ICollection<Ingredient>? Ingredients { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
