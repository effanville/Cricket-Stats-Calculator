namespace StructureCommon.Validation
{
    public static class Validating
    {
        public static ValidationResult NotNegative(double value, string propertyName, string location)
        {
            if (value < 0)
            {
                var notNegativeResult = new ValidationResult(isValid: false, propertyName, location);
                notNegativeResult.AddMessage($"{propertyName} cannot take a negative value.");
                return notNegativeResult;
            }

            return null;
        }

        public static ValidationResult NotLessThan(double value, double lowerLimit, string propertyName, string location)
        {
            if (value < lowerLimit)
            {
                var notLessThanResult = new ValidationResult(isValid: false, propertyName, location);
                notLessThanResult.AddMessage($"{propertyName} cannot take values below {lowerLimit}.");
                return notLessThanResult;
            }

            return null;
        }

        public static ValidationResult NotGreaterThan(double value, double upperLimit, string propertyName, string location)
        {
            if (value > upperLimit)
            {
                var notMoreThanResult = new ValidationResult(isValid: false, propertyName, location);
                notMoreThanResult.AddMessage($"{propertyName} cannot take values above {upperLimit}.");
                return notMoreThanResult;
            }

            return null;
        }

        public static ValidationResult NotEqualTo(double value, double expected, string propertyName, string location)
        {
            if (!value.Equals(expected))
            {
                var notEqualTo = new ValidationResult(isValid: false, propertyName, location);
                notEqualTo.AddMessage($"{propertyName} was expected to be equal to {expected}.");
                return notEqualTo;
            }

            return null;
        }

        public static ValidationResult IsNotNullOrEmpty(string value, string propertyName, string location)
        {
            if (string.IsNullOrEmpty(value))
            {
                var result = new ValidationResult(isValid: false, propertyName, location);
                result.AddMessage($"{propertyName} cannot be empty or null.");
                return result;
            }

            return null;
        }
    }
}
