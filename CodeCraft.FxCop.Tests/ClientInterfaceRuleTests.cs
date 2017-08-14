using System;
using System.Linq;
using System.Reflection;
using CodeCraft.FxCop.ClientInterface;
using CodeCraft.FxCop.LargeClass;
using CodeCraft.FxCop.LongMethod;
using Microsoft.FxCop.Sdk;
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
            ClientInterfaceRule rule = new ClientInterfaceRule();
            var typeToCheck = GetTypeToCheck(typeName);
            rule.Check(typeToCheck);
            Assert.AreEqual(expectedProblemCount, rule.Problems.Count);
        }

        [TestCase("Subclass", 0)]
        public void CallsToBaseClassMethodsDontCount(string typeName, int expectedProblemCount)
        {
            ClientInterfaceRule rule = new ClientInterfaceRule();
            var typeToCheck = GetTypeToCheck(typeName);
            rule.Check(typeToCheck);
            Assert.AreEqual(expectedProblemCount, rule.Problems.Count);
        }

        private TypeNode GetTypeToCheck(string typeName)
        {
            AssemblyNode assembly = AssemblyNode.GetAssembly(this.GetType().Module.Assembly.Location, true, true, true);
            TypeNode typeNode = GetTypeByName(assembly, typeName);
            return typeNode;
        }

        private TypeNode GetTypeByName(AssemblyNode assembly, string typeName)
        {
            return assembly.Types.FirstOrDefault(t => t.Name.Name == typeName);
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
        ClientClass2(SupplierClass1 supplier)
        {
            int x = supplier.Property1;
        }

        void DoFoo(SupplierClass1 supplier)
        {
            FooMore(supplier);
        }

        void FooMore(SupplierClass1 supplier)
        {
            supplier.Method2();
        }

        void MoreFoo(SupplierClass2 supplier)
        {
            supplier.Method1();
            supplier.Method2();
        }
    }

    internal class SupplierClass1
    {

        public int Property1
        {
            get { return 0;  }
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
