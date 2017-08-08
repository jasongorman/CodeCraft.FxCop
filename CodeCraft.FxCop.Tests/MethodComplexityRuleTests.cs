using System.Linq;
using CodeCraft.FxCop.MethodComplexity;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace CodeCraft.FxCop.Tests
{
    [TestFixture]
    public class MethodComplexityRuleTests
    {
        [TestCase("MethodWithTwoBranches", 0)]
        [TestCase("MethodWithOneLoop", 0)]
        [TestCase("OnlyDefaultSwitchCaseAllowed", 0)]
        [TestCase("MethodWithThreeBranches", 1)]
        [TestCase("MethodWithTwoLoops", 1)]
        public void MethodsWithMoreThanThreeBranchesBreakRule(string methodName, int expectedProblemCount)
        {
            var rule = new MethodComplexityRule();
            var memberToCheck = GetMemberToCheck(methodName);
            rule.Check(memberToCheck);
            Assert.AreEqual(expectedProblemCount, rule.Problems.Count);
        }

        private Member GetMemberToCheck(string methodName)
        {
            var type = typeof (ClassD);
            var assembly = AssemblyNode.GetAssembly(type.Module.Assembly.Location);
            var typeNode = assembly.GetType(Identifier.For(type.Namespace), Identifier.For(type.Name));
            var methodToCheck = GetMethodByName(typeNode, methodName);
            return methodToCheck;
        }

        private Member GetMethodByName(TypeNode typeNode, string methodName)
        {
            return typeNode.Members.FirstOrDefault(member => member.Name.Name == methodName);
        }
    }

    internal class ClassD
    {
        private void MethodWithTwoBranches()
        {
            var x = 0;

            if (x > -1)
            {
                if (x < 1)
                {
                    x++;
                }
            }
        }

        private void MethodWithThreeBranches()
        {
            var x = 0;

            if (x > -1)
            {
                if (x < 1)
                {
                    if (x == 0)
                    {
                        x++;
                    }
                }
            }
        }

        private void MethodWithOneLoop()
        {
            for (var i = 0; i < 5; i++)
            {
            }
        }

        private void MethodWithTwoLoops()
        {
            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                }
            }
        }

        private void OnlyDefaultSwitchCaseAllowed()
        {
            var x = 0;
            switch (x)
            {
                default:
                    break;
            }
        }
    }
}
