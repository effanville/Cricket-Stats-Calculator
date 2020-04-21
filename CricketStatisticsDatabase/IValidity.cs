using System.Collections.Generic;

namespace Validation
{
    public class ValidationResult
    {
        /// <summary>
        /// whether the result is valid or not.
        /// </summary>
        public bool IsValid{ get; set; }

        /// <summary>
        /// The name of the property that is valid or not.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// The first message about the validity.
        /// </summary>
        public string GetMessage()
        {
            if (Messages.Count > 0)
            {
                return Messages[0];
            }

            return null;
        }

        /// <summary>
        /// All messages about the validity.
        /// </summary>
        public List<string> Messages = new List<string>();

        public void AddMessage(string message)
        {
            Messages.Add(message);
        }

    }
    public interface IValidity
    {
        /// <summary>
        /// Enacts the validity and returns whether valid or not.
        /// </summary>
        bool Validate();

        /// <summary>
        /// Returns the list of errors from the validation.
        /// </summary>
        List<ValidationResult> Validation();
    }
}
