using System.Collections.Generic;
using NUnit.Framework;
using Common.Structure.Validation;

namespace CricketStructures.Tests
{
    public static class Assertions
    {
        public static void ValidationListsEqual(List<ValidationResult> expected, List<ValidationResult> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                ValidationEqual(expected[i], actual[i]);
            }
        }

        public static void ValidationEqual(ValidationResult expected, ValidationResult actual)
        {
            if (expected == null)
            {
                Assert.IsNull(actual);
            }

            if (actual == null)
            {
                Assert.IsNull(expected);
            }

            if (expected != null && actual != null)
            {
                Assert.AreEqual(expected.IsValid, actual.IsValid, "Both results should be valid or not valid.");
                Assert.AreEqual(expected.Messages, actual.Messages);
            }
        }
    }
}
