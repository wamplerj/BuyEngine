namespace BuyEngine.Common;

public static class Guard
{
    public static void Null<T>(T t, string parameterName) where T : class
    {
        if (t is null)
            throw new ArgumentNullException(nameof(parameterName), $"{nameof(parameterName)} can not be null");
    }

    public static void Empty<T>(T t, string parameterName) where T : ICollection<T>
    {
        if (t is null || t.Count == 0)
            throw new ArgumentException($"{nameof(parameterName)} can not be empty", nameof(parameterName));
    }

    public static void Empty(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{nameof(parameterName)} can not be empty", nameof(parameterName));
    }

    public static void Negative(int value, string parameterName)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException($"{nameof(parameterName)} must be a positve number or zero", parameterName);
    }

    public static void NegativeOrZero(int value, string parameterName)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException($"{nameof(parameterName)} must be a positive number greater then zero", parameterName);
    }

    public static void Default<T>(T value, string parameterName)
    {
        if (EqualityComparer<T>.Default.Equals(value, default))
            throw new ArgumentException($"{nameof(parameterName)} can not be a default value", parameterName);
    }
}