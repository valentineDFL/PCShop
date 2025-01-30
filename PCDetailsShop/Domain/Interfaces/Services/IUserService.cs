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

        public Task<CollectionResult<User>> GetAllUsersAsync();

        public Task<BaseResult<User>> CreateUserAsync(CreateUserDto dto);

        public Task<BaseResult<User>> UpdateUserAsync(UpdateUserDto dto);

        public Task<BaseResult<Guid>> DeleteUserByIdAsync(Guid id);
    }
}