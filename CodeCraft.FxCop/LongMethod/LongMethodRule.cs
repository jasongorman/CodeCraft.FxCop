using System.Collections.Generic;
using System.Linq;
using CodeCraft.FxCop.MethodComplexity;
using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop.LongMethod
{
    public class LongMethodRule : BaseIntrospectionRule
    {

        public LongMethodRule()
            : base(
                "LongMethodRule", "CodeCraft.FxCop.LongMethod.LongMethodRuleMetadata",
                typeof (LongMethodRule).Assembly)
        {
        }

        public override ProblemCollection Check(Member member)
        {
            Method method = member as Method;

            if (method == null)
            {
                return Problems;
            }

            CheckForLongMethod(method);
            return this.Problems;
        }

        private void CheckForLongMethod(Method method)
        {
            if (CreateMetrics().Calculate(method) > 10)
            {
                string[] resolutionParams = {method.FullName};
                Problems.Add(new Problem(new Resolution("Method {0} is too long", resolutionParams)));
            }
        }

        private IMetric CreateMetrics()
        {
            return new LinesOfCodeMetric();
        }
    }
}
