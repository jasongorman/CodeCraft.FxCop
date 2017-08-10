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
        [TestCase("MethodWithTwoWhileLoops", 0)]
        [TestCase("SwitchStatementWithTwoCases", 0)]
        [TestCase("MethodWithCatchBlock", 0)]
        [TestCase("SwitchStatementWithThreeCases", 1)]
        [TestCase("MethodWithThreeWhileLoops", 1)]
        [TestCase("MethodWithThreeBranches", 1)]
        [TestCase("MethodWithThreeLoops", 1)]
        [TestCase("MethodWithThreeForeachLoops", 1)]
        [TestCase("MethodWithComplexConditional", 1)]
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
        private void MethodWithComplexConditional(int x)
        {
            if (x < 0 || x >= 100 || x == -10)
                x++;
        }

        private void MethodWithCatchBlock()
        {
            int i = 0;

            while (i < 10)
            {
                    try
                    {
                        int x = 0;
                    }
                    catch (InvalidMetadataException e)
                    {

                    }
            }
        }

        private void MethodWithTwoBranches()
        {
            var x = 0;

            if (x > -1)
            {
                if (x < 10)
                {
                    
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

        private void MethodWithThreeLoops()
        {
            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    for (var k = 0; k < 2; k++)
                    {
                        
                    }
                }
            }
        }

        private void MethodWithThreeForeachLoops()
        {
            int[] stuff = {1, 2, 3};

            foreach (var i in stuff)
            {
                
            }

            foreach (var i in stuff)
            {

            }

            foreach (var i in stuff)
            {

            }
        }

        private void MethodWithTwoWhileLoops()
        {
            int x = 0;

            while (x < 10)
            {
                x++;
                int y = 0;
                while (y < 10)
                {
                    y++;
                }

            }
        }

        private void MethodWithThreeWhileLoops()
        {
            int x = 0;

            while (x < 10)
            {
                x++;
                int y = 0;

                while (y < 10)
                {
                    y++;
                    int z = 0;

                    while (z < 10)
                    {
                        z++;
                    }
                }
            }
        }

        private void SwitchStatementWithTwoCases()
        {
            var x = 0;
            switch (x)
            {
                case 0:
                    break;
                case 1:
                    break;
                default:
                    break;
            }
        }

        private void SwitchStatementWithThreeCases()
        {
            var x = 0;
            switch (x)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                default:
                    break;
            }
        }
    }
}
