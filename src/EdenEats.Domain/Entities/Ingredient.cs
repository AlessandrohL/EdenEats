using EdenEats.Domain.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Domain.Entities
{
    public sealed class Ingredient : BaseEntity, ISoftDeletable
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<FoodIngredient>? FoodIngredients { get; set; }
        public ICollection<Food>? Foods { get; set; }
        public ICollection<IngredientAllergen>? IngredientAllergens { get; set; }
        public ICollection<Allergen>? Allergens { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
