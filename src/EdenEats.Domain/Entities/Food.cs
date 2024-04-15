using EdenEats.Domain.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Domain.Entities
{
    public sealed class Food : BaseEntity, ISoftDeletable
    {
        public Guid Id { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public double Weight { get; set; }
        public int QuantityAvailable { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;
        public int NutritionalInformationId { get; set; }
        public NutritionalInformation NutritionalInformation { get; set; } = null!;
        public ICollection<Instruction>? Instructions { get; set; }
        public ICollection<FoodIngredient>? FoodIngredients { get; set; }
        public ICollection<Ingredient>? Ingredients { get; set; }
        public ICollection<OrderFood>? OrderFoods { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<FoodComment>? Comments { get; set; }
        public bool IsDeleted { get ; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
