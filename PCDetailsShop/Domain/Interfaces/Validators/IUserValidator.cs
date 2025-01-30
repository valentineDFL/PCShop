using Domain.Dto.UserDtos;
using Domain.Enums;
using Domain.Models;
using Domain.Result;

namespace Domain.Interfaces.Validators
{
    public interface IUserValidator : IBaseValidator<User>
    {
        public BaseResult<User> ExistsValidation(User user, CreateUserDto dto);

        public BaseResult<User> ExistsValidation(User user, UpdateUserDto dto);

        public BaseResult<User> PasswordValidation(string userPassword, string oldPassword);
    }
}