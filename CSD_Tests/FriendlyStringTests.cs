using System.IO;

using NUnit.Framework;

namespace CricketStructures.Tests
{
    internal class FriendlyStringTests
    {
        [TestCase("HighestODIChase")]
        public void DoStuff(string index)
        {
            var matchToTest = TestCaseInstances.ExampleMatches[index];
            var friendlyString = matchToTest.SerializeToString(Common.Structure.ReportWriting.DocumentType.Html);
            var stringThing = friendlyString.ToString();
            File.WriteAllText($"c:\\data\\source\\test{index}.html", stringThing);
        }
    }
}
