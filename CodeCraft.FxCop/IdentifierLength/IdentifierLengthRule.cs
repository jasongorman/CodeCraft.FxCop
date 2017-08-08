using System;
using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop.IdentifierLength
{
    public class IdentifierLengthRule : BaseIntrospectionRule
    {
        private Node _currentNode;

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
            _currentNode = node;
            CheckMember(node);
            CheckVariable(node);
            base.Visit(node);
        }

        public override void VisitParameter(Parameter parameter)
        {
            CheckLongIdentifier(parameter.Name);
            base.VisitParameter(parameter);
        }

        private void CheckVariable(Node node)
        {
            Variable variable = node as Variable;
            if (variable != null && !variable.Name.Name.Contains("<"))
            {
                CheckLongIdentifier(variable.Name);
            }
        }

        private void CheckMember(Node node)
        {
            Member member = node as Member;
            if (member != null && !member.IsSpecialName)
            {
                 CheckLongIdentifier(member.Name);
            }
        }

        private void CheckLongIdentifier(Identifier identifier)
        {
            if (identifier.Name.Length > 20 &! (identifier.Name.Contains("<") || (identifier.Name.Contains("$"))))
            {
                string[] resolutionParams = {identifier.Name};
                Problems.Add(new Problem(new Resolution("Identifier {0} has more than 20 characters", resolutionParams), _currentNode));
            }
        }
    }
}
