namespace BuyEngine.Common;

public class ValidationException : Exception
{
    public ValidationException(ValidationResult validationResult, string model)
    {
        ValidationResult = validationResult;
        Model = model;
    }

    public ValidationResult ValidationResult { get; }

    public string Model { get; }
}