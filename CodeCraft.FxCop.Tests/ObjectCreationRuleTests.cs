using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeCraft.FxCop.ObjectCreation;
using Microsoft.FxCop.Sdk;
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
            ObjectCreationRule rule = new ObjectCreationRule();
            Method method = GetMethodToCheck(methodName);
            rule.Check(method);
            Assert.That(rule.Problems.Count, Is.EqualTo(expectedProblemCount));
        }

        [TestCase("CreateMethod", 0)]
        [TestCase("BuildMethod", 0)]
        public void FactoryOrBuilderMethodsDontCount(string methodName, int expectedProblemCount)
        {
            ObjectCreationRule rule = new ObjectCreationRule();
            Method method = GetMethodToCheck(methodName);
            rule.Check(method);
            Assert.That(rule.Problems.Count, Is.EqualTo(expectedProblemCount));
        }

        private Method GetMethodToCheck(string methodName)
        {
            Type type = typeof (ClassE);
            AssemblyNode assemblyNode = AssemblyNode.GetAssembly(this.GetType().Assembly.Location);
            TypeNode typeNode = assemblyNode.GetType(Identifier.For(type.Namespace), Identifier.For(type.Name));
            return typeNode.GetMethod(Identifier.For(methodName));
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
            var foo = new System.IO.BinaryWriter(new MemoryStream(),Encoding.ASCII,false);
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


