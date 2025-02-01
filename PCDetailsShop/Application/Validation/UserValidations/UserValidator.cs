using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dto.UserDtos;
using Domain.Enums;
using Domain.Interfaces.Validators;
using Domain.Models;
using Domain.Result;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Validation.UserValidations
{
    internal class UserValidator : IUserValidator
    {
        public BaseResult<User> ValidateOnExists(User user, CreateUserDto dto)
        {
            if (user != null)
            {
                int errorCode = 0;
                int count = 0;

                if (user.Login == dto.Login)
                {
                    errorCode = (int)ErrorCodes.UserWithTurnedLoginAlreadyExists;
                    count++;
                }
                else if (user.Email == dto.Email)
                {
                    errorCode = (int)ErrorCodes.UserWithTurnedEmailAlreadyExists;
                    count++;
                }

                if (count == 2)
                    errorCode = (int)ErrorCodes.UserWithTurnerdEmailAndLoginAlreadyExists;

                return new BaseResult<User>()
                {
                    ErrorCode = errorCode,
                    ErrorMessage = ErrorCodes.UserAlreadyExists.ToString()
                };
            }

            return new BaseResult<User>();
        }

        public BaseResult<string> ValidateOnLoginExists(User user, string newLogin)
        {
            if (user != null)
            {
                if (user.Login == newLogin)
                {
                    return new BaseResult<string>()
                    {
                        ErrorCode = (int)ErrorCodes.UserWithTurnedLoginAlreadyExists,
                        ErrorMessage = ErrorCodes.UserAlreadyExists.ToString()
                    };
                }
            }

            return new BaseResult<string>();
        }

        public BaseResult<string> ValidateOnEmailExists(User user, string newEmail)
        {
            if (user != null)
            {
                if (user.Login == newEmail)
                {
                    return new BaseResult<string>()
                    {
                        ErrorCode = (int)ErrorCodes.UserWithTurnedEmailAlreadyExists,
                        ErrorMessage = ErrorCodes.UserAlreadyExists.ToString()
                    };
                }
            }

            return new BaseResult<string>();
        }

        public BaseResult<string> ValidateOnLoginRepeat(string oldLogin, string newLogin)
        {
            if (oldLogin == newLogin)
            {
                return new BaseResult<string>()
                {
                    ErrorCode = (int)ErrorCodes.TheOldLoginDoesNotMatchTheLogin,
                    ErrorMessage = ErrorCodes.TheOldLoginDoesNotMatchTheLogin.ToString()
                };
            }

            return new BaseResult<string>();
        }

        public BaseResult<string> ValidateOnEmailRepeat(string oldEmail, string newEmail)
        {
            if(oldEmail == newEmail)
            {
                return new BaseResult<string>()
                {
                    ErrorCode = (int)ErrorCodes.TheOldEmailDoesNotMatchThePassword,
                    ErrorMessage = ErrorCodes.TheOldEmailDoesNotMatchThePassword.ToString()
                };
            }

            return new BaseResult<string>();
        }

        public BaseResult<string> ValidateOnPasswordRepeat(string userPassword, string oldPassword)
        {
            if (userPassword == oldPassword)
            {
                return new BaseResult<string>()
                {
                    ErrorCode = (int)ErrorCodes.TheOldPasswordDoesNotMatchThePassword,
                    ErrorMessage = ErrorCodes.TheOldPasswordDoesNotMatchThePassword.ToString(),
                };
            }

            return new BaseResult<string>();
        }

        public BaseResult<decimal> ValidateOnCredit(decimal creditToAmount)
        {
            if(creditToAmount < 0)
            {
                return new BaseResult<decimal>()
                {
                    ErrorCode = (int)ErrorCodes.TheAmountToBeCreditedMustBeGreaterThanZero,
                    ErrorMessage = ErrorCodes.TheAmountToBeCreditedMustBeGreaterThanZero.ToString()
                };
            }

            return new BaseResult<decimal>();
        }
    }
}
