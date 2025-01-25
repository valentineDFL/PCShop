using System;

namespace DataAccessLayer.Entities
{
    internal class UserEntity
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }
        
        public string Password { get; set; }

        public decimal WalletBalance { get; set; }

        public DateTime BirthDate { get ; set; }

        public DateTime RegistrationDate { get; set; }
        
        // Навигационное свойство
        public CartEntity Cart { get; set; }

        // Внешний ключ
        public Guid CartId { get; set; }
    }
}