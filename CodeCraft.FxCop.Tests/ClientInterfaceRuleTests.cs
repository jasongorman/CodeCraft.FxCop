using CodeCraft.FxCop.ClientInterface;
using NUnit.Framework;

namespace CodeCraft.FxCop.Tests
{
    [TestFixture]
    public class ClientInterfaceRuleTests
    {
        [TestCase("ClientClass1", 1)]
        [TestCase("ClientClass2", 0)]
        public void InterfacesWithMoreMethodsThatClientUsesBreakRule(string typeName, int expectedProblemCount)
        {
            var rule = new ClientInterfaceRule();
            rule.Check(AssemblyReader.GetType(typeName));
            Assert.AreEqual(expectedProblemCount, rule.Problems.Count);
        }

        [TestCase("Subclass", 0)]
        public void CallsToBaseClassMethodsDontCount(string typeName, int expectedProblemCount)
        {
            var rule = new ClientInterfaceRule();
            rule.Check(AssemblyReader.GetType(typeName));
            Assert.AreEqual(expectedProblemCount, rule.Problems.Count);
        }
    }

    internal class Subclass : ClientClass1
    {
        public void Method1()
        {
            DoFoo(null);
        }
    }

    internal class ClientClass1
    {
        protected void DoFoo(SupplierClass1 supplier)
        {
            supplier.Method2();
        }
    }

    internal class ClientClass2
    {
        private ClientClass2(SupplierClass1 supplier)
        {
            var x = supplier.Property1;
        }

        private void DoFoo(SupplierClass1 supplier)
        {
            FooMore(supplier);
        }

        private void FooMore(SupplierClass1 supplier)
        {
            supplier.Method2();
        }

        private void MoreFoo(SupplierClass2 supplier)
        {
            supplier.Method1();
            supplier.Method2();
        }
    }

    internal class SupplierClass1
    {
        public int Property1
        {
            get { return 0; }
        }

        internal void Method2()
        {
        }
    }

    internal interface SupplierClass2
    {
        void Method1();
        void Method2();
    }
}