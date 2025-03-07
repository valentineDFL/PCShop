using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;
using Domain.Models;
using Domain.Result;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<List<User>> GetAllAsync();

        public Task<(User User, ErrorCodes ErrorCode)> GetByIdAsync(Guid id);

        public Task<(User User, ErrorCodes ErrorCode)> GetByLoginAsync(string name);

        public Task<(User User, ErrorCodes ErrorCode)> GetByEmailAsync(string email);

        public Task<List<Domain.Enums.Permissions>> GetPermissionsAsync(Guid userId);

        public Task<User> CreateAsync(User user);

        public Task<int> ChangeLoginAsync(Guid id, string newLogin);

        public Task<int> ChangeEmailAsync(Guid id, string newEmail);

        public Task<int> ChangePasswordAsync(Guid id, string newPassword);

        public Task<int> AddMoneyToBalance(Guid id, decimal increaseSumm);

        public Task<int> DeleteAsync(Guid id);
    }
}
