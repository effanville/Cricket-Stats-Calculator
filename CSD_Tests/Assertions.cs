using NUnit.Framework;
using System.Collections.Generic;
using Validation;

namespace CSD_Tests
{
    public static class Assertions
    {
        /// <summary>
        /// Asserts that two matrices are equal up to the given tolerance.
        /// </summary>
        public static void AreEqual(double[,] expected, double[,] actual, double tol = 1e-8, string message = null)
        {
            if (!expected.GetLength(0).Equals(actual.GetLength(0)))
            {
                throw new AssertionException($"Number of rows not the same. Expected {expected.GetLength(0)} but actually {actual.GetLength(0)}");
            }

            if (!expected.GetLength(1).Equals(actual.GetLength(1)))
            {
                throw new AssertionException($"Number of columns not the same. Expected {expected.GetLength(1)} but actually {actual.GetLength(1)}");
            }
            for (int rowIndex = 0; rowIndex < expected.GetLength(0); rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < expected.GetLength(1); columnIndex++)
                {
                    Assert.AreEqual(expected[rowIndex, columnIndex], actual[rowIndex, columnIndex], tol, message);
                }
            }
        }

        /// <summary>
        /// Asserts that two matrices are equal up to the given tolerance.
        /// </summary>
        public static void AreEqual(double[] expected, double[] actual, double tol = 1e-8, string message = null)
        {
            if (!expected.GetLength(0).Equals(actual.GetLength(0)))
            {
                throw new AssertionException($"Number of rows not the same. Expected {expected.GetLength(0)} but actually {actual.GetLength(0)}");
            }

            for (int rowIndex = 0; rowIndex < expected.GetLength(0); rowIndex++)
            {
                Assert.AreEqual(expected[rowIndex], actual[rowIndex], tol, message);
            }
        }

        public static void AreEqualResults(List<ValidationResult> expected, List<ValidationResult> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                AreEqualResults(expected[i], actual[i]);
            }
        }

        public static void AreEqualResults(ValidationResult expected, ValidationResult actual)
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
