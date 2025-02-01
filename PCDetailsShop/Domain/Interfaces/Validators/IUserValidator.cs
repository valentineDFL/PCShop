using Domain.Dto.UserDtos;
using Domain.Enums;
using Domain.Models;
using Domain.Result;

namespace Domain.Interfaces.Validators
{
    public interface IUserValidator
    {
        public BaseResult<User> ValidateOnExists(User user, CreateUserDto dto);

        public BaseResult<string> ValidateOnLoginExists(User user, string newLogin);

        public BaseResult<string> ValidateOnEmailExists(User user, string newEmail);

        public BaseResult<string> ValidateOnLoginRepeat(string oldLogin, string newLogin);

        public BaseResult<string> ValidateOnEmailRepeat(string oldEmail, string newEmail);

        public BaseResult<string> ValidateOnPasswordRepeat(string userPassword, string oldPassword);

        public BaseResult<decimal> ValidateOnCredit(decimal creditToAmount);
    }
}