using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto.UserDtos;
using Domain.Models;
using Domain.Result;

namespace Domain.Interfaces.Services
{
    public interface IUserService
    {
        public Task<BaseResult<User>> GetUserByIdAsync(Guid id);

        public Task<BaseResult<User>> GetUserByNameAsync(string name);

        public Task<CollectionResult<User>> GetAllUsersAsync();

        public Task<BaseResult<User>> CreateUserAsync(CreateUserDto dto);

        public Task<BaseResult<string>> ChangeUserLoginAsync(Guid id, string newLogin);

        public Task<BaseResult<string>> ChangeUserEmailAsync(Guid id, string newEmail);

        public Task<BaseResult<string>> ChangeUserPasswordAsync(Guid id, string oldPassword, string newPassword);

        public Task<BaseResult<decimal>> AddMoneyToBalanceAsync(Guid id, decimal increaseSumm);

        public Task<BaseResult<Guid>> DeleteUserByIdAsync(Guid id);
    }
}