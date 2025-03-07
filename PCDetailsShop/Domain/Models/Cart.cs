using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Cart
    {
        public Cart() { }

        public Cart(Guid id, decimal cartTotalPrice, float cartTotalWeight, User user, 
            Guid userId, List<Product> products)
        {
            Id = id;
            CartTotalPrice = cartTotalPrice;
            CartTotalWeight = cartTotalWeight;
            User = user;
            UserId = userId;
            Products = products;
        }

        public Guid Id { get; private set; }

        public decimal CartTotalPrice { get; set; }

        public float CartTotalWeight { get; set; }

        // Навигационное свойство
        public User User { get; private set; }

        // Внешний ключ
        public Guid UserId { get; private set; }

        // Навигационное свойство
        public List<Product> Products { get; private set; }

    }
}