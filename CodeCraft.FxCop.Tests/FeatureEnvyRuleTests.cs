﻿using System;
using CodeCraft.FxCop.FeatureEnvy;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace CodeCraft.FxCop.Tests
{
    [TestFixture]
    public class FeatureEnvyRuleTests
    {
        [Test]
        [TestCase("MethodThatMakesOneCallOnOneSupplier", 0)]
        [TestCase("MethodThatMakesTwoCallsOnOneSupplier", 1)]
        [TestCase("MethodThatMakesTwoCallsOnDifferentSuppliers", 0)]
        [TestCase("MethodWithFeatureEnvyForThirdPartyType", 0)]
        public void MethodsThatMakeMultipleCallsToCollaboratorsHaveFeatureEnvy(string methodName, int expectedProblemCount)
        {
            FeatureEnvyRule rule = new FeatureEnvyRule();
            rule.Check(GetMemberToCheck(methodName));
            Assert.AreEqual(expectedProblemCount, rule.Problems.Count);
        }

        private Member GetMemberToCheck(string methodName)
        {
            Type type = typeof(Client);
            AssemblyNode assembly = AssemblyNode.GetAssembly(type.Module.Assembly.Location);
            TypeNode typeNode = assembly.GetType(Identifier.For(type.Namespace), Identifier.For(type.Name));
            Member methodToCheck = GetMethodByName(typeNode, methodName);
            return methodToCheck;
        }

        private Member GetMethodByName(TypeNode typeNode, string methodName)
        {
            foreach (Member member in typeNode.Members)
            {
                if (member.Name.Name == methodName)
                {
                    return member;
                }
            }
            return null;
        }
    }

    internal class Client
    {
        private SupplierA supplierA = new SupplierA();
        private SupplierB supplierB = new SupplierB();

        public void MethodThatMakesOneCallOnOneSupplier()
        {
            supplierA.FeatureA();
        }

        public void MethodThatMakesTwoCallsOnOneSupplier()
        {
            supplierA.FeatureA();
            supplierA.FeatureB();
        }

        public void MethodThatMakesTwoCallsOnDifferentSuppliers()
        {
            supplierA.FeatureA();
            supplierB.FeatureA();
        }

        public void MethodWithFeatureEnvyForThirdPartyType()
        {
            string[] blah = {"a", "b", "c"};

            foreach (string s in blah)
            {
                string x = s.ToUpper();
            }
        }
    }

    internal class SupplierA
    {
        internal void FeatureA()
        {
        }

        internal void FeatureB()
        {
        }
    }

    internal class SupplierB
    {
        internal void FeatureA()
        {
        }
    }
}