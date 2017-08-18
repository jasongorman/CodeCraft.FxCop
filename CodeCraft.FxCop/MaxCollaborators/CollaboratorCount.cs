using System.Collections.Generic;
using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop.MaxCollaborators
{
    public class CollaboratorCount : BinaryReadOnlyVisitor, IMetric
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

        public override void VisitMemberBinding(MemberBinding binding)
        {
            AddCollaboratorType(binding.BoundMember.DeclaringType);
        }

        private void AddCollaboratorType(TypeNode type)
        {
            if (ToBeIncluded(type) && _type.DeclaringModule.Types.Contains(type))
                _collaborators.Add(type);
        }

        private bool ToBeIncluded(TypeNode type)
        {
            return type != _type && !_type.IsDerivedFrom(type);
        }

        public int Calculate(Node type)
        {
            return GetCollaboratorsFor((TypeNode)type).Count;
        }
    }
}