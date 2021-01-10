using System;

namespace BuyEngine.Common
{
    public class ValidationException : Exception
    {
        public ValidationException(ValidationResult validationResult, string method)
        {
            ValidationResult = validationResult;
            Method = method;
        }

        public ValidationResult ValidationResult { get; }

        public string Method { get; } 
        
    }
}
