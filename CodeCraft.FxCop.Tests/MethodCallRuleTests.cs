using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeCraft.FxCop.FeatureEnvy;
using CodeCraft.FxCop.MethodCall;
using CodeCraft.FxCop.ObjectCreation;
using Microsoft.FxCop.Sdk;
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
            MethodCallRule rule = new MethodCallRule();
            Method method = GetMethodToCheck(methodName, typeof (ClassF), this.GetType().Assembly.Location);
            rule.Check(method);
            Assert.That(rule.Problems.Count, Is.EqualTo(expectedProblemCount));
        }

        [TestCase("InterfaceMethod", 0)]
        [TestCase("Build", 0)]
        public void FactoryOrBuilderMethodsDontCount(string methodName, int expectedProblemCount)
        {
            MethodCallRule rule = new MethodCallRule();
            Method method = GetMethodToCheck(methodName, typeof (ClassF), this.GetType().Assembly.Location);
            rule.Check(method);
            Assert.That(rule.Problems.Count, Is.EqualTo(expectedProblemCount));
        }

        private Method GetMethodToCheck(string methodName, Type typeToTest, string assemblyPath)
        {
            Type type = typeToTest;
            AssemblyNode assemblyNode = AssemblyNode.GetAssembly(assemblyPath);
            TypeNode typeNode = assemblyNode.GetType(Identifier.For(type.Namespace), Identifier.For(type.Name));
            return (Method)typeNode.Members.FirstOrDefault(m => m.Name.Name == methodName);
        }
    }

    internal class ClassF
    {
        private ClassG _g;

        ClassF(ClassG g)
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
        internal virtual void VirtualMethod(){}

        internal abstract void AbstractMethod();

        internal void ConcreteMethod(){}
    }
}


