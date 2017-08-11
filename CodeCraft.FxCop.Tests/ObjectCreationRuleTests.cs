using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodeCraft.FxCop.FeatureEnvy;
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
            Method method = GetMethodToCheck(methodName, typeof (ClassE), this.GetType().Assembly.Location);
            rule.Check(method);
            Assert.That(rule.Problems.Count, Is.EqualTo(expectedProblemCount));
        }

        [TestCase("CreateMethod", 0)]
        [TestCase("BuildMethod", 0)]
        public void FactoryOrBuilderMethodsDontCount(string methodName, int expectedProblemCount)
        {
            ObjectCreationRule rule = new ObjectCreationRule();
            Method method = GetMethodToCheck(methodName, typeof (ClassE), this.GetType().Assembly.Location);
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


