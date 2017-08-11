using System;
using System.Collections.Generic;
using CodeCraft.FxCop.LongMethod;
using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop.MethodComplexity
{
    public class MethodComplexityRule : BaseIntrospectionRule
    {
        private int _branches;

        public MethodComplexityRule()
            : base(
                "MethodComplexityRule", "CodeCraft.FxCop.MethodComplexity.MethodComplexityRuleMetadata",
                typeof (MethodComplexityRule).Assembly)
        {
        }

        public override ProblemCollection Check(Member member)
        {
            var method = member as Method;

            if (method == null)
            {
                return Problems;
            }

            CheckIfComplex(method);
            return Problems;
        }

        private void CheckIfComplex(Method method)
        {
            int complexity = CreateMetrics().Calculate(method);

            if (complexity > 3)
            {
                string[] resolutionParams = { method.FullName, complexity.ToString() };
                Problems.Add(new Problem(new Resolution("Method {0} has {1} cyclomatic complexity. Max allowed is 3.", resolutionParams)));
            }
        }

        private IMetric CreateMetrics()
        {
            return new ComplexityMetric();
        }
    }
}
