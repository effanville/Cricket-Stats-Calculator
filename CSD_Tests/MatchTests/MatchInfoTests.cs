using System;
using System.Collections.Generic;
using CricketStructures.Match;
using NUnit.Framework;
using Common.Structure.Validation;

namespace CricketStructures.Tests.MatchTests
{
    [TestFixture]
    public class MatchInfoTests
    {
        [TestCase("", "", "", "2020/1/1", false)]
        [TestCase(null, "walkern", "", "2020/1/1", false)]
        [TestCase("Sam", "", "", "2020/1/1", false)]
        [TestCase("Sam", "walkern", "", "2020/1/1", true)]
        public void ValidityTests(string homeTeam, string awayTeam, string location, DateTime date, bool isValid)
        {
            var info = new MatchInfo
            {
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                Location = location,
                Date = date
            };

            bool result = info.Validate();
            Assert.AreEqual(isValid, result);
        }

        [TestCase("", "", "", "2020/1/1", false, new string[] { "HomeTeam cannot be empty or null.", "AwayTeam cannot be empty or null." })]
        [TestCase(null, "walkern", "", "2020/1/1", false, new string[] { "HomeTeam cannot be empty or null." })]
        [TestCase("Sam", "", "", "2020/1/1", false, new string[] { "AwayTeam cannot be empty or null." })]
        [TestCase("Sam", "walkern", "", "2020/1/1", true, new string[] { })]
        public void ValidityMessageTests(string homeTeam, string awayTeam, string location, DateTime date, bool isValid, string[] messages)
        {
            var info = new MatchInfo
            {
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                Location = location,
                Date = date
            };
            var valid = info.Validation();

            var expectedList = new List<ValidationResult>();

            Array.ForEach(messages, message =>
            {
                if (!isValid)
                {
                    var expected = new ValidationResult
                    {
                        IsValid = isValid,
                    };
                    expected.AddMessage(message);
                    expectedList.Add(expected);
                }
            });

            Assertions.ValidationListsEqual(expectedList, valid);
        }
    }
}
