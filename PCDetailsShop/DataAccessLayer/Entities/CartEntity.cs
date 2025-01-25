using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    internal class CartEntity
    {
        public Guid Id { get; set; }

        public decimal CartTotalPrice { get; set; }

        public float CartTotalWeight { get; set; }

        // Навигационное свойство
        public UserEntity User { get; set; }

        // Внешний ключ
        public Guid UserId { get; set; }

        // Навигационное свойство
        public List<ProductEntity> Products { get; set; }
    }
}