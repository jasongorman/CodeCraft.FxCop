using CodeCraft.FxCop.MaxCollaborators;
using NUnit.Framework;

namespace CodeCraft.FxCop.Tests
{
    [TestFixture]
    public class MaxCollaboratorsRuleTests
    {
        [TestCase("TooManyCollabs", 1)]
        [TestCase("ThreeCollabs", 0)]
        public void ClassesWithMoreThanThreeCollaboratorsBreakRule(string typeName, int expectedProblemCount)
        {
            var rule = new MaxCollaboratorsRule();
            rule.Check(AssemblyReader.GetType(typeName));
            Assert.That(rule.Problems.Count, Is.EqualTo(expectedProblemCount));
        }
    }

    public class TooManyCollabs
    {
        private Collab1 _collab1;
        private Collab2 _collab2;
        private Collab3 _collab3;
        private Collab4 _collab4;
    }

    public class ThreeCollabs
    {
        private Collab1 _collab1;
        private Collab2 _collab2;
        private Collab3 _collab3;
    }

    internal class Collab4
    {
    }

    internal class Collab3
    {
    }

    internal class Collab2
    {
    }

    internal class Collab1
    {
    }
}