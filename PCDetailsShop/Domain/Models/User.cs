using System;

namespace Domain.Models
{
    public class User
    {
        public User() { }

        public User(Guid id, string login, string email, string password, decimal walletBalance, 
            DateTime birthDate, DateTime registrationDate, Cart cart, Guid cartId)
        {
            Id = id;
            Login = login;
            Email = email;
            Password = password;
            WalletBalance = walletBalance;
            BirthDate = birthDate;
            RegistrationDate = registrationDate;
            Cart = cart;
            CartId = cartId;
        }

        public Guid Id { get; private set; }

        public string Login { get; private set; }

        public string Email { get; private set; }
        
        public string Password { get; private set; }

        public decimal WalletBalance { get; private set; }

        public DateTime BirthDate { get; private set; }

        public DateTime RegistrationDate { get; private set; }
        
        // Навигационное свойство
        public Cart Cart { get; private set; }

        // Внешний ключ
        public Guid CartId { get; private set; }

        public List<Role> Roles { get; private set; }
    }
}