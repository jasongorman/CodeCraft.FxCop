using System.IO;
using System.Text;
using CodeCraft.FxCop.ObjectCreation;
using NUnit.Framework;

namespace CodeCraft.FxCop.Tests
{
    [TestFixture]
    public class ObjectCreationRuleTests
    {
        [TestCase("MethodThatCreatesObject", 1)]
        [TestCase("MethodWithNoCreation", 0)]
        [TestCase("CreatesSystemType", 0)]
        public void MethodsCantUseNewOnProjectClasses(string methodName, int expectedProblemCount)
        {
            var rule = new ObjectCreationRule();
            rule.Check(AssemblyReader.GetMethodByName(typeof (ClassE), methodName));
            Assert.That(rule.Problems.Count, Is.EqualTo(expectedProblemCount));
        }

        [TestCase("CreateMethod", 0)]
        [TestCase("BuildMethod", 0)]
        public void FactoryOrBuilderMethodsDontCount(string methodName, int expectedProblemCount)
        {
            var rule = new ObjectCreationRule();
            rule.Check(AssemblyReader.GetMethodByName(typeof (ClassE), methodName));
            Assert.That(rule.Problems.Count, Is.EqualTo(expectedProblemCount));
        }
    }

    internal class ClassE
    {
        private void MethodThatCreatesObject()
        {
            var foo = new ClassC();
        }

        private void MethodWithNoCreation()
        {
        }

        private void CreatesSystemType()
        {
            var foo = new BinaryWriter(new MemoryStream(), Encoding.ASCII, false);
        }

        private IClassC CreateMethod()
        {
            return new ClassC();
        }

        private IClassC BuildMethod()
        {
            return new ClassC();
        }
    }
}