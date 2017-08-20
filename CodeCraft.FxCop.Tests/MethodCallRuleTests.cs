using CodeCraft.FxCop.MethodCall;
using NUnit.Framework;

namespace CodeCraft.FxCop.Tests
{
    [TestFixture]
    public class MethodCallRuleTests
    {
        [TestCase("CallsConcreteMethod", 1)]
        [TestCase("CallsAbstractMethod", 0)]
        [TestCase("CallsVirtualMethod", 0)]
        public void CannotInvokeConcreteMethods(string methodName, int expectedProblemCount)
        {
            var rule = new MethodCallRule();
            rule.Check(AssemblyReader.GetMethodByName(typeof (ClassF), methodName));
            Assert.That(rule.Problems.Count, Is.EqualTo(expectedProblemCount));
        }

        [TestCase("InterfaceMethod", 0)]
        [TestCase("Build", 0)]
        public void FactoryOrBuilderMethodsDontCount(string methodName, int expectedProblemCount)
        {
            var rule = new MethodCallRule();
            rule.Check(AssemblyReader.GetMethodByName(typeof (ClassF), methodName));
            Assert.That(rule.Problems.Count, Is.EqualTo(expectedProblemCount));
        }
    }

    internal class ClassF
    {
        private readonly ClassG _g;

        private ClassF(ClassG g)
        {
            _g = g;
        }

        private void CallsConcreteMethod()
        {
            _g.ConcreteMethod();
        }

        private void CallsAbstractMethod()
        {
            _g.AbstractMethod();
        }

        private void CallsVirtualMethod()
        {
            _g.VirtualMethod();
        }

        private ClassG Create()
        {
            _g.ConcreteMethod();
            return _g;
        }

        private ClassG Build()
        {
            _g.ConcreteMethod();
            return _g;
        }
    }


    internal abstract class ClassG
    {
        internal virtual void VirtualMethod()
        {
        }

        internal abstract void AbstractMethod();

        internal void ConcreteMethod()
        {
        }
    }
}