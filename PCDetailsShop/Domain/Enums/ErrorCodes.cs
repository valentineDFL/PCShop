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
        TheOldPasswordDoesNotMatchThePassword = 18,

        TheAmountToBeCreditedMustBeGreaterThanZero = 19,

        CartNotFound = 21,
        CartsNotFound = 22,



        ProductNotFound = 31,
        ProductsNotFound = 32,
        ProductAlreadyExists = 33,


        CategoryNotFound = 41,
        CategoriesNotFound = 42,
        
    }
}