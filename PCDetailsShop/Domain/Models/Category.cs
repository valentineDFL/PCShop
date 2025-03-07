using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Category
    {
        public Category() { }

        public Category(Guid id, string name, List<Product> products, List<CharacteristicPattern> characteristicPatterns)
        {
            Id = id;
            Name = name;
            Products = products;
            CharacteristicPatterns = characteristicPatterns;
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public List<Product> Products { get; private set; }

        public List<CharacteristicPattern> CharacteristicPatterns { get; private set; }
    }
}