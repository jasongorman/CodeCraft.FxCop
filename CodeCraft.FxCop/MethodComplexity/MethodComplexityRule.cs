using System;
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

            CheckIfComplex(method);
            return this.Problems;
        }

        private void CheckIfComplex(Method method)
        {
            string[] resolutionParams = {method.FullName};
            Console.WriteLine("Branches in {0} = " + _branches, resolutionParams.ToString());
            if (_branches > 2)
            {
                Problems.Add(new Problem(new Resolution("Method {0} has too many branches", resolutionParams)));
            }
        }

        public override void VisitBranch(Branch branch)
        {
            if(branch.Condition != null && branch.SourceContext.StartLine > 0)
                _branches++;
            base.VisitBranch(branch);
        }

        public override void VisitSwitchInstruction(SwitchInstruction switchInstruction)
        {
            _branches+=switchInstruction.Targets.Count;
            base.VisitSwitchInstruction(switchInstruction);
        }
    }
}
