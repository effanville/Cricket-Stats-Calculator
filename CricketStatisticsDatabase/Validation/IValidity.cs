using System.Collections.Generic;

namespace StructureCommon.Validation
{
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
