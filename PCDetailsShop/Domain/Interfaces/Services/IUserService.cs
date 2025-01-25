using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto.UserDtos;
using Domain.Result;

namespace Domain.Interfaces.Services
{
    public interface IUserService
    {
        public Task<BaseResult<UserDto>> GetUserByIdAsync(Guid id);

        public Task<BaseResult<UserDto>> CreateUserAsync(CreateUserDto dto);

        public Task<BaseResult<UserDto>> UpdateUserAsync(UpdateUserDto dto);

        public Task<BaseResult<UserDto>> DeleteUserByIdAsync(Guid id);

        public Task<BaseResult<UserDto>> IncreaseUserWalletBalanceAsync(Guid id, decimal increaseSumm);
    }
}