using CodeCraft.FxCop.FeatureEnvy;
using NUnit.Framework;

namespace CodeCraft.FxCop.Tests
{
    [TestFixture]
    public class FeatureEnvyRuleTests
    {
        [TestCase("MethodThatMakesOneCallOnOneSupplier", 0)]
        [TestCase("MethodThatMakesTwoCallsOnOneSupplier", 1)]
        [TestCase("MethodThatMakesTwoCallsOnDifferentSuppliers", 0)]
        [TestCase("MethodWithFeatureEnvyForThirdPartyType", 0)]
        public void MethodsThatMakeMultipleCallsToCollaboratorsHaveFeatureEnvy(string methodName,
            int expectedProblemCount)
        {
            var rule = new FeatureEnvyRule();
            rule.Check(AssemblyReader.GetMethodByName(typeof (Client), methodName));
            Assert.AreEqual(expectedProblemCount, rule.Problems.Count);
        }
    }

    internal class Client
    {
        private readonly SupplierA supplierA = new SupplierA();
        private readonly SupplierB supplierB = new SupplierB();

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

            foreach (var s in blah)
            {
                var x = s.ToUpper();
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