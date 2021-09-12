using System.Collections.Generic;
using System.Linq;

namespace BuyEngine.Common
{
    public class ValidationResult
    {

        public ValidationResult()
        {
            Messages = new Dictionary<string, string>();
        }
        
        public bool IsValid => !Messages.Any();
        public bool IsInvalid => Messages.Any();

        public void ThrowIfInvalid(string modelName)
        {
           
        }

        public IDictionary<string, string> Messages { get; }

        public void AddMessage(string name, string message)
        {
            Messages.Add(name, message);
        }
    }
}
