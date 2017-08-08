using System;
using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop.IdentifierLength
{
    public class IdentifierLengthRule : BaseIntrospectionRule
    {
        public IdentifierLengthRule()
            : base(
                "IdentifierLengthRule", "CodeCraft.FxCop.IdentifierLength.IdentifierLengthRuleMetadata",
                typeof (IdentifierLengthRule).Assembly)
        {
        }

        public override ProblemCollection Check(ModuleNode module)
        {
           Visit(module);
           return this.Problems;
        }

        public override void Visit(Node node)
        {
            CheckMember(node);
            CheckVariable(node);
            base.Visit(node);
        }

        public override void VisitParameter(Parameter parameter)
        {
            CheckIfLongIdentifier(parameter.Name);
            base.VisitParameter(parameter);
        }

        private void CheckVariable(Node node)
        {
            Variable variable = node as Variable;
            if (variable != null)
            {
                CheckIfLongIdentifier(variable.Name);
            }
        }

        private void CheckMember(Node node)
        {
            Member member = node as Member;
            if (member != null && !member.IsSpecialName)
            {
                 CheckIfLongIdentifier(member.Name);
            }
        }

        private void CheckIfLongIdentifier(Identifier identifier)
        {
            if (identifier.Name.Length > 20)
            {
                string[] resolutionParams = {identifier.Name};
                Problems.Add(new Problem(new Resolution("Identifier {0} has more than 20 characters", resolutionParams)));
            }
        }
    }
}
