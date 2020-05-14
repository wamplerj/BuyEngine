using System.Collections.Generic;

namespace BuyEngine.Common
{
    public class ValidationResult
    {

        public ValidationResult()
        {
            IsValid = true;
            Messages = new Dictionary<string, string>();
        }

        public bool IsValid { get; private set; }

        public IDictionary<string, string> Messages { get; private set; }

        public void AddMessage(string name, string message)
        {
            if (IsValid) IsValid = false;

            Messages.Add(name, message);
        }
    }
}
