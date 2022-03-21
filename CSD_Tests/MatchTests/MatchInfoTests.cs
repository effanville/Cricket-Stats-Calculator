using System.Collections.Generic;
using Cricket.Match;
using CSD_Tests;
using NUnit.Framework;
using Common.Structure.Validation;

namespace CricketClasses.MatchTests
{
    [TestFixture]
    public class MatchInfoTests
    {
        [TestCase("", false)]
        [TestCase(null, false)]
        [TestCase("Sam", true)]
        public void ValidityTests(string opposition, bool isValid)
        {
            var info = new MatchInfo
            {
                Opposition = opposition
            };
            var result = info.Validate();
            Assert.AreEqual(isValid, result);
        }

        [TestCase("", false, new string[] { "Opposition cannot be empty or null." })]
        [TestCase(null, false, new string[] { "Opposition cannot be empty or null." })]
        [TestCase("Sam", true, new string[] { })]
        public void ValidityMessageTests(string opposition, bool isValid, string[] messages)
        {
            var info = new MatchInfo
            {
                Opposition = opposition
            };
            var valid = info.Validation();

            var expectedList = new List<ValidationResult>();
            if (!isValid)
            {
                var expected = new ValidationResult
                {
                    IsValid = isValid
                };
                expected.Messages.AddRange(messages);
                expectedList.Add(expected);
            }

            Assertions.ValidationListsEqual(expectedList, valid);
        }
    }
}
