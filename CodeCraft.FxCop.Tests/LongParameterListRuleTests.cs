using System;
using System.Linq;
using CodeCraft.FxCop.FeatureEnvy;
using CodeCraft.FxCop.LongParameterList;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace CodeCraft.FxCop.Tests
{
    [TestFixture]
    public class LongParameterListRuleTests
    {
        [Test]
        [TestCase("MethodWithThreeParams", 0)]
        [TestCase("MethodWithFourParams", 1)]
        public void MethodsWithMoreThanThreeParamsBreakRule(string methodName, int expectedProblemCount)
        {
            LongParameterListRule rule = new LongParameterListRule();
            var memberToCheck = GetMemberToCheck(methodName);
            rule.Check(memberToCheck);
            Assert.AreEqual(expectedProblemCount, rule.Problems.Count);
        }

        private Member GetMemberToCheck(string methodName)
        {
            Type type = typeof (Foo);
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

    internal class Foo
    {
        void MethodWithThreeParams(int a, int b, int c)
        {
            
        }

        void MethodWithFourParams(int a, int b, int c, int d)
        {
            
        }
    }

}
