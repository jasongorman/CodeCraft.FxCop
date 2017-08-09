using System.Linq;
using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop.BooleanParameter
{
    public class BooleanParameterRule : BaseIntrospectionRule
    {
        public BooleanParameterRule()
            : base(
                "BooleanParameterRule", "CodeCraft.FxCop.BooleanParameter.BooleanParameterRuleMetadata",
                typeof (BooleanParameterRule).Assembly)
        {
        }

        public override ProblemCollection Check(Parameter parameter)
        {
            if (parameter.Type.Name.Name == "Boolean")
            {
                string[] resolutionParams = {parameter.DeclaringMethod.Name.Name};
                Problems.Add(new Problem(new Resolution("Method {0} has Boolean parameters", resolutionParams)));
            }
            return this.Problems;
        }
    }
}
