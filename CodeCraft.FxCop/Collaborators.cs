using System.Collections.Generic;
using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop
{
    public class Collaborators : BinaryReadOnlyVisitor
    {
        private TypeNode _type;
        private HashSet<TypeNode> _collaborators;

        public HashSet<TypeNode> GetCollaboratorsFor(TypeNode type)
        {
            _type = type;
            _collaborators = new HashSet<TypeNode>();
            VisitMembers(type.Members);
            return _collaborators;
        }

        public override void VisitField(Field field)
        {
            AddCollaboratorType(field.Type);
        }

        public override void VisitParameter(Parameter parameter)
        {
            AddCollaboratorType(parameter.Type);
        }

        public override void VisitLocal(Local local)
        {
            AddCollaboratorType(local.Type);
        }

        private void AddCollaboratorType(TypeNode type)
        {
            if (IsProjectType(type))
                _collaborators.Add(type);
        }

        private bool IsProjectType(TypeNode fieldType)
        {
            return _type.DeclaringModule.Types.Contains(fieldType);
        }
    }
}