using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;
using Domain.Interfaces.Validators;
using Domain.Models;
using Domain.Result;

namespace Application.Validation.CategoryValidations
{
    internal class CategoryValidator : ICategoryValidator
    {
        public BaseResult<Category> ValidateOnNameExists(Category category)
        {
            if(category != null)
            {
                return new BaseResult<Category>()
                {
                    ErrorCode = (int)ErrorCodes.CategoryWithTurnedNameAlreadyExists,
                    ErrorMessage = ErrorCodes.CategoryWithTurnedNameAlreadyExists.ToString()
                };
            }

            return new BaseResult<Category>();
        }

        public BaseResult<Product> ValidateOnProductRepeat(Category category, Product productToAdd)
        {
            if (category.Products.Contains(productToAdd))
            {
                return new BaseResult<Product>()
                {
                    ErrorCode = (int)ErrorCodes.TheCategoryShouldNotContainRepetitiveProducts,
                    ErrorMessage = ErrorCodes.TheCategoryShouldNotContainRepetitiveProducts.ToString()
                };
            }

            return new BaseResult<Product>();
        }

        public BaseResult<Product> ValidateOnProductExistsInCategory(Category category, Product product)
        {
            if (category.Products.Contains(product))
                return new BaseResult<Product>();

            return new BaseResult<Product>()
            {
                ErrorCode = (int)ErrorCodes.ThereIsNoSuchProductInTheCategory,
                ErrorMessage = ErrorCodes.ThereIsNoSuchProductInTheCategory.ToString()
            };
        }
    }
}
