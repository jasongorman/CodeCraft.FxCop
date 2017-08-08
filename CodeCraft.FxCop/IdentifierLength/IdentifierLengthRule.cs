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

        public override void VisitTypeNode(TypeNode type)
        {
            CheckIfLongIdentifier(type.Name);
            base.VisitTypeNode(type);
        }

        public override void VisitMethod(Method method)
        {
            if (!method.IsSpecialName)
            {
                CheckIfLongIdentifier(method.Name);
            }
            base.VisitMethod(method);
        }

        public override void VisitProperty(PropertyNode property)
        {
            CheckIfLongIdentifier(property.Name);
            base.VisitProperty(property);
        }

        public override void VisitField(Field field)
        {
            CheckIfLongIdentifier(field.Name);
            base.VisitField(field);
        }

        public override void VisitParameter(Parameter parameter)
        {
            CheckIfLongIdentifier(parameter.Name);
            base.VisitParameter(parameter);
        }

        public override void VisitLocal(Local local)
        {
            CheckIfLongIdentifier(local.Name);
            base.VisitLocal(local);
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
