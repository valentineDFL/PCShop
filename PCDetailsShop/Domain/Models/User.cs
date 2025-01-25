
namespace Domain.Models
{
    public class User
    {
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

        public Guid Id { get; }

        public string Login { get; }

        public string Email { get; }

        public string Password { get; }

        public decimal WalletBalance { get; }

        public DateTime BirthDate { get; }

        public DateTime RegistrationDate { get; }

        // Навигационное свойство
        public Cart Cart { get; }

        // Внешний ключ
        public Guid CartId { get; }
    }
}