using System.Collections.Generic;
using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop.FeatureEnvy
{
    public class MethodCallVisitor : BinaryReadOnlyVisitor
    {
        private readonly List<TypeNode> _collaborators = new List<TypeNode>();
        private readonly List<TypeNode> _enviedTypes = new List<TypeNode>();
        private readonly TypeNode _callingObjectType;

        public MethodCallVisitor(TypeNode callingObjectType)
        {
            _callingObjectType = callingObjectType;
        }

        public List<TypeNode> EnviedTypes
        {
            get { return _enviedTypes; }
        }

        public override void VisitMemberBinding(MemberBinding memberBinding)
        {
            base.VisitMemberBinding(memberBinding);
            var boundMember = memberBinding.BoundMember;
            if (boundMember.NodeType == NodeType.Method)
            {
                InspectCollaborator(memberBinding, boundMember);
            }
        }

        private void InspectCollaborator(MemberBinding memberBinding, Member boundMember)
        {
            var declaringType = boundMember.DeclaringType;
            if (declaringType != _callingObjectType && _callingObjectType.DeclaringModule.Types.Contains(declaringType))
            {
                CheckIfEnvied(declaringType);
                _collaborators.Add(declaringType);
            }
        }

        private void CheckIfEnvied(TypeNode declaringType)
        {
            if (_collaborators.Contains(declaringType))
            {
                _enviedTypes.Add(declaringType);
            }
        }
    }
}