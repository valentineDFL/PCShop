using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class CharacteristicPattern
    {
        public CharacteristicPattern() { }

        public CharacteristicPattern(Guid id, string name, Guid categoryId, Category category, 
            List<CharacteristicRealization> realizations)
        {
            Id = id;
            Name = name;
            CategoryId = categoryId;
            Category = category;
            Realizations = realizations;
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public Guid CategoryId { get; private set; }

        public Category Category { get; private set; }

        public List<CharacteristicRealization> Realizations { get; private set; }
    }
}