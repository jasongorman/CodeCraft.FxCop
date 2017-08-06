using System.Linq;
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

            StatementVisitor visitor = new StatementVisitor();
            visitor.VisitStatements(method.Body.Statements);
            CheckForLongMethod(visitor, method);
            return this.Problems;
        }

        private void CheckForLongMethod(StatementVisitor visitor, Method method)
        {
            if (visitor.LinesOfCode > 10)
            {
                string[] resolutionParams = {method.FullName};
                Problems.Add(new Problem(new Resolution("Method {0} is too long", resolutionParams)));
            }
        }
    }
}
