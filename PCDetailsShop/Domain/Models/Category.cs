using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Category
    {
        public Category(Guid id, string name, IReadOnlyList<Product> products, IReadOnlyList<Characteristic> characteristics)
        {
            Id = id;
            Name = name;
            Products = products;
            Characteristics = characteristics;
        }

        public Guid Id { get; }

        public string Name { get; }

        public IReadOnlyList<Product> Products { get; }

        public IReadOnlyList<Characteristic> Characteristics { get; }
    }
}
