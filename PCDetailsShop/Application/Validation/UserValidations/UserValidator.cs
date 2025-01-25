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

namespace Application.Validation.UserValidations
{
    internal class UserValidator : IUserValidator
    {
        public BaseResult ValidateOnNull(User entity)
        {
            if(entity == null)
            {
                return new BaseResult()
                {
                    ErrorCode = (int)ErrorCodes.UserNotFound,
                    ErrorMessage = ErrorCodes.UserNotFound.ToString(),
                };
            }

            return new BaseResult();
        }

        public BaseResult ExistsValidation(User user, CreateUserDto dto)
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

                return new BaseResult()
                {
                    ErrorCode = errorCode,
                    ErrorMessage = ErrorCodes.UserAlreadyExists.ToString()
                };
            }

            return new BaseResult();
        }
    }
}
