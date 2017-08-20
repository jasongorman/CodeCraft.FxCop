using CodeCraft.FxCop.LongParamList;
using NUnit.Framework;

namespace CodeCraft.FxCop.Tests
{
    [TestFixture]
    public class LongParameterListRuleTests
    {
        [TestCase("MethodWithThreeParams", 0)]
        [TestCase("MethodWithFourParams", 1)]
        public void MethodsWithMoreThanThreeParamsBreakRule(string methodName, int expectedProblemCount)
        {
            var rule = new LongParamListRule();
            rule.Check(AssemblyReader.GetMethodByName(typeof (Foo), methodName));
            Assert.AreEqual(expectedProblemCount, rule.Problems.Count);
        }
    }

    internal class Foo
    {
        private int memberNotMethod = 0;

        private void MethodWithThreeParams(int a, int b, int c)
        {
        }

        private void MethodWithFourParams(int a, int b, int c, int d)
        {
        }
    }
}