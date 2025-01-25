using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Result;

namespace Domain.Interfaces.Validators
{
    public interface IProductValidator : IBaseValidator<Product>
    {
        public BaseResult CreateValidator(Product entity);
    }
}