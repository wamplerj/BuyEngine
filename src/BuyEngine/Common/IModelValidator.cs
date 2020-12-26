namespace BuyEngine.Common
{
    public interface IModelValidator<in T>
    {
        bool IsValid(T t) => Validate(t).IsValid;
        ValidationResult Validate(T brand);
    }
}
