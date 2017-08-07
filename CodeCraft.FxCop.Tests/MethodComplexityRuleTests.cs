using System;
using System.Collections.Generic;
using System.Linq;
using CodeCraft.FxCop.FeatureEnvy;
using CodeCraft.FxCop.LongParameterList;
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
        [TestCase("OnlySwitchAllowed", 0)]
        [TestCase("MethodWithThreeBranches", 1)]
        [TestCase("MethodWithTwoLoops", 1)]
        public void MethodsWithMoreThanThreeBranchesBreakRule(string methodName, int expectedProblemCount)
        {
            MethodComplexityRule rule = new MethodComplexityRule();
            var memberToCheck = GetMemberToCheck(methodName);
            rule.Check(memberToCheck);
            Assert.AreEqual(expectedProblemCount, rule.Problems.Count);
        }

        private Member GetMemberToCheck(string methodName)
        {
            Type type = typeof (ClassD);
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

    internal class ClassD
    {
        void MethodWithTwoBranches()
        {
            int x = 0;

            if (x > -1)
            {
                if (x < 1)
                {
                    x++;
                }
            }
        }

        void MethodWithThreeBranches()
        {
            int x = 0;

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

        void MethodWithOneLoop()
        {
            for (int i = 0; i < 5; i++)
            {
       
            }
        }

        void MethodWithTwoLoops()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    
                }
            }
        }

        void OnlySwitchAllowed()
        {
            int x = 0;
            switch (x)
            {
                default:
                    break;
            }
        }
    }

}
