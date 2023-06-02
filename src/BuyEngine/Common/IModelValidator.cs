namespace BuyEngine.Common;

public interface IModelValidator<in T>
{
    Task<ValidationResult> ValidateAsync(T model);

    async Task ThrowIfInvalidAsync(T model, string modelName)
    {
        var result = await ValidateAsync(model);
        if (result.IsValid) return;

        throw new ValidationException(result, modelName);
    }
}