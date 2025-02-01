using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Characteristic
    {
        public Characteristic(Guid id, string name, string value)
        {
            Id = id;
            Name = name;
            Value = value;
        }

        public Guid Id { get; }

        public string Name { get; }

        public string Value { get; }
    }
}
