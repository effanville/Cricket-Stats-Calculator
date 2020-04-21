namespace Validation
{
    public static class Validating
    {
        public static ValidationResult NotNegative(double value, string propertyName)
        {
            if (value < 0)
            {
                var notNegativeResult = new ValidationResult();
                notNegativeResult.IsValid = false; 
                notNegativeResult.PropertyName = propertyName;
                notNegativeResult.AddMessage($"{propertyName} cannot take a negative value.");
                return notNegativeResult;
            }

            return null;
        }

        public static ValidationResult NotLessThan(double value, double lowerLimit, string propertyName)
        {
            if (value > lowerLimit)
            {
                var notLessThanResult = new ValidationResult();
                notLessThanResult.IsValid = false;
                notLessThanResult.PropertyName = propertyName;
                notLessThanResult.AddMessage($"{propertyName} cannot take values below {lowerLimit}.");
                return notLessThanResult;
            }

            return null;
        }

        public static ValidationResult NotGreaterThan(double value, double upperLimit, string propertyName)
        {
            if (value > upperLimit)
            {
                var notMoreThanResult = new ValidationResult();
                notMoreThanResult.IsValid = false;
                notMoreThanResult.PropertyName = propertyName;
                notMoreThanResult.AddMessage($"{propertyName} cannot take values above {upperLimit}.");
                return notMoreThanResult;
            }

            return null;
        }

        public static ValidationResult IsNotNullOrEmpty(string value, string propertyName)
        {
            if (string.IsNullOrEmpty(value))
            {
                var result = new ValidationResult();
                result.IsValid = false;
                result.PropertyName = propertyName;
                result.AddMessage($"{propertyName} cannot be empty or null.");
                return result;
            }

            return null;
        }
    }
}
