using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Result;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<CollectionResult<User>> GetAllAsync();

        public Task<BaseResult<User>> GetByIdAsync(Guid id);

        public Task<BaseResult<User>> GetByLoginAsync(string name);

        public Task<BaseResult<User>> CreateAsync(User user);

        public Task<BaseResult<string>> ChangeLoginAsync(Guid id, string newLogin);

        public Task<BaseResult<string>> ChangeEmailAsync(Guid id, string newEmail);

        public Task<BaseResult<string>> ChangePasswordAsync(Guid id, string newPassword);

        public Task<BaseResult<decimal>> AddMoneyToBalance(Guid id, decimal increaseSumm);

        public Task<BaseResult<Guid>> DeleteAsync(Guid id);
    }
}
