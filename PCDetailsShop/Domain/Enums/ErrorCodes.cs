using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum ErrorCodes
    {
        None = 0,
        InternalServerError = 1,


        UserNotFound = 11,
        UsersNotFound = 12,
        UserPasswordDoesNotMatch = 13,

        InsufficientFunds = 14,

        UserAlreadyExists = 15,
        UserWithTurnedLoginAlreadyExists = 16,
        UserWithTurnedEmailAlreadyExists = 17,
        UserWithTurnerdEmailAndLoginAlreadyExists = 18,

        NewUserPasswordCannotBeRepeatedWithTheOldOne = 19,

        TheOldLoginDoesNotMatchTheLogin = 20,
        TheOldEmailDoesNotMatchThePassword = 21,
        TheOldPasswordDoesNotMatchThePassword = 22,

        TheAmountToBeCreditedMustBeGreaterThanZero = 23,

        TheAmountToBeCreditedMustBeANumber = 24,

        NotEnoughFundsInTheUsersBalance = 25,


        CartNotFound = 31,
        CartsNotFound = 32,
        CartDoesNotContainSelectedProduct = 33,

        ProductNotFound = 41,
        ProductsNotFound = 42,
        ProductAlreadyExists = 43,

        ProductPriceMustBeGreaterThanZero = 44,

        ProductWeightMustBeGreaterThanZero = 45,
        ProductContainTurnedCategory = 46,
        ProductDontContainTurnedCategory = 47,

        NewProductCountMustBeGraaterThanZero = 47,

        CategoryNotFound = 51,
        CategoriesNotFound = 52,
        CategoryWithTurnedNameAlreadyExists = 53,


        CharacteristicNotFound = 61,
        CharacteristicsNotFound = 62,

        CharacteristicWithTurnedNameAlreadyExists = 63,

        CharacteristicRealizationNotFound = 64,
        
        CharacteristicRealizationsNotFound = 65,

        TheNumberOfImplementationsDoesNotMatchTheNumberOfTemplates = 71,
        
        TheNumberOfRealizationsOfTheCategoryPatternsDoesNotCorrespondToTheRequiredNumberOfPatterns = 72,

        CategoryDoesNotContainSelectedCharacteristic = 73,
    }
}