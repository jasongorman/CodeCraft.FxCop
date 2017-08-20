using System.Linq;
using CodeCraft.FxCop.IdentifierLength;
using NUnit.Framework;

namespace CodeCraft.FxCop.Tests
{
    [TestFixture]
    public class IdentifierLengthRuleTests
    {
        [TestCase("ClassNameThatsTooLong", 1)]
        [TestCase("_fieldNameThatsTooLong", 1)]
        [TestCase("constantNameThatIsTooLong", 1)]
        [TestCase("PropertyNameTooLongXX", 1)]
        [TestCase("MethodNameThatsTooLong", 1)]
        [TestCase("parameterNameThatsTooLong", 1)]
        [TestCase("localNameThatIsTooLong", 1)]
        [TestCase("InterfaceNameThatsTooLong", 1)]
        [TestCase("EnumNameThatIsTooLong", 1)]
        [TestCase("EnumValueThatIsTooLong", 1)]
        [TestCase("StaticMethodThatHasNameLongerThanTwentyChars", 1)]
        [TestCase("ShortMethodNameXXXXX", 0)]
        public void IdentifiersLongerThanTwentyCharsBreakRule(string identifier, int expectedProblemCount)
        {
            var rule = new IdentifierLengthRule();
            var module = AssemblyReader.GetAssembly();
            rule.Check(module);
            Assert.That(rule.Problems.Count(p => p.Resolution.ToString().Contains(identifier)),
                Is.EqualTo(expectedProblemCount));
        }
    }

    internal class ClassNameThatsTooLong
    {
        private const int constantNameThatIsTooLong = 0;
        private int _fieldNameThatsTooLong;

        private int PropertyNameTooLongXX
        {
            get { return 0; }
        }

        private void MethodNameThatsTooLong(int parameterNameThatsTooLong)
        {
            var localNameThatIsTooLong = 0;
        }

        private void ShortMethodNameXXXXX()
        {
        }

        private static void StaticMethodThatHasNameLongerThanTwentyChars()
        {
        }
    }

    internal interface InterfaceNameThatsTooLong
    {
    }

    internal enum EnumNameThatIsTooLong
    {
        EnumValueThatIsTooLong
    }
}