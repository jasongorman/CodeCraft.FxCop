using CodeCraft.FxCop.BooleanParameter;
using NUnit.Framework;

namespace CodeCraft.FxCop.Tests
{
    [TestFixture]
    public class BooleanParameterRuleTests
    {
        [TestCase("MethodWithoutBooleanParam", 0)]
        [TestCase("MethodWithBooleanParam", 1)]
        public void MethodsWithBooleanParametersBreakRule(string methodName, int expectedProblemCount)
        {
            var rule = new BooleanParameterRule();
            var method = AssemblyReader.GetMethodByName(typeof (ClassC), methodName);
            var param = method.Parameters[0];
            rule.Check(param);
            Assert.AreEqual(expectedProblemCount, rule.Problems.Count);
        }
    }

    internal interface IClassC
    {
        void MethodWithoutBooleanParam(int a);
        void MethodWithBooleanParam(bool x);
    }

    internal class ClassC : IClassC
    {
        public void MethodWithoutBooleanParam(int a)
        {
        }

        public void MethodWithBooleanParam(bool x)
        {
        }
    }
}