using Domain.Dto.UserDtos;
using Domain.Enums;
using Domain.Models;
using Domain.Result;

namespace Domain.Interfaces.Validators
{
    public interface IUserValidator : IBaseValidator<User>
    {
        public BaseResult ExistsValidation(User user, CreateUserDto dto);
    }
}