using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto.UserDto;
using Domain.Dto.UserDtos;
using Domain.Models;
using Domain.Result;

namespace Domain.Interfaces.Services
{
    public interface IUserService
    {
        public Task<BaseResult<User>> GetByIdAsync(Guid id);

        public Task<BaseResult<User>> GetByNameAsync(string name);

        public Task<CollectionResult<User>> GetAllAsync();

        public Task<BaseResult<User>> CreateAsync(CreateUserDto dto);

        public Task<BaseResult<string>> LoginAsync(LoginUserDto loginInfo);

        public Task<BaseResult<string>> ChangeLoginAsync(Guid id, string newLogin);

        public Task<BaseResult<string>> ChangeEmailAsync(Guid id, string newEmail);

        public Task<BaseResult<string>> ChangePasswordAsync(Guid id, string oldPassword, string newPassword);

        public Task<BaseResult<decimal>> AddMoneyToBalanceAsync(Guid id, decimal increaseSumm);

        public Task<BaseResult<Guid>> DeleteByIdAsync(Guid id);
    }
}