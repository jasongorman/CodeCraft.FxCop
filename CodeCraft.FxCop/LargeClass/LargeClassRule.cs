using System.Linq;
using System.Threading;
using CodeCraft.FxCop.LongMethod;
using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop.LargeClass
{
    public class LargeClassRule : BaseIntrospectionRule
    {
        public LargeClassRule()
            : base(
                "LargeClassRule", "CodeCraft.FxCop.LargeClass.LargeClassRuleMetadata",
                typeof (LargeClassRule).Assembly)
        {
        }

        public override ProblemCollection Check(TypeNode type)
        {
           CheckForLargeClass(type);
           return this.Problems;
        }

        private void CheckForLargeClass(TypeNode type)
        {
            if (MethodCount(type) > 8)
            {
                string[] resolutionParams = {type.FullName};
                Problems.Add(new Problem(new Resolution("Type {0} has too many methods/properties", resolutionParams)));
            }
        }

        private int MethodCount(TypeNode type)
        {
            return type.Members.Count(m => m.NodeType == NodeType.Method);
        }
    }
}
