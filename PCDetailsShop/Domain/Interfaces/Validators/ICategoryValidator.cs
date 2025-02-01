using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Result;

namespace Domain.Interfaces.Validators
{
    public interface ICategoryValidator
    {
        public BaseResult<Category> ValidateOnNameExists(Category category);

        public BaseResult<Product> ValidateOnProductRepeat(Category category, Product productToAdd);

        public BaseResult<Product> ValidateOnProductExistsInCategory(Category category, Product product);
    }
}