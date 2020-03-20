namespace BuyEngine.Common
{
    public interface IModelValidator<in T>
    {
        ValidationResult IsValid(T t);
    }
}
