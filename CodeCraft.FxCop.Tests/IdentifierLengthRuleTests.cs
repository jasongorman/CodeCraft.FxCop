using System;
using System.Linq;
using CodeCraft.FxCop.IdentifierLength;
using CodeCraft.FxCop.LongMethod;
using Microsoft.FxCop.Sdk;
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
            IdentifierLengthRule rule = new IdentifierLengthRule();
            var module = GetAssembly();
            rule.Check(module);
            Assert.That(rule.Problems.Count(p => p.Resolution.ToString().Contains(identifier)), Is.EqualTo(expectedProblemCount));
        }

        private AssemblyNode GetAssembly()
        {
            return AssemblyNode.GetAssembly(this.GetType().Module.Assembly.Location, true, true, true);
        }
    }

    class ClassNameThatsTooLong
    {
        private int _fieldNameThatsTooLong;

        private const int constantNameThatIsTooLong = 0;

        int PropertyNameTooLongXX { get { return 0; } }

        void MethodNameThatsTooLong(int parameterNameThatsTooLong)
        {
            int localNameThatIsTooLong = 0;
        }

        private void ShortMethodNameXXXXX()
        {
        }

        static void StaticMethodThatHasNameLongerThanTwentyChars()
        {
            
        }
    }

    interface InterfaceNameThatsTooLong
    {
        
    }

    enum EnumNameThatIsTooLong
    {
        EnumValueThatIsTooLong
    }

}
