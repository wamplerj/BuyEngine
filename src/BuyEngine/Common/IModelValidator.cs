namespace BuyEngine.Common;

public interface IModelValidator<in T>
{
    async Task<bool> IsValidAsync(T t) => (await ValidateAsync(t)).IsValid;
    async Task<bool> IsInvalidAsync(T t) => (await ValidateAsync(t)).IsInvalid;
    Task<ValidationResult> ValidateAsync(T model);

    async Task ThrowIfInvalidAsync(T model, string modelName)
    {
        var result = await ValidateAsync(model);
        if (result.IsValid) return;

        throw new ValidationException(result, modelName);
    }
}