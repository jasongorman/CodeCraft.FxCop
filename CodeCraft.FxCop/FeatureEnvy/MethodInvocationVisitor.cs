using System.Collections.Generic;
using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop.FeatureEnvy
{
    public class MethodInvocationVisitor : BinaryReadOnlyVisitor
    {
        private readonly List<TypeNode> _collaborators = new List<TypeNode>();
        private readonly List<TypeNode> _enviedTypes = new List<TypeNode>();
        private readonly TypeNode _callingObjectType;

        public MethodInvocationVisitor(TypeNode callingObjectType)
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
                InspectCollaboratingType(memberBinding, boundMember);
            }
        }

        private void InspectCollaboratingType(MemberBinding memberBinding, Member boundMember)
        {
            var declaringType = boundMember.DeclaringType;
            if (declaringType != _callingObjectType && _callingObjectType.DeclaringModule.Types.Contains(declaringType))
            {
                if (_collaborators.Contains(declaringType))
                {
                    _enviedTypes.Add(declaringType);
                }
                _collaborators.Add(declaringType);
            }
        }
    }
}