using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Cart
    {
        public Cart(Guid id, decimal cartTotalPrice, float cartTotalWeight, 
            User user, Guid userId, List<Product> products)
        {
            Id = id;
            CartTotalPrice = cartTotalPrice;
            CartTotalWeight = cartTotalWeight;
            User = user;
            UserId = userId;
            Products = products;
        }

        public Guid Id { get; }

        public decimal CartTotalPrice { get; }

        public float CartTotalWeight { get; }

        // Навигационное свойство
        public User User { get; }

        // Внешний ключ
        public Guid UserId { get; }

        // Навигационное свойство
        public IReadOnlyList<Product> Products { get; }
    }
}
