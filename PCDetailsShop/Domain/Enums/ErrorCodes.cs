using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum ErrorCodes
    {
        InternalServerError = 0,

        UserNotFound = 11,
        UsersNotFound = 12,

        UserAlreadyExists = 13,
        UserWithTurnedLoginAlreadyExists = 14,
        UserWithTurnedEmailAlreadyExists = 15,
        UserWithTurnerdEmailAndLoginAlreadyExists = 16,

        NewUserPasswordCannotBeRepeatedWithTheOldOne = 17,

        ProductNotFound = 21,
        ProductsNotFound = 22,
        ProductAlreadyExists = 23,


        CategoryNotFound = 31,
        
    }
}