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
            float weight, IReadOnlyList<Category> categories)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Weight = weight;
            Categories = categories;
        }

        public Guid Id { get; }

        public string Name { get; }

        public string Description { get; }

        public decimal Price { get; }

        public float Weight { get; }


        // Навигационное свойство
        public IReadOnlyList<Category> Categories { get; }
    }
}
