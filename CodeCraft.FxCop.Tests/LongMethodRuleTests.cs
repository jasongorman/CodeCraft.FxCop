using System;
using System.Linq;
using CodeCraft.FxCop.LongMethod;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace CodeCraft.FxCop.Tests
{
    [TestFixture]
    public class LongMethodRuleTests
    {
        [Test]
        [TestCase("AbstractMethod", 0)]
        [TestCase("EmptyMethod", 0)]
        [TestCase("MethodWithTenLOC", 0)]
        [TestCase("IgnoresEmptyLines", 0)]
        [TestCase("MethodWithMultiLineStatement", 0)]
        [TestCase("MethodWithCurlyBraces", 0)]
        [TestCase("MethodWithElevenLOC", 1)]
        public void MethodsWithMoreThanTenStatementsBreakRule(string methodName, int expectedProblemCount)
        {
            LongMethodRule rule = new LongMethodRule();
            var memberToCheck = GetMemberToCheck(methodName);
            rule.Check(memberToCheck);
            Assert.AreEqual(expectedProblemCount, rule.Problems.Count);
        }

        private Member GetMemberToCheck(string methodName)
        {
            Type type = typeof (Bar);
            AssemblyNode assembly = AssemblyNode.GetAssembly(type.Module.Assembly.Location, true, true, true);
            TypeNode typeNode = assembly.GetType(Identifier.For(type.Namespace), Identifier.For(type.Name));
            Member methodToCheck = GetMethodByName(typeNode, methodName);
            return methodToCheck;
        }

        private Member GetMethodByName(TypeNode typeNode, string methodName)
        {
            return typeNode.Members.FirstOrDefault(member => member.Name.Name == methodName);
        }
    }

    abstract internal class Bar
    {
        private int z;

        public abstract void AbstractMethod();

        void EmptyMethod()
        {
        }

        void MethodWithTenLOC()
        {
            string x = "";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
        }

        void MethodWithElevenLOC()
        {
            string x = "";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
        }

        void IgnoresEmptyLines()
        {
            string x = "";
            x = x + "X";
            x = x + "X";

            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
        }

        void MethodWithMultiLineStatement()
        {
            string x = "";
            x = x + "X";
            x = 
                x 
                + "X";
            x = 
                x 
                + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
            x = x + "X";
        }

        void MethodWithCurlyBraces()
        {
            {
                {
                    {
                        {
                            {
                                string x = "";
                            }
                        }
                    }
                }
            }
        }
    }

}
