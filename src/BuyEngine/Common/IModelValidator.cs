using System.Threading.Tasks;

namespace BuyEngine.Common
{
    public interface IModelValidator<in T>
    {
        async Task<bool> IsValidAsync(T t) => (await ValidateAsync(t)).IsValid;
        Task<ValidationResult> ValidateAsync(T brand);
    }
}
