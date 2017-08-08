using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop.LongParameterList
{
    public class LongParameterListRule : BaseIntrospectionRule
    {
        public LongParameterListRule()
            : base(
                "LongParameterListRule", "CodeCraft.FxCop.LongParameterList.LongParameterListRuleMetadata",
                typeof (LongParameterListRule).Assembly)
        {
        }

        public override ProblemCollection Check(Member member)
        {
            Method method = member as Method;

            if (method == null)
            {
                return Problems;
            }

            CheckParamsLength(method);
            return this.Problems;
        }

        private void CheckParamsLength(Method method)
        {
            if (method.Parameters.Count > 3)
            {
                string[] resolutionParams = {method.FullName};
                Problems.Add(new Problem(new Resolution("Method {0} has too many parameters", resolutionParams)));
            }
        }
    }
}
