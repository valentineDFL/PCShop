using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Product
    {
        public Product(Guid id, string name, string description, decimal price,
            float weight, IReadOnlyList<Category> categories, 
            List<CharacteristicRealization> characteristicsRealization, int amount)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Weight = weight;
            Categories = categories;
            CharacteristicsRelization = characteristicsRealization;
            Amount = amount;
        }

        public Guid Id { get; }

        public string Name { get; }

        public string Description { get; }

        public decimal Price { get; }

        public float Weight { get; }

        public bool StockAvailability => Amount > 0;

        public int Amount { get; }


        // Навигационное свойство
        public IReadOnlyList<Category> Categories { get; }

        public IReadOnlyList<CharacteristicRealization> CharacteristicsRelization { get; }  
    }
}