using CodeCraft.FxCop.LongParameterList;
using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop.MethodComplexity
{
    public class MethodComplexityRule : BaseIntrospectionRule
    {
        private int _branches = 0;

        public MethodComplexityRule()
            : base(
                "MethodComplexityRule", "CodeCraft.FxCop.MethodComplexity.MethodComplexityRuleMetadata",
                typeof (MethodComplexityRule).Assembly)
        {
        }

        public override ProblemCollection Check(Member member)
        {
            Method method = member as Method;

            if (method == null)
            {
                return Problems;
            }

            VisitStatements(method.Body.Statements);

            if (_branches > 2)
            {
                string[] resolutionParams = {method.FullName};
                Problems.Add(new Problem(new Resolution("Method {0} has too many branches", resolutionParams)));
            }
            return this.Problems;
        }

        public override void VisitBranch(Branch branch)
        {
            _branches++;
            base.VisitBranch(branch);
        }

        public override void VisitSwitchInstruction(SwitchInstruction switchInstruction)
        {
            _branches++;
            base.VisitSwitchInstruction(switchInstruction);
        }
    }
}
