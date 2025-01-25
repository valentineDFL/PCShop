using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Category
    {
        public Category(Guid id, string name, IReadOnlyList<Product> products)
        {
            Id = id;
            Name = name;
            Products = products;
        }

        public Guid Id { get; }

        public string Name { get; }

        public IReadOnlyList<Product> Products { get; }
    }
}
