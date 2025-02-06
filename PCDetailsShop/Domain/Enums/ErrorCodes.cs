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

        TheOldLoginDoesNotMatchTheLogin = 18,
        TheOldEmailDoesNotMatchThePassword = 19,
        TheOldPasswordDoesNotMatchThePassword = 20,

        TheAmountToBeCreditedMustBeGreaterThanZero = 21,

        TheAmountToBeCreditedMustBeANumber = 22,

        NotEnoughFundsInTheUsersBalance = 23,


        CartNotFound = 31,
        CartsNotFound = 32,



        ProductNotFound = 41,
        ProductsNotFound = 42,
        ProductAlreadyExists = 43,


        CategoryNotFound = 51,
        CategoriesNotFound = 52,
        CategoryWithTurnedNameAlreadyExists = 53,

        TheCategoryShouldNotContainRepetitiveProducts = 54,

        ThereIsNoSuchProductInTheCategory = 55,


        CharacteristicNotFound = 61,
        CharacteristicsNotFound = 62,
    }
}