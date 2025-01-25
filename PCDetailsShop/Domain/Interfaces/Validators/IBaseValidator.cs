using Domain.Result;

namespace Domain.Interfaces.Validators
{
    public interface IBaseValidator<T> where T : class
    {
        public BaseResult ValidateOnNull(T entity);
    }
}