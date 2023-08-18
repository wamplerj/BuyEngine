namespace BuyEngine.Common;

public static class Guard
{
    public static void Null<T>(T t, string parameterName) where T : class
    {
        if (t is null)
            throw new ArgumentNullException(parameterName, $"{nameof(parameterName)} can not be null");
    }

    public static void Empty(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{parameterName} can not be empty", parameterName);
    }

    public static void Negative(int value, string parameterName)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(parameterName, $"{parameterName} must be a positive number or zero");
    }

    public static void NegativeOrZero(int value, string parameterName)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(parameterName, $"{nameof(parameterName)} must be a positive number greater then zero");
    }

    public static void Default<T>(T value, string parameterName)
    {
        if (EqualityComparer<T>.Default.Equals(value, default))
            throw new ArgumentException($"{parameterName} can not be a default value", parameterName);
    }
}