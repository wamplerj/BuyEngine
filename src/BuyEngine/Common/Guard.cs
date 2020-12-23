using System;

namespace BuyEngine.Common
{
    public static class Guard
    {

        public static void AgainstNull<T>(T t, string parameterName) where T : class
        {
            if (t is null)
                throw new ArgumentNullException(nameof(parameterName), $"{nameof(parameterName)} can not be null");
        }
    }
}
