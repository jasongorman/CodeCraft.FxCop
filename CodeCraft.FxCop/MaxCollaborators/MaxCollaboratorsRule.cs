using System.Linq;
using CodeCraft.FxCop.LargeClass;
using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop.MaxCollaborators
{
    public class MaxCollaboratorsRule : BaseIntrospectionRule
    {
        public MaxCollaboratorsRule()
            : base(
                "MaxCollaboratorsRule", "CodeCraft.FxCop.MaxCollaborators.MaxCollaboratorsRuleMetadata.xml",
                typeof (MaxCollaboratorsRule).Assembly)
        {
        }

        public override ProblemCollection Check(TypeNode type)
        {
           CheckCollaborators(type);
           return this.Problems;
        }

        private void CheckCollaborators(TypeNode type)
        {
            if (CreateCollabCount().Calculate(type) > 3)
            {
                string[] resolutionParams = {type.FullName};
                Problems.Add(new Problem(new Resolution("Type {0} has more than 3 collaborators", resolutionParams),type));
            }
        }

        internal virtual IMetric CreateCollabCount()
        {
            return new CollaboratorCount();
        }
    }
}
