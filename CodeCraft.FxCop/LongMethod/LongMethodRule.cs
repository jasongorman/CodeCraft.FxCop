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
            var method = member as Method;

            if (method == null)
            {
                return Problems;
            }

            CheckForLongMethod(method);
            return Problems;
        }

        private void CheckForLongMethod(Method method)
        {
            if (CreateMetrics().Calculate(method) > 10)
            {
                string[] resolutionParams = {method.FullName};
                Problems.Add(new Problem(new Resolution("Method {0} is too long", resolutionParams)));
            }
        }

        protected IMetric CreateMetrics()
        {
            return new LinesOfCodeMetric();
        }
    }
}
