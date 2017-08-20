using CodeCraft.FxCop.LongMethod;
using NUnit.Framework;

namespace CodeCraft.FxCop.Tests
{
    [TestFixture]
    public class LongMethodRuleTests
    {
        [Test]
        [TestCase("AbstractMethod", 0)]
        [TestCase("EmptyMethod", 0)]
        [TestCase("MethodWithTenLOC", 0)]
        [TestCase("IgnoresEmptyLines", 0)]
        [TestCase("MethodWithMultiLineStatement", 0)]
        [TestCase("MethodWithCurlyBraces", 0)]
        [TestCase("MethodWithElevenLOC", 1)]
        public void MethodsWithMoreThanTenStatementsBreakRule(string methodName, int expectedProblemCount)
        {
            var rule = new LongMethodRule();
            rule.Check(AssemblyReader.GetMethodByName(typeof (Bar), methodName));
            Assert.AreEqual(expectedProblemCount, rule.Problems.Count);
        }
    }

    internal abstract class Bar
    {
        private int z;
        public abstract void AbstractMethod();

        private void EmptyMethod()
        {
        }

        private void MethodWithTenLOC()
        {
            var x = "";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
        }

        private void MethodWithElevenLOC()
        {
            var x = "";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
        }

        private void IgnoresEmptyLines()
        {
            var x = "";
            x = x + "X";
            x = x + "X";

            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
        }

        private void MethodWithMultiLineStatement()
        {
            var x = "";
            x = x + "X";
            x =
                x
                + "X";
            x =
                x
                + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
        }

        private void MethodWithCurlyBraces()
        {
            {
                {
                    {
                        {
                            {
                                var x = "";
                            }
                        }
                    }
                }
            }
        }
    }
}