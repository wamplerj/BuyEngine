using System.Threading.Tasks;

namespace BuyEngine.Common
{
    public interface IModelValidator<in T>
    {
        ValidationResult IsValid(T t);
        Task<ValidationResult> IsValidAsync(T t);
    }
}
