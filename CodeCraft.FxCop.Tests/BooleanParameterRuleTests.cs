using System;
using System.Linq;
using CodeCraft.FxCop.BooleanParameter;
using CodeCraft.FxCop.FeatureEnvy;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace CodeCraft.FxCop.Tests
{
    [TestFixture]
    public class BooleanParameterRuleTests
    {
        [Test]
        [TestCase("MethodWithoutBooleanParam", 0)]
        [TestCase("MethodWithBooleanParam", 1)]
        public void MethodsWithBooleanParametersBreakRule(string methodName, int expectedProblemCount)
        {
            BooleanParameterRule rule = new BooleanParameterRule();
            var memberToCheck = GetMemberToCheck(methodName);
            var param = ((Method) memberToCheck).Parameters[0];
            rule.Check(param);
            Assert.AreEqual(expectedProblemCount, rule.Problems.Count);
        }

        private Member GetMemberToCheck(string methodName)
        {
            Type type = typeof (ClassC);
            AssemblyNode assembly = AssemblyNode.GetAssembly(type.Module.Assembly.Location);
            TypeNode typeNode = assembly.GetType(Identifier.For(type.Namespace), Identifier.For(type.Name));
            Member methodToCheck = GetMethodByName(typeNode, methodName);
            return methodToCheck;
        }

        private Member GetMethodByName(TypeNode typeNode, string methodName)
        {
            return typeNode.Members.FirstOrDefault(member => member.Name.Name == methodName);
        }
    }

    internal interface IClassC
    {
        void MethodWithoutBooleanParam(int a);
        void MethodWithBooleanParam(bool x);
    }

    internal class ClassC : IClassC
    {
        public void MethodWithoutBooleanParam(int a)
        {
            
        }

        public void MethodWithBooleanParam(bool x)
        {
            
        }
    }

}
